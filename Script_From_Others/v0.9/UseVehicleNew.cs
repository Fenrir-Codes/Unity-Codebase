using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseVehicleNew : MonoBehaviour {

	public GameObject controller;
	public bool inCar;
	public GameObject vehicleScript;
	public GameObject vehicle;
	public GameObject hands;
	//var body : GameObject;
	public bool waitTime;
	public Transform getOutPosition;
	public AudioClip startEngine;
	public AudioClip handbrake;
	public Transform seat;
	public string TargetName = "PlayerController";
	public void Start()
	{
		controller = GameObject.Find(TargetName);
	}

	/*
function OnTriggerStay (other : Collider) {
	if (other.gameObject.name == "CODcontroller"){
		if (Input.GetKey("e")) Action(1);
	}	
}
*/
	public void Update()
	{
		if (!inCar)
		{
			return;
		}
		if (Input.GetKey("e"))
		{
			StartCoroutine(Action(2));
		}
	}

	public IEnumerator Action(int useVehicle)
	{
		if (waitTime)
		{
			yield break;
		}
		waitTime = true;
		CarController vScript = vehicleScript.GetComponent<CarController>();
		//Get in car
		if (useVehicle == 1)
		{
			GetComponent<AudioSource>().clip = startEngine;
			GetComponent<AudioSource>().volume = 1f;
			GetComponent<AudioSource>().Play();
			controller.SetActive(false);
			vehicle.SetActive(true);
			hands.SetActive(true);
			//body.SetActive(true);
			inCar = true;
			controller.transform.eulerAngles = new Vector3(0f, vehicleScript.transform.rotation.eulerAngles.y, 0f);
			controller.transform.position = transform.position + (Vector3.right * 2f);
			controller.transform.parent = vehicle.transform;
			yield return new WaitForSeconds(0.4f);
			vehicleScript.GetComponent<CarController>().controlsEnabled = true;
			vehicleScript.GetComponent<Rigidbody>().isKinematic = false;
			waitTime = false;
		}
		//Get out
		if (useVehicle == 2)
		{
			if (vScript.vSpeed < 0.5f)
			{
				vehicleScript.GetComponent<Rigidbody>().isKinematic = true;
				GetComponent<AudioSource>().clip = handbrake;
				GetComponent<AudioSource>().volume = 1f;
				GetComponent<AudioSource>().Play();
				yield return new WaitForSeconds(0.7f);
			}
			vehicle.SetActive(false);
			hands.SetActive(false);
			//body.SetActive(false);
			inCar = false;
			vScript.controlsEnabled = false;
			controller.transform.parent = null;
			controller.transform.eulerAngles = new Vector3(0f, vehicleScript.transform.rotation.eulerAngles.y, 0f);
			controller.transform.position = getOutPosition.position;
			controller.SetActive(true);
			yield return new WaitForSeconds(0.3f);
			waitTime = false;
		}
		//Die inside
		if (useVehicle == 3)
		{
			vehicle.SetActive(false);
			hands.SetActive(false);
			//body.SetActive(false);
			inCar = false;
			vScript.controlsEnabled = false;
			controller.transform.parent = null;
			controller.transform.eulerAngles = new Vector3(0f, vehicleScript.transform.rotation.eulerAngles.y, 0f);
			controller.transform.position = seat.position;
			controller.SetActive(true);
			controller.SendMessage("ApplyDamage", 1000);
		}
	}
}
