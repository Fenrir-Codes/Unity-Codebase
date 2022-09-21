using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleporterNew : MonoBehaviour {

	public Transform target;
	public AudioClip teleportSound;
	public Vector3 rotation = Vector3.forward;

	void OnTriggerEnter(Collider other)
	{
		other.transform.position = target.position;
		GetComponent<AudioSource>().PlayOneShot(teleportSound, 0.7f);
	} 
}
