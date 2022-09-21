using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketNew : MonoBehaviour {

	private float damage = 0; // damage bullet applies to a target			
	private float impactForce; // force applied to a rigid body object
	private float speed; // bullet speed
	private Vector3 velocity; // bullet velocity
	private Vector3 newPos; // bullet's new position
	private Vector3 oldPos; // bullet's previous position     	
	private Vector3 direction;
	private float spread;
	private RaycastHit hit;
	public float rocketGravity;
	public float destroyRocketAfter; // time till bullet is destroyed
	public Transform rocketProjectile;
	public LayerMask layerMask;
	public Transform explPrefab;
	private Transform myTransform;
	public ParticleSystem[] RocketEffects;
	[HideInInspector]
	public Transform target;
	private float rotationSpeed;
	public float maxRotationSpeed;

	public IEnumerator Start()
	{
		myTransform = transform;
		yield return new WaitForSeconds(0.5f);
		if (GameObject.Find("JetLock"))
		{
			target = GameObject.Find("JetLock").transform;
			GetComponent<AudioSource >().Play();
		}
	}

	public IEnumerator SetUp(float[] info)
	{
		damage = info[0];
		speed = info[1];

		newPos = transform.position;
		oldPos = newPos;

		
		Destroy(gameObject, destroyRocketAfter);

		//enable traser after few miliseconds
		if (rocketProjectile)
		{
			yield return new WaitForSeconds(0.02f);
			rocketProjectile.gameObject.SetActive(true);
		}
	}


	public void Update()
	{

		newPos += (transform.forward + velocity) * speed * Time.deltaTime;

		Vector3 dir = newPos - oldPos;
		float dist = dir.magnitude;
		dir /= dist;
		if (dist > 0)
		{

			if (Physics.Raycast(oldPos, dir, out hit, dist, layerMask))
			{
				newPos = hit.point;

				OnHit(hit, dir);
			}
		}

		oldPos = transform.position;
		transform.position = newPos;


		if (target)
		{
			if (Vector3.Distance(target.position, myTransform.position) < 80)
			{
				target.SendMessageUpwards("ChangeTarget", SendMessageOptions.DontRequireReceiver);
				if (GameObject.Find("AntiRocket"))
				{
					target = GameObject.Find("AntiRocket").transform;
				}
				//return;
			}
			if (rotationSpeed < maxRotationSpeed)
			{
				rotationSpeed += Time.deltaTime * 5;
			}

			myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), rotationSpeed * Time.deltaTime);
			if (Vector3.Distance(target.position, myTransform.position) < 4)
			{
				Instantiate(explPrefab, transform.position, transform.rotation);
				DestryRocket();
			}
		}
		else
		{
			if (rocketProjectile) rocketProjectile.transform.rotation = Quaternion.LookRotation(dir);
			velocity.y -= rocketGravity * Time.deltaTime;
		}
	}

	public void OnHit(RaycastHit rHit, Vector3 hitDir)
	{
		var contact = rHit.point;
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, rHit.normal);

		Instantiate(explPrefab, contact, rotation);
		rHit.collider.SendMessageUpwards("ApplyDamage", 5000, SendMessageOptions.DontRequireReceiver);
		DestryRocket();
		for(int i = 0; i < RocketEffects.Length; i++)
		{
			//RocketEffects[i].loop = false;
			RocketEffects[i].Stop();
		}
	}

	public void DestryRocket()
	{
		//Disable emmiter (to stop emit smoke trail particles)
		var emiter = GetComponentsInChildren(typeof(ParticleSystem));
		foreach (ParticleSystem emi in emiter)
		{
			emi.Play(false);
		}
		//Detach smoke trail from rocket and destry rocket
		transform.DetachChildren();
		Destroy(gameObject);
	}
}
