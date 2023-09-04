using HELLSLAYERCrosshairs;
using Kinemation.Recoilly;
using Kinemation.Recoilly.Runtime;
using UnityEngine;

public class InputController : MonoBehaviour
{
    #region Setting Up
    Gun Gun;
    private AudioSource m_audio;
    public int weaponIndex;
    [Header(" ----- Booleans -----")]
    public bool isEmptyHands;
    public bool canShoot;
    public bool canAim;
    public bool isAiming;
    public bool AdsUpHasPlayed = false;
    public bool AdsDownHasPlayed = false;
    public bool isReloading = false;
    public bool isShooting = false;

    //Private keycodes
    private KeyCode Firekey = KeyCode.Mouse0;
    private KeyCode Aim = KeyCode.Mouse1;
    private KeyCode ReloadKey = KeyCode.R;

    [Space]
    [Header("----- Weapons -----")]
    public GameObject[] weapons;
    [Space]
    [Header(" ----- AimPositions -----")]
    [SerializeField] private Transform defaultPos; // weaponholder
    [SerializeField] private Transform aimPistolPos; //position of the Pistol when aiming
    [SerializeField] private Transform aimSmgPos; //position of the Small Smg when aiming
    [SerializeField] private Transform aimShotgunPos; //position of the Shotgun when aiming
    [SerializeField] private Transform aimAssaultPos; //position of the Assault rifle when aiming
    [SerializeField] private Transform aimLmgPos; //position of the PightMachineGun when aiming
    [SerializeField] private float aimSpeed = 10f;
    [SerializeField] private float resetSpeed = 20f;
    private Vector3 ResetPos;
    [Header("------ Aim Sound ------")]
    public AudioClip aimUp;
    public AudioClip aimDown;

    [Space]
    [Header(" ----- Cameras -----")]
    [Tooltip("Both main camera and noclip camera here(If exist both). Need for use zoom function while aiming")]
    public Camera[] cameras;
    [SerializeField] private int Zoom = 35;
    [SerializeField] private float smooth = 3f;
    private int defaultFOV = 60;

    //-------- store Weapon types --------
    const string EmptyHand = "EmptyHands";
    const string Pistol = "Pistol";
    const string Smg = "Smg";
    const string AssaultRifle = "AssaultRifle";
    const string Shotgun = "Shotgun";
    const string Minigun = "Minigun";
    const string Sniper = "Sniper";
    const string Lmg = "lmg";
    #endregion

    #region start
    private void Start()
    {
        SetUp();
    }
    #endregion

    #region Update
    // Update is called once per frame
    private void Update()
    {
        activatedWeapon(weaponIndex);
        CheckForEmptyHands();
        PlayerInput();
    }
    #endregion

    #region Initialize
    private void SetUp()
    {
        ResetPos = Vector3.zero;
        m_audio = GetComponent<AudioSource>();
        canAim = false;
    }
    #endregion

    #region Input handling
    private void PlayerInput()
    {
        if (isEmptyHands == false)
        {
            Gun = getGun(weaponIndex);
            if (Gun != null)
            {
                isReloading = Gun.IsReloading();
                canShoot = Gun.CanShoot();
                if (Gun.WeaponType == Pistol || Gun.WeaponType == Shotgun)
                {
                    if (Input.GetKeyDown(Firekey) && Gun.CanShoot())
                    {
                        isShooting = true;
                        if (Gun.WeaponType == Shotgun)
                        {
                            Gun.ShootShotgun();
                        }
                        else
                        {
                            Gun.ShootGun();
                        }
                    }
                    else
                    {   
                        isShooting = false;

                        if (Input.GetKeyDown(Firekey) && Gun.MagEmpty())
                        {
                            Gun.playDryFire();
                        }
                    }
                }
                else
                {
                    if (Input.GetKey(Firekey) && Gun.CanShoot())
                    {
                        isShooting = true;
                        Gun.ShootGun();
                    }
                    else
                    {
                        isShooting = false;
                        if (Input.GetKeyDown(Firekey) && Gun.MagEmpty())
                        {
                            Gun.playDryFire();
                        }
                    }
                }

                if (Input.GetKey(ReloadKey) && Gun.CanReload())
                {  
                    Gun.ReloadGun();
                }
                if (Input.GetKey(Aim) && !isReloading)
                {

                    isAiming = true;
                }
                else
                {
                    isAiming = false;
                }

                AimGun();
                ZoomOnAim();

            }
            else
            {
                Debug.Log("No GUN found, maybe you have to attach it to the GameObject list.");
            }
        }
    }
    #endregion

