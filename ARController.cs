using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

//Assaul rifle controller
public class ARController : MonoBehaviour
{
    [SerializeField] GunData gunData;
    InputController InputController;
    float timeBetweenLastShot;
    private float hitForce = 40f;
    RaycastHit hit;

    [Header("Objects to attach to weapon")]
    [Header("Hitmarker")]
    [SerializeField] private GameObject headshotMarker;
    [SerializeField] private AudioClip headMarkerClip;
    [SerializeField] private GameObject hitMarker;
    [SerializeField] private AudioClip hitMarkerClip;
    [Space]
    [Tooltip("The blood spill effect spawn (object)")]
    [SerializeField] private GameObject[] bloodEffect;
    [Tooltip("The impact effect spawn (object)")]
    [SerializeField] private GameObject woodImpactEffect;
    [SerializeField] private GameObject stoneImpactEffect;
    [SerializeField] private GameObject metalImpactEffect;
    [Space]
    [Tooltip("Muzzle particle system (object)")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [Tooltip("MBulletShells particle system (object)")]
    [SerializeField] private ParticleSystem bulletShells;
    [Space]
    [Tooltip("Source of the fire sound effect (weapon)")]
    [SerializeField] private AudioSource fireSource;
    [Tooltip("Reload sound")]
    [SerializeField] private AudioClip reloadClip;
    [Tooltip("Shooting sound")]
    [SerializeField] private AudioClip fireClip;
    [Space]
    [Header("Muzzle transform - Main Camera")]
    [Tooltip("Muzzle transform here(Main Camera)")]
    [SerializeField] private Transform Muzzle;
    [Header("Muzzle transform(Muzzle object in weapons)")]
    public TrailRenderer tracerEffect;
    [Tooltip("Muzzle transform here(Muzzle object)")]
    public Transform TrailCastOrigin;

    [Header("Set MIN and MAX Damage range")]
    public float minDamage = 30;
    public float maxDamage = 36f;

    private Animator animator;

    private int setDefaultAmmoReserve = 150;

    private bool CanShoot() => timeBetweenLastShot >= 1f / (gunData.fireRate / 60f);
    private bool CanReload() => gunData.currentAmmo < gunData.magSize && gunData.ammoReserves > 0;

    private void Start()
    {
        Initialize();
    }

    #region initialize
    private void Initialize()
    {
        gunData.ammoReserves = setDefaultAmmoReserve;
        hitMarker.SetActive(false);
        animator = GetComponentInChildren<Animator>();
        InputController = GetComponentInParent<InputController>();
        InputController.isReloading = false;
    }
    #endregion

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

    #region update
    private void Update()
    {
        gunData.damage = Random.Range(minDamage, maxDamage);
        InputController.currentAmmo = gunData.currentAmmo;
        InputController.ammoReserves = gunData.ammoReserves;
        timeBetweenLastShot += Time.deltaTime;
        //Debug.DrawRay(Muzzle.position, Muzzle.forward, Color.red, 5, false);
    }
    #endregion

    #region shoot function
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
    }
    #endregion

    #region reload function
    private void ReloadGun()
    {
        if (!InputController.isReloading && CanReload())
        {
            StartCoroutine(Reload());
        }
    }
    #endregion

    #region reload enumerator
    IEnumerator Reload()
    {
        InputController.isReloading = true;
        yield return new WaitForSeconds(gunData.reloadTimeAR);

        int reloadAmmount = gunData.magSize - gunData.currentAmmo; // how many bullets to refill to magazine
                                                                   // check if we have enough ammo in reserves to refill
        reloadAmmount = (gunData.ammoReserves - reloadAmmount) >= 0 ? reloadAmmount : gunData.ammoReserves;
        gunData.currentAmmo += reloadAmmount;
        gunData.ammoReserves -= reloadAmmount;

        if (gunData.currentAmmo > gunData.magSize)
        {
            gunData.currentAmmo = gunData.magSize;
        }
        //gunData.currentAmmo = gunData.magSize;
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
        else if (hit.transform.CompareTag("Enemy") || hit.transform.CompareTag("Head") || hit.transform.CompareTag("Torso") || hit.transform.CompareTag("Arm") || hit.transform.CompareTag("Leg"))
        {
            spawnEnemyBloodSpill(hit);
            EnemyAIController Enemy = hit.transform.GetComponentInParent<EnemyAIController>();

            MarkerActive();
            Invoke("disableMarker", 0.1f);

            if (hit.transform.CompareTag("Head"))
            {
                markHeadshot();
                Invoke("disableHeadMarker", 1.057f);
                Enemy.takeDamage(Random.Range(99f, 100f));

            }
            else if (hit.transform.CompareTag("Torso"))
            {
                Enemy.takeDamage(Random.Range(40f, 45f));
            }
            else if (hit.transform.CompareTag("Arm"))
            {
                Enemy.takeDamage(Random.Range(20f, 26f));
            }
            else if (hit.transform.CompareTag("Leg"))
            {
                Enemy.takeDamage(Random.Range(22f, 26f));
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
            //disableMarker();
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

    #region Hit Marker
    void MarkerActive()
    {
        fireSource.PlayOneShot(hitMarkerClip);
        hitMarker.SetActive(true);
    }

    void markHeadshot()
    {
        headshotMarker.SetActive(true);
        fireSource.PlayOneShot(headMarkerClip);
    }

    void disableHeadMarker()
    {
        headshotMarker.SetActive(false);
    }

    void disableMarker()
    {
        hitMarker.SetActive(false);
    }
    #endregion

}
