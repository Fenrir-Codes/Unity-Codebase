using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateSentryGunNew : MonoBehaviour {

	public float rayDistance; //Distance till the ground
	//private RaycastHit hit;
	//private Transform myTransform;
	private bool canPlant;
	public GameObject sentryGun;
	public Transform sentrySpawnPos;
	public WeaponsPlayer playerWeapons;
	private bool trigger;

	/*public void Start()
	{
		myTransform = transform;
	}*/

	public void Update()
	{
		RaycastHit hit;
		if (!trigger)
		{
			if (Physics.Raycast(sentrySpawnPos.position, -Vector3.up, out hit, rayDistance))
			{
				canPlant = true;
			}
			else
			{
				canPlant = false;
			}
		}
		var gos = GetComponentsInChildren(typeof(Renderer));
		if (canPlant)
		{
			foreach (Renderer go in gos)
			{
				go.GetComponent<Renderer>().material.color = new Color(0.3f, 0.8f, 0.3f, 0.5f);
			}
		}
		else
		{
			foreach (Renderer go in gos)
			{
				go.GetComponent<Renderer>().material.color = new Color(1.0f, 0.0f, 0.0f, 0.5f);
			}
		}

		if (Input.GetButtonDown("Fire1"))
		{
			InstantSentryGun();
		}
	}

	public void InstantSentryGun()
	{
		if (canPlant)
		{
			Instantiate(sentryGun, sentrySpawnPos.position, sentrySpawnPos.rotation);
			if (playerWeapons.currentWeapon == 2) playerWeapons.SelectWeapon(playerWeapons.currentWeapon, 2);
			else playerWeapons.SelectWeapon(playerWeapons.currentWeapon, 1);
			playerWeapons.specialSelected = false;
			gameObject.SetActive(false);
		}
	}

	public void OnTriggerStay(Collider other)
	{
		canPlant = false;
		trigger = true;
	}

	public void OnTriggerExit(Collider other)
	{
		canPlant = true;
		trigger = false;
	}
}
