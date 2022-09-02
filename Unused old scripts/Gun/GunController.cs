using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GunController : MonoBehaviour
{
    [Header("weapon settings")]
    [Tooltip("Weapon danage")]
    [SerializeField, Range(1, 100)] private int damage = 12;
    [Tooltip("Weapon range in meters")]
    [SerializeField] private float range = 500f;
    [Tooltip("Rate of fire (fire rate is 600 / min for the ak so we have to divide 600 with 60 = 10 rounds/sec)")]
    [SerializeField] private float fireRate = 10f; // fire rate is 600 / min for the ak so we have to divide 600 with 60 = 10 rounds/sec
    [Tooltip("Source of the fire sound effect (weapon)")]
    [SerializeField] private AudioSource fireSource;
    [Tooltip("Shooting sound")]
    [SerializeField] private AudioClip fireClip;
    [Tooltip("Reload sound")]
    [SerializeField] private AudioClip reloadClip;
    [Tooltip("AmmoPickUp Sound")]
    [SerializeField] private AudioClip ammoPickUpClip;

    //objects
    [Header("Objects to attach to weapon")]
    [Tooltip("The blood spill effect spawn (object)")]
    [SerializeField] private GameObject bloodEffect;
    [Tooltip("The impact effect spawn (object)")]
    [SerializeField] private GameObject woodImpactEffect;
    [SerializeField] private GameObject stoneImpactEffect;
    [Tooltip("Muzzle particle system (object)")]
    [SerializeField] private ParticleSystem muzzleFlash;
    [Tooltip("MBulletShells particle system (object)")]
    [SerializeField] private ParticleSystem bulletShells;

    public TrailRenderer tracerEffect;
    public Transform Raycastorigin;

    [Tooltip("Text shown on the right corner (Ammo in the magazine)")]
    [SerializeField] public Text ammoDisplay;
    [SerializeField] public Text ammoReserveDisplay;
    [Header("Muzzle")]
    [Tooltip("Main Camera find place here")]
    public Camera cameraAsMuzzle;

    private float hitForce = 30f;
    private float fireDelay = 0.5f;

    [HideInInspector]
    public int MaxAmmo = 150;
    [HideInInspector]
    public int CurrentMagazine = 0;
    [HideInInspector]
    public int AmmoReserve = 0;
    [HideInInspector]
    public int MaxMagazineSize = 31;

    public bool isShooting = false;
    public bool isReloading = false;
    public bool canShoot = false;

    private KeyCode FireButton = KeyCode.Mouse0;


    //Ray ray;
    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        cameraAsMuzzle = Camera.main;
        AmmoReserve = MaxAmmo;
        CurrentMagazine = MaxMagazineSize;
    }

    private void Update()
    {
        checkAmmoReserves();
        UpdateTexts();
        checkIfCanShootOrNot();
        AmmoDisplay();
    }

    #region Shooting script
    public void Shoot()
    {
        if (Input.GetKey(FireButton) && CurrentMagazine != 0)
        {
            isShooting = true;
            if (Time.time >= fireDelay)
            {
                fireDelay = Time.time + 1f / fireRate;
                muzzleFlash.Play();
                bulletShells.Play();
                fireSource.PlayOneShot(fireClip);
                //count ammo
                CurrentMagazine--;

                checkTagsOnHit();

            }
        }
        else
        {
            isShooting = false;
        }


    }
    #endregion

    #region on shootig checkint the hits
    private void checkTagsOnHit()
    {
        //raycast shoot
        if (Physics.Raycast(cameraAsMuzzle.transform.position, cameraAsMuzzle.transform.forward, out hit, range))
        {
            //Debug.Log("Target hit: " + hit.transform.name);

            //if hit hits rigidbody add force to it
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }

            //If the objet we are hitting tagged as Enemy spawn blood splash instead of bullet impact
            if (hit.transform.tag == "Enemy")
            {
                spawnEnemyBloodSpill(hit);
                EnemyAIController Enemy = hit.transform.GetComponent<EnemyAIController>();

                if (Enemy != null)
                {
                    Enemy.takeDamage(damage);
                }
            }
            else if (hit.transform.tag == "Wood")
            {
                spawnWoodImpactEffect(hit);
            }
            else if (hit.transform.tag == "Damageable")
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
            else if (hit.transform.tag == "Stone")
            {
                spawnStoneImpactEffect(hit);
            }
            else if (hit.transform.tag == "StonePillar")
            {
                ColumnBreakScript column = hit.transform.GetComponentInParent<ColumnBreakScript>();
                spawnStoneImpactEffect(hit);
                column.BreakColumn();
            }
            else
            {
                spawnStoneImpactEffect(hit);
            }
            spawnBulletTracer(hit);

        }
    }
    #endregion

    #region Ammo refill function
    public void refillAmmoReserves(int ammoAmount)
    {
        //fireSource.PlayOneShot(ammoPickUpClip);
        AmmoReserve += ammoAmount;
        if (AmmoReserve > MaxAmmo)
        {
            AmmoReserve = MaxAmmo;
        }
    }
    #endregion

    #region Reload script
    public IEnumerator Reload()
    {
        isReloading = true;
        canShoot = false;
        isShooting = false;
        fireSource.PlayOneShot(reloadClip);
        yield return new WaitForSeconds(2.146f);
        int reloadAmmount = MaxMagazineSize - CurrentMagazine; // how many bullets to refill to magazine
                                                               // check if we have enough ammo in reserves to refill
        reloadAmmount = (AmmoReserve - reloadAmmount) >= 0 ? reloadAmmount : AmmoReserve;
        CurrentMagazine += reloadAmmount;
        AmmoReserve -= reloadAmmount;

        if (CurrentMagazine > MaxMagazineSize)
        {
            CurrentMagazine = MaxMagazineSize;
        }
        isReloading = false;
        canShoot = true;
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
        TrailRenderer trail = Instantiate(tracerEffect, Raycastorigin.position, Quaternion.identity);
        trail.AddPosition(Raycastorigin.position);

        trail.transform.position = hit.point;

    }

    #endregion

    #region Check if Ammo reserve has ammo
    void checkAmmoReserves()
    {
        if (AmmoReserve <= 0)
        {
            AmmoReserve = 0;
        }
        if (CurrentMagazine <= 0)
        {
            isShooting = false;
            canShoot = false;
        }
    }
    #endregion

    #region update text fields
    void UpdateTexts()
    {
        ammoDisplay.text = $"{CurrentMagazine}";
        ammoReserveDisplay.text = $"{AmmoReserve}";
    }
    #endregion

    #region AmmoDisplay
    void AmmoDisplay()
    {
        for (int i = 0; i <= CurrentMagazine; i++)
        {
            ammoDisplay.text = new string('|', i);
        }
    }
    #endregion

    #region check if when we can shoot the weapon and when not
    void checkIfCanShootOrNot()
    {
        if (CurrentMagazine > 0 && isReloading == false)
        {
            canShoot = true;
        }
        else
        {
            canShoot = false;
        }
    }
    #endregion
}
