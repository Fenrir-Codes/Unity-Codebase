using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

public class ArrowNew : MonoBehaviour {

	private float damage; // damage bullet applies to a target			
	private float impactForce; // force applied to a rigid body object
	private float speed; // bullet speed
	private Vector3 velocity; // bullet velocity
	private Vector3 newPos; // bullet's new position
	private Vector3 oldPos; // bullet's previous position     	
	private Vector3 direction;
	private float spread;
	private RaycastHit hit;
	private GameObject follow;
	private bool hasHit;
	public float arrowGravity;
	public float destroyArrowAfter; // time till bullet is destroyed
	public Transform arrowModel;
	public LayerMask layerMask;
	//Particle Effects
	public GameObject Concrete;
	public GameObject Wood;
	public GameObject Metal;
	public GameObject Blood;
	public bool exploadingArrow;
	public Transform explPrefab;

	public IEnumerator SetUp(float[] info)
	{
		damage = info[0];
		impactForce = info[1];
		spread = info[2];
		speed = info[3];
		float x = (1 - (2 * Random.value)) * spread;
		float y = (1 - (2 * Random.value)) * spread;
		direction = transform.TransformDirection(new Vector3(x, y, 1));
		newPos = transform.position;
		oldPos = newPos;
		velocity = speed * transform.forward;
		Destroy(gameObject, destroyArrowAfter);
		//enable traser after few miliseconds
		if (arrowModel)
		{
			yield return new WaitForSeconds(0.02f);
			arrowModel.gameObject.SetActive(true);
		}
	}

	public void Update()
	{
		if (hasHit)
		{
			if (follow)
			{
				transform.position = follow.transform.position;
				transform.rotation = follow.transform.rotation;
			}
			else
			{
				Destroy(gameObject);
			}
			return;
		}
		newPos = newPos + ((velocity + direction) * Time.deltaTime);
		Vector3 dir = newPos - oldPos;
		float dist = dir.magnitude;
		dir = dir / dist;
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
		velocity.y = velocity.y - (arrowGravity * Time.deltaTime);
		if (arrowModel)
		{
			arrowModel.transform.rotation = Quaternion.LookRotation(dir);
		}
	}

	public void OnHit(RaycastHit rHit, Vector3 hitDir)
	{
		Vector3 contact = rHit.point;
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, rHit.normal);
		if (exploadingArrow)
		{
			rHit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
			Instantiate(explPrefab, rHit.point, rotation);
			Destroy(gameObject);
		}
		else
		{
			if ((rHit.transform.tag == "Untagged") || (rHit.transform.tag == "Concrete"))
			{
				GameObject bulletHole = Instantiate(Concrete, contact, rotation);
				bulletHole.transform.parent = rHit.transform;
			}
			else
			{
				//ArrowAddRigidBody();
				if (rHit.transform.tag == "Enemy")
				{
					GameObject bloodHole = Instantiate(Blood, contact, rotation);
					bloodHole.transform.parent = rHit.transform;
				}
				else
				{
					if (rHit.transform.tag == "Wood")
					{
						GameObject woodHole = Instantiate(Wood, contact, rotation);
						woodHole.transform.parent = rHit.transform;
					}
					else
					{
						if (rHit.transform.tag == "Metal")
						{
							GameObject metalHole = Instantiate(Metal, contact, rotation);
							metalHole.transform.parent = rHit.transform;
						}
					}
				}
			}
			//ArrowAddRigidBody();
			rHit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
			if (rHit.rigidbody)
			{
				GameObject hitPoint = Instantiate(new GameObject(), rHit.point, transform.root.rotation);
				hitPoint.transform.parent = rHit.transform;
				GetComponent<Collider>().isTrigger = true;
				follow = hitPoint;
				hasHit = true;
				rHit.rigidbody.AddForceAtPosition((impactForce * hitDir) + ((Vector3.up * impactForce) / 4), contact);
			}
			else
			{
				enabled = false;
			}
		}
	}

	public void ArrowAddRigidBody()
	{
		gameObject.AddComponent<Rigidbody>();
		GetComponent<Rigidbody>().drag = 1;
		GetComponent<Rigidbody>().AddRelativeForce(0, 0, -200);
		enabled = false;
	}
}
