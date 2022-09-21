using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckTargetNew : MonoBehaviour {

	public float timeToSetTarget;
	public float timer;
	public float distance;
	public Transform target;
	public RaycastHit hit;
	public LayerMask layerMask;
	public AudioClip hitTargetSound;
	public bool playing;
	public ScriptWeapon wepScript;

	void Update()
	{

		Vector3 direction = transform.TransformDirection(Vector3.forward);
		//Debug.DrawRay (transform.position, direction * distance, Color.green);  
		if (Physics.Raycast(transform.position, direction, out hit, distance, layerMask))
		{
			if (wepScript.gunActive && wepScript.bulletsLeft == 1)
			{
				if (hit.collider.name == "JetAI" && target == null)
				{
					target = hit.transform;
				}
				if (target)
				{
					if (timer > 0.0f)
					{

						timer -= Time.deltaTime;
						target.name = "Jet";

						if (timer <= 0.0f)
						{
							target.name = "JetLock";
						}
					}
				}
			}
		}
		else
		{
			if (timer < timeToSetTarget)
				timer += Time.deltaTime;

			if (target)
			{
				if (timer >= timeToSetTarget)
				{
					target.name = "JetAI";
					target = null;
				}
			}
		}

		if (timer < 1.5f && timer > -0.1f)
		{
			PlaySound();
		}
	}

	public IEnumerator PlaySound()
	{
		if (playing) yield break;

		float waitTime = timer / 5;
		if (waitTime < 0.07f) waitTime = 0.07f;

		playing = true;
		yield return new WaitForSeconds(waitTime);
		GetComponent<AudioSource>().clip = hitTargetSound;
		GetComponent<AudioSource>().Play();
		playing = false;
	}
}
