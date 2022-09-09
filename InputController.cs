using System;
using System.Collections;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;

    [Header("Currently active weapon (array 0,1,2,3,4)")]
    public int activeWeapon = 0;
    [Space]
    [Header(" ----- Booleans -----")]
    public bool canShoot;
    private bool isSemiAuto;
    public bool isShooting;
    public bool isReloading;
    private bool canAim;
    public bool isAiming;
    [Space]
    [Header("Current amount of ammo in the magazine")]
    public int currentAmmo;
    [Space]
    [Header("Default and Aim position transforms")]
    public Transform defaultPosition;
    public Transform aimPosition;
    [Space]
    [Header(" ----- Weapons -----")]
    public Transform[] weapons;
    [Space]
    private float aimSpeed = 15f;
    private float waitSemiShootInterval = 0.450f;
    Vector3 defaultPos = new Vector3(0f, 0f, 0f);


    [Header(" ----- Cameras -----")]
    [Tooltip("Both main camera and noclip camera here. Need for use zoom function while aiming")]
    public Camera[] cameras;
    [SerializeField] private int Zoom = 5;
    [SerializeField] private float smooth = 3f;
    private int defaultFOV = 60;

    private void Start()
    {
        canShoot = true;
        activeWeapon = 0;
    }

    private void Update()
    {
        activatedWeapon();
        aimDownTheSight();
    }

    #region Function for check the activated weapon
    void activatedWeapon()
    {
        int i = 0;
        foreach (Transform weapon in weapons)
        {
            //set the activated gameobject visible
            if (i == activeWeapon)
            {
                weapon.gameObject.SetActive(true);

                // if the weapons are pistol or Shotgun the weapons are semi-automatic weapons
                if (weapons[i].CompareTag("Pistol") || weapons[i].CompareTag("Shotgun"))
                {
                    isSemiAuto = true;
                }
                else
                {
                    isSemiAuto = false;
                }

                // check wich weapon can aim down the sight
                if (weapons[i].CompareTag("Pistol") || weapons[i].CompareTag("Shotgun")
                || weapons[i].CompareTag("Assault Rifle") || weapons[i].CompareTag("LMG"))
                {
                    canAim = true;
                }
                else
                {
                    canAim = false;
                }

            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;

        }

        checkActivatedWeaponType(i - 1);

    }
    #endregion

    #region Checking the weapon type semi or full-auto reload and can aim
    void checkActivatedWeaponType(int i)
    {
        if (isSemiAuto)
        {
            StartCoroutine(shootSemiGun());
        }

        if (!isSemiAuto)
        {
            shootFullAutoGun();
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }

        if (Input.GetKey(KeyCode.R))
        {
            reloadInput?.Invoke();
        }

    }
    #endregion

    #region shooting function for semi-auto gun
    IEnumerator shootSemiGun()
    {
        if (currentAmmo > 0 && !isReloading)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && canShoot == true)
            {
                isShooting = true;
                canShoot = false;
                shootInput?.Invoke();
                yield return new WaitForSeconds(waitSemiShootInterval);
                isShooting = false;
                canShoot = true;
            }
        }
    }
    #endregion

    #region shooting function for full auto type weapons
    void shootFullAutoGun()
    {
        if (Input.GetKey(KeyCode.Mouse0) && !isReloading && currentAmmo > 0)
        {
            isShooting = true;
            shootInput?.Invoke();
        }
        else
        {
            isShooting = false;
        }
    }
    #endregion

    #region aim down the sight is the weapon can aim, and zoom the field of view
    void aimDownTheSight()
    {
        if (canAim && isAiming)
        {
            //aiming position of the transforms
            defaultPosition.localPosition = Vector3.Lerp(defaultPosition.localPosition, aimPosition.localPosition, aimSpeed * Time.deltaTime);

            //zoomed field of view of the cameras
            foreach (Camera c in cameras)
            {
                c.fieldOfView = Mathf.Lerp(c.fieldOfView, Zoom, Time.deltaTime * smooth);
            }


        }
        else
        {
            //default position of the transforms
            defaultPosition.localPosition = Vector3.Lerp(defaultPosition.localPosition, defaultPos, aimSpeed * Time.deltaTime);

            //default field of view of the cameras
            foreach (Camera c in cameras)
            {
                c.fieldOfView = Mathf.Lerp(c.fieldOfView, defaultFOV, Time.deltaTime * smooth);
            }
        }
    }
    #endregion



}