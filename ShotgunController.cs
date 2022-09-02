using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShotgunController : MonoBehaviour
{
    [SerializeField] GunData gunData;
    InputController InputController;
    float timeBetweenLastShot;
    private float hitForce = 80f;
    RaycastHit hit;

    [Header("Objects to attach to weapon")]
    [Tooltip("The blood spill effect spawn (object)")]
    [SerializeField] private GameObject bloodEffect;
    [Tooltip("The impact effect spawn (object)")]
    [SerializeField] private GameObject woodImpactEffect;
    [SerializeField] private GameObject stoneImpactEffect;
    [SerializeField] private GameObject metalImpactEffect;
    [Tooltip("Muzzle particle system (object)")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [Tooltip("Source of the fire sound effect (weapon)")]
    [SerializeField] private AudioSource fireSource;
    [Tooltip("Reload sound")]
    [SerializeField] private AudioClip reloadClip;
    [Tooltip("Shooting sound")]
    [SerializeField] private AudioClip fireClip;

    [Header("Muzzle transform - Main Camera")]
    [Tooltip("Muzzle transform here(Main Camera)")]
    [SerializeField] private Transform Muzzle;
    [Header("Muzzle transform(Muzzle object in weapons)")]
    [Tooltip("Muzzle transform here(Muzzle object)")]
    public TrailRenderer tracerEffect;
    public Transform TrailCastOrigin;

    [Header("Set FIXED damage to shotgun")]
    public float fixDamage = 22;


    [Header("InAccuracy of the shotgun")]
    [SerializeField] private float inAccuracy = 4f;
    [Header("Gauge / Shotgun shell")]
    [SerializeField] private int gaugePerShot = 12;

    private bool CanShoot() => gunData.currentAmmo > 0 && timeBetweenLastShot >= 1f / (gunData.fireRate / 60f);
    private bool CanReload() => gunData.currentAmmo < gunData.magSize;


    // Start is called before the first frame update
    void Start()
    {
        InputController = GetComponentInParent<InputController>();
        InputController.isReloading = false;
    }

    // Update is called once per frame
    void Update()
    {
        gunData.damage = fixDamage;
        InputController.currentAmmo = gunData.currentAmmo;
        timeBetweenLastShot += Time.deltaTime;

        setInaccuracyWhileAiming();

    }

    private void OnEnable()
    {
        InputController.shootInput += Shoot;
        InputController.reloadInput += ReloadGun;
    }
    private void OnDisable()
    {
        InputController.shootInput -= Shoot;
        InputController.reloadInput -= ReloadGun;
    }

    #region setting the inaccuracy while aiming (less spread of the gauges)
    void setInaccuracyWhileAiming()
    {
        if (InputController.isAiming)
        {
            inAccuracy = 2.5f;
        }
        else
        {
            inAccuracy = 4f;
        }
    }
    #endregion

    #region shoot function
    public void Shoot()
    {
        if (CanShoot())
        {
            muzzleFlash.Play();
            fireSource.PlayOneShot(fireClip);

            for (int i = 0; i < gaugePerShot; i++)
            {
                if (Physics.Raycast(Muzzle.position, GetShootingDirection(), out hit, gunData.range))
                {
                    OnGunShoot();
                    // Debug.Log("Hitted item: " + hit.transform.name);
                }
            }
            gunData.currentAmmo--;
            timeBetweenLastShot = 0;

        }


        if (CanReload())
        {
            Reload();
        }

    }
    #endregion

    #region calculating the inaccuracy for shotgun (bullet spread)
    Vector3 GetShootingDirection()
    {
        Vector3 targetPosition = Muzzle.position + Muzzle.forward * gunData.range;
        targetPosition = new Vector3(
            targetPosition.x + Random.Range(-inAccuracy, inAccuracy),
            targetPosition.y + Random.Range(-inAccuracy, inAccuracy),
            targetPosition.z + Random.Range(-inAccuracy, inAccuracy)
            );
        Vector3 direction = targetPosition - Muzzle.position;
        return direction.normalized;
    }
    #endregion

    #region reload function
    private void ReloadGun()
    {
        if (!InputController.isReloading)
        {
            StartCoroutine(Reload());
        }
    }
    #endregion

    #region reload enumerator
    IEnumerator Reload()
    {
        InputController.isReloading = true;
        fireSource.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(gunData.reloadTimeShotgun);

        gunData.currentAmmo = gunData.magSize;

        InputController.isReloading = false;

    }
    #endregion

    #region Checking tags with Raycast then instantiate bloodspill or other effects
    private void OnGunShoot()
    {
        //if hit hits rigidbody add force to it
        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-hit.normal * hitForce);
        }

        //If the objet we are hitting tagged as Enemy spawn blood splash instead of bullet impact
        if (hit.transform.CompareTag("Enemy"))
        {
            spawnEnemyBloodSpill(hit);
            EnemyAIController Enemy = hit.transform.GetComponent<EnemyAIController>();

            if (Enemy != null)
            {
                Enemy.takeDamage(gunData.damage);
            }
        }
        else if (hit.transform.CompareTag("Wood"))
        {
            spawnWoodImpactEffect(hit);
        }
        else if (hit.transform.CompareTag("Metal"))
        {
            spawnMetalImpactEffect(hit);
        }
        else if (hit.transform.CompareTag("Damageable"))
        {
            spawnWoodImpactEffect(hit);

            CrashCrateController crate = GameObject.FindGameObjectWithTag("Damageable").transform.GetComponent<CrashCrateController>();
            if (crate)
            {
                crate.crashTheCrate();
            }
            else
            {
                Debug.Log("Crash crate not found! Missing?");
            }

        }
        else if (hit.transform.CompareTag("Stone") || hit.transform.CompareTag("Gravel"))
        {
            spawnStoneImpactEffect(hit);
        }
        else if (hit.transform.CompareTag("StonePillar"))
        {
            ColumnBreakScript column = hit.transform.GetComponentInParent<ColumnBreakScript>();
            spawnStoneImpactEffect(hit);
            column.BreakColumn();
        }
        else
        {
            //spawnStoneImpactEffect(hit);
        }
        spawnBulletTracer(hit);
    }
    #endregion

    #region spawnMetalImpact code
    void spawnMetalImpactEffect(RaycastHit hit)
    {
        GameObject MetalObject = Instantiate(metalImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        MetalObject.transform.position += MetalObject.transform.forward / 1000;
        Destroy(MetalObject, 3f);
    }
    #endregion

    #region spawnBloodSpill code
    void spawnEnemyBloodSpill(RaycastHit hit)
    {
        //blood effect
        GameObject bloodObject = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
        bloodObject.transform.position += bloodObject.transform.forward / 1000;
        Destroy(bloodObject, 0.5f);
    }
    #endregion

    #region spawn Wood Impact code
    void spawnWoodImpactEffect(RaycastHit hit)
    {
        GameObject woodObject = Instantiate(woodImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        woodObject.transform.position += woodObject.transform.forward / 1000;
        Destroy(woodObject, 3f);
    }
    #endregion

    #region spawn Stone Impact code
    void spawnStoneImpactEffect(RaycastHit hit)
    {
        GameObject stoneObject = Instantiate(stoneImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        stoneObject.transform.position += stoneObject.transform.forward / 1000;
        Destroy(stoneObject, 3f);
    }
    #endregion

    #region spawn bullet tracer effect

    void spawnBulletTracer(RaycastHit hit)
    {
        TrailRenderer trail = Instantiate(tracerEffect, TrailCastOrigin.position, Quaternion.identity);
        trail.AddPosition(TrailCastOrigin.position);

        trail.transform.position = hit.point;

    }

    #endregion

}
