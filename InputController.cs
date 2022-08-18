using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static Action shootInput;
    public static Action reloadInput;
    public bool isShooting;
    public bool isReloading;
    public int activeWeapon = 0;
    public Transform[] weapons;


    private void Start()
    {
        if (weapons[0] != null)
        {
            weapons[0].gameObject.SetActive(true);
        }
        else
        {
            weapons[0].gameObject.SetActive(false);
            Debug.Log("No weapon in Array weapons[0] position, or weapon tag is not matching!");
        }
    }

    private void Update()
    {
        if (weapons[0].gameObject.CompareTag("Pistol") || weapons[0].gameObject.CompareTag("Shotgun"))
        {
            if (Input.GetKeyDown(KeyCode.Mouse0) && !isReloading)
            {
                isShooting = true;
                shootInput?.Invoke();
            }
            else
            {
                isShooting = false;
            }


            if (Input.GetKey(KeyCode.R))
            {
                reloadInput?.Invoke();
            }


        }
        if (weapons[0].gameObject.CompareTag("Assault Rifle") || weapons[0].gameObject.CompareTag("LMG"))
        {
            if (Input.GetKey(KeyCode.Mouse0) && !isReloading)
            {
                isShooting = true;
                shootInput?.Invoke();
            }
            else
            {
                isShooting = false;
            }


            if (Input.GetKey(KeyCode.R))
            {
                reloadInput?.Invoke();
            }


        }


        //if (Input.GetKey(KeyCode.Mouse0))
        //{
        //    isShooting = true;
        //    shootInput?.Invoke();
        //}
        //else
        //{
        //    isShooting = false;
        //}
    }

    void activetWeapon()
    {

        int i = 0;
        foreach (Transform weapon in weapons)
        {
            if (i == activeWeapon)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }
}
