using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class LMGController : MonoBehaviour
{
    [SerializeField] GunData gunData;
    InputController InputController;
    float timeBetweenLastShot;
    private float hitForce = 50f;
    RaycastHit hit;

    [Header("Objects to attach to weapon")]
    [Tooltip("The blood spill effect spawn (object)")]
    [SerializeField] private GameObject[] bloodEffect;
    [Tooltip("The impact effect spawn (object)")]
    [SerializeField] private GameObject woodImpactEffect;
    [SerializeField] private GameObject stoneImpactEffect;
    [SerializeField] private GameObject metalImpactEffect;
    [Tooltip("Muzzle particle system (object)")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [Tooltip("MBulletShells particle system (object)")]
    [SerializeField] private ParticleSystem bulletShells;
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

    [Header("Set MIN and MAX Damage range")]
    public float minDamage = 28;
    public float maxDamage = 32f;

    private Animator animator;

    private bool CanShoot() => gunData.currentAmmo > 0 && timeBetweenLastShot >= 1f / (gunData.fireRate / 60f);
    private bool CanReload() => gunData.currentAmmo < gunData.magSize;

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
        InputController = GetComponentInParent<InputController>();
        InputController.isReloading = false;
    }

    #region OnEnable / OnDisable functions
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
    #endregion

    #region Update
    private void Update()
    {
        gunData.damage = Random.Range(minDamage, maxDamage);
        InputController.currentAmmo = gunData.currentAmmo;
        timeBetweenLastShot += Time.deltaTime;
        //Debug.DrawRay(Muzzle.position, Muzzle.forward, Color.red, 5, false);
    }
    #endregion

    #region Shoot function
    public void Shoot()
    {
        if (CanShoot())
        {
            muzzleFlash.Play();
            bulletShells.Play();
            fireSource.PlayOneShot(fireClip);

            if (Physics.Raycast(Muzzle.position, Muzzle.forward, out hit, gunData.range))
            {
                OnGunShoot();
                //Debug.Log("Hitted item: " +hit.transform.name);
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

    #region Reload function
    private void ReloadGun()
    {
        if (!InputController.isReloading)
        {
            StartCoroutine(Reload());
        }
    }
    #endregion

    #region Reload enumerator
    IEnumerator Reload()
    {
        InputController.isReloading = true;
        fireSource.PlayOneShot(reloadClip);

        yield return new WaitForSeconds(gunData.reloadTimeAR);

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
        else if (hit.transform.CompareTag("Head") || hit.transform.CompareTag("Torso") || hit.transform.CompareTag("Arm") || hit.transform.CompareTag("Leg"))
        {
            spawnEnemyBloodSpill(hit);
            EnemyAIController Enemy = hit.transform.GetComponentInParent<EnemyAIController>();

            if (hit.transform.CompareTag("Head"))
            {
                Enemy.takeDamage(Random.Range(95f, 100f));
            }
            else if (hit.transform.CompareTag("Torso"))
            {
                Enemy.takeDamage(Random.Range(45f, 48f));
            }
            else if (hit.transform.CompareTag("Arm"))
            {
                Enemy.takeDamage(Random.Range(26f, 33f));

            }
            else if (hit.transform.CompareTag("Leg"))
            {
                Enemy.takeDamage(Random.Range(22f, 28f));

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
        int randomBlood = Random.Range(0, bloodEffect.Length);
        ////blood effect
        GameObject bloodObject = Instantiate(bloodEffect[randomBlood], hit.point, Quaternion.LookRotation(hit.normal));
        bloodObject.transform.position += bloodObject.transform.forward / 1000;
        Destroy(bloodObject, 0.5f);

        //GameObject bloodObject = Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
        //bloodObject.transform.position += bloodObject.transform.forward / 1000;
        //Destroy(bloodObject, 0.5f);
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
