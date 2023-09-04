using Kinemation.Recoilly;
using Kinemation.Recoilly.Runtime;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
    #region Setup
    private Recoil recoil;
    RaycastHit hit;
    [Header("Script of the gun data")]
    [SerializeField] GunData gunData;
    //[Header("Muzzle transform - Main Camera")]
    //[Tooltip("Muzzle transform here(Main Camera)")]
    [SerializeField] private Transform Muzzle;
    [SerializeField] private ParticleSystem MuzzleSmoke;
    [SerializeField] private ParticleSystem BulletShells;
    [SerializeField] private ParticleSystem MuzzleFlash;
    public TrailRenderer bulletTrail;
    [SerializeField] private GameObject ConcreteImpactEffect;
    [SerializeField] private GameObject MetalImpactEffect;
    [SerializeField] private GameObject WoodImpactEffect;
    [SerializeField] private GameObject DirtImpactEffect;
    private AudioSource m_audioSource;
    private AudioClip m_clip;
    [Space]
    public string Name;
    public int CurrentAmmo = 0;
    public int AmmoReserves = 0;
    public Sprite GunImage;
    public string WeaponType;

    public bool isShooting;
    public bool isReloading;

    private float inAccuracy;
    private int gaugePerShot = 12;

    #region inherited settings
    //Recoil
    [HideInInspector] public float recoilX;
    [HideInInspector] public float recoilY;
    [HideInInspector] public float recoilZ;

    [HideInInspector] public float aimrecoilX;
    [HideInInspector] public float aimrecoilY;
    [HideInInspector] public float aimrecoilZ;


    [HideInInspector] public float recoilPosX;
    [HideInInspector] public float recoilPosY;
    [HideInInspector] public float recoilPosZ;

    [HideInInspector] public float aimrecoilPosX;
    [HideInInspector] public float aimrecoilPosY;
    [HideInInspector] public float aimrecoilPosZ;
    #endregion

    public bool CanShoot() => !gunData.isReloading && gunData.currentAmmo > 0 && gunData.timeBetweenLastShot >= 1f / (gunData.fireRate / 60f) && gunData.name != "EmptyHands";
    public bool CanReload() => gunData.currentAmmo < gunData.magazineSize && gunData.ammoReserves > 0;
    public bool IsReloading() => gunData.isReloading;
    public bool MagEmpty() => gunData.currentAmmo == 0 && gunData.timeBetweenLastShot >= .5f;

    #endregion

    #region Start
    private void Start()
    {
        InitializeComponents();
    }
    #endregion

    #region Update
    void Update()
    {
        gunData.timeBetweenLastShot += Time.deltaTime;
        CurrentAmmo = gunData.currentAmmo;
        AmmoReserves = gunData.ammoReserves;
        GunImage = gunData.GunImage;
        Name = gunData.name;
        isReloading = gunData.isReloading;
    }
    #endregion

    #region Initialize
    private void InitializeComponents()
    {
        recoil = GetComponentInParent<Recoil>();
        m_audioSource = GetComponentInParent<AudioSource>();
        SetupGunData();
    }
    #endregion

    #region Setup data on weapon
    private void SetupGunData()
    {
        Name = gunData.name;
        GunImage = gunData.GunImage;
        WeaponType = gunData.TypeOfWeapon.ToString();
        gunData.ammoReserves = gunData.maxAmmoReserves;
        inAccuracy = gunData.inAccuracy;

        recoilX = gunData.recoilX;
        recoilY = gunData.recoilY;
        recoilZ = gunData.recoilZ;
        aimrecoilX = gunData.aimrecoilX;
        aimrecoilY = gunData.aimrecoilY;
        aimrecoilZ = gunData.aimrecoilZ;


        if (IsReloading() == true)
        {
            gunData.isReloading = false;
        }
    }
    #endregion

    #region Shoot function
    public void ShootGun()
    {
        if (CanShoot())
        {
            MuzzleFlash.Play();
            MuzzleSmoke.Play();
            recoil.RecoilFire();
            BulletShells.Play();
            m_clip = gunData.fireClip[Random.Range(0, gunData.fireClip.Length)];
            m_audioSource.PlayOneShot(m_clip);

            if (Physics.Raycast(Muzzle.transform.position, GetShootingDirection(), out hit, gunData.range))
            {

                TrailRenderer trail = Instantiate(bulletTrail, Muzzle.transform.position, Quaternion.identity);
                StartCoroutine(BulletTrail(trail, hit));
                OnHitMaterial(hit);
            }

            gunData.currentAmmo--;
            gunData.timeBetweenLastShot = 0.0f;
        }

    }
    #endregion

    #region shotgun shoot function
    public void ShootShotgun()
    {
        if (CanShoot())
        {
           
            MuzzleFlash.Play();
            MuzzleSmoke.Play();
            m_clip = gunData.fireClip[Random.Range(0, gunData.fireClip.Length)];
            m_audioSource.PlayOneShot(m_clip);

            for (int i = 0; i < gaugePerShot; i++)
            {
                if (Physics.Raycast(Muzzle.position, GetShootingDirection(), out hit, gunData.range))
                {
                    OnHitMaterial(hit);
                }
            }
            gunData.currentAmmo--;
            gunData.timeBetweenLastShot = 0.0f;

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

    #region reload
    public void ReloadGun()
    {
        StartCoroutine(Reload());
    }

    IEnumerator Reload()
    {
        gunData.isReloading = true;
        m_clip = gunData.reloadClip;
        m_audioSource.clip = m_clip;
        m_audioSource.Play();
        yield return new WaitForSeconds(gunData.reloadTime);

        int reloadAmmount = gunData.magazineSize - gunData.currentAmmo; // how many bullets to refill to magazine
                                                                        // check if we have enough ammo in reserves to refill
        reloadAmmount = (gunData.ammoReserves - reloadAmmount) >= 0 ? reloadAmmount : gunData.ammoReserves;
        gunData.currentAmmo += reloadAmmount;
        gunData.ammoReserves -= reloadAmmount;

        if (gunData.currentAmmo > gunData.magazineSize)
        {
            gunData.currentAmmo = gunData.magazineSize;
        }
        gunData.isReloading = false;
    }
    #endregion

    #region Ammo refill function
    public void refillAmmoReserves(int bulletAmount)
    {
        gunData.ammoReserves += bulletAmount;
        if (gunData.ammoReserves > gunData.maxAmmoReserves)
        {
            gunData.ammoReserves = gunData.maxAmmoReserves;
        }
    }
    #endregion

    #region Check material types on hit
    private void OnHitMaterial(RaycastHit hit)
    {

        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-hit.normal * gunData.hitForce);
        }
        else if (hit.transform)
        {
            //Debug.Log(hit.transform.name);
            spawnImpactEffect(hit);
        }
        else
        {
            Debug.Log("Nothing found");
        }
    }
    #endregion

    #region spawnImpact code
    void spawnImpactEffect(RaycastHit hit)
    {
        if (hit.collider.CompareTag("Concrete"))
        {
            GameObject impact = Instantiate(ConcreteImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            impact.transform.position += impact.transform.forward / 1000;

        }
        else if (hit.collider.CompareTag("Wood"))
        {
            GameObject impact = Instantiate(WoodImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            impact.transform.position += impact.transform.forward / 1000;

        }
        else if (hit.collider.CompareTag("Metal"))
        {
            GameObject impact = Instantiate(MetalImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            impact.transform.position += impact.transform.forward / 1000;

        }
        else
        {
            GameObject impact = Instantiate(DirtImpactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            impact.transform.position += impact.transform.forward / 1000;

        }
    }
    #endregion

    #region Bullet Trail
    IEnumerator BulletTrail(TrailRenderer Trail, RaycastHit Hit)
    {
        float time = 0f;
        Vector3 startPosition = Muzzle.transform.position;

        while (time < 1)
        {
            Trail.transform.position = Vector3.Lerp(startPosition, Hit.point, time);
            time += Time.deltaTime / Trail.time;

            yield return null;
        }

        Trail.transform.position = Hit.point;
        Destroy(Trail.gameObject, Trail.time);

    }
    #endregion

    public void playDryFire()
    {
        if (MagEmpty())
        {
            m_clip = gunData.dryFireClip[Random.Range(0, gunData.dryFireClip.Length)];
            m_audioSource.PlayOneShot(m_clip);
            gunData.timeBetweenLastShot = 0.0f;
        }
    }
}



