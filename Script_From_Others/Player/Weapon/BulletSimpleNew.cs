using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSimpleNew : MonoBehaviour {

	private float damage; // damage bullet applies to a target			
	private float impactForce; // force applied to a rigid body object
	private float speed; // bullet speed
	private Vector3 velocity; // bullet velocity
	private Vector3 newPos; // bullet's new position
	private Vector3 oldPos; // bullet's previous position     	
	private Vector3 direction;
	private float spread;
	private RaycastHit hit;
	public float bulletGravity;
	public float destroyBulletAfter; // time till bullet is destroyed
	public Transform tracer;
	public LayerMask layerMask;
	//Particle Effects
	public GameObject Concrete;
	public GameObject Wood;
	public GameObject Metal;
	public GameObject Blood;
	public GameObject Glass;

	public IEnumerator SetUp(float[] info)
	{
		damage = info[0];
		impactForce = info[1];
		spread = info[2];
		speed = info[3];

		var x = (1 - 2 * Random.value) * spread;
		var y = (1 - 2 * Random.value) * spread;

		direction = transform.TransformDirection(new Vector3(x, y, 1));

		newPos = transform.position;
		oldPos = newPos;
		velocity = speed * transform.forward;

		Destroy(gameObject, destroyBulletAfter);

		//enable traser after few miliseconds
		if (tracer)
		{
			yield return new WaitForSeconds(0.02f);
			tracer.gameObject.SetActive(true);
		}
	}


	public void Update()
	{

		newPos += (velocity + direction) * Time.deltaTime;

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
		velocity.y -= bulletGravity * Time.deltaTime;

		if (tracer) tracer.transform.rotation = Quaternion.LookRotation(dir);
	}

	public void OnHit(RaycastHit rHit, Vector3 hitDir)
	{
		var contact = rHit.point;
		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, rHit.normal);

		if (rHit.transform.tag == "Untagged" || rHit.transform.tag == "Concrete")
		{
			GameObject bulletHole = Instantiate(Concrete, contact, rotation);
			bulletHole.transform.parent = rHit.transform;

		}
		else if (rHit.transform.tag == "Enemy")
		{
			GameObject bloodHole = Instantiate(Blood, contact, rotation);
			bloodHole.transform.parent = rHit.transform;
		}
		else if (rHit.transform.tag == "Wood")
		{
			GameObject woodHole = Instantiate(Wood, contact, rotation);
			woodHole.transform.parent = rHit.transform;

		}
		else if (rHit.transform.tag == "Metal")
		{
			GameObject metalHole = Instantiate(Metal, contact, rotation);
			metalHole.transform.parent = rHit.transform;

		}
		else if (rHit.transform.tag == "Glass")
		{
			GameObject glassHole = Instantiate(Glass, contact, rotation);
			glassHole.transform.parent = rHit.transform;
		}

		rHit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);

		if (rHit.rigidbody) rHit.rigidbody.AddForceAtPosition(impactForce * hitDir + (Vector3.up * impactForce / 4), contact);

		Destroy(gameObject);
	}
}