    #region get gun by index of array
    private Gun getGun(int index)
    {
        Gun = weapons[weaponIndex].GetComponentInChildren<Gun>();
        return Gun;
    }
    #endregion

    #region checking the weapons if active or the hands are empty
    private void CheckForEmptyHands()
    {
        if (weapons != null && weapons.Length >= 0 && weaponIndex < weapons.Length)
        {
            if (weapons[weaponIndex].CompareTag(EmptyHand) || weaponIndex < 1)
            {
                isEmptyHands = true;
                canAim = false;
            }
            else
            {
                isEmptyHands = false;
                canAim = true;
            }
        }
        else
        {
            Debug.LogError("No weapons assigned or invalid weapon selection.");
        }
    }
    #endregion

    #region Function for check the activated weapon
    void activatedWeapon(int index)
    {
        index = 0;
        foreach (GameObject weapon in weapons)
        {
            //set the activated gameobject visible
            if (index == weaponIndex)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            index++;

        }

    }
    #endregion

    #region aim down the sight is the weapon can aim, and zoom the field of view
    void AimGun()
    {
        if (!isEmptyHands && canAim && isAiming)
        {
            AdsDownHasPlayed = false;
            if (!AdsUpHasPlayed)
            {
                m_audio.PlayOneShot(aimUp);
                AdsUpHasPlayed = true;
            }
            if (Gun.WeaponType == Pistol)
            {
                defaultPos.localPosition = Vector3.Lerp(defaultPos.localPosition, aimPistolPos.localPosition, aimSpeed * Time.deltaTime);
            }
            if (Gun.WeaponType == AssaultRifle)
            {
                defaultPos.localPosition = Vector3.Lerp(defaultPos.localPosition, aimAssaultPos.localPosition, aimSpeed * Time.deltaTime);
            }
            if (Gun.WeaponType == Smg)
            {
                defaultPos.localPosition = Vector3.Lerp(defaultPos.localPosition, aimSmgPos.localPosition, aimSpeed * Time.deltaTime);
            }
            if (Gun.WeaponType == Lmg)
            {
                defaultPos.localPosition = Vector3.Lerp(defaultPos.localPosition, aimLmgPos.localPosition, aimSpeed * Time.deltaTime);
            }
            if (Gun.WeaponType == Shotgun)
            {
                defaultPos.localPosition = Vector3.Lerp(defaultPos.localPosition, aimShotgunPos.localPosition, aimSpeed * Time.deltaTime);
            }
        }
        else
        {
            AdsUpHasPlayed = false;
            if (!AdsDownHasPlayed)
            {
                m_audio.PlayOneShot(aimDown);
                AdsDownHasPlayed = true;
            }
            defaultPos.localPosition = Vector3.Lerp(defaultPos.localPosition, ResetPos, resetSpeed * Time.deltaTime);

            if (defaultPos.localPosition != ResetPos)
            {
                defaultPos.localPosition = Vector3.zero;
            }
        }

    }
    #endregion

    #region camera zoom on aiming
    private void ZoomOnAim()
    {
        if (!isAiming)
        {
            foreach (Camera c in cameras)
            {
                c.fieldOfView = Mathf.Lerp(c.fieldOfView, defaultFOV, Time.deltaTime * smooth);
            }
        }
        else
        {
            //zoomed field of view of the cameras
            foreach (Camera c in cameras)
            {
                c.fieldOfView = Mathf.Lerp(c.fieldOfView, Zoom, Time.deltaTime * smooth);
            }
        }
    }
    #endregion
}
