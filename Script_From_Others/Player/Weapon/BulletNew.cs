using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitTypeBullet
{

	BODY,
	CONCRETE,
	WOOD,
	METAL,
	//OLD_METAL,
	//GLASS,
	SENTRYGUN
}

public class BulletNew : MonoBehaviour
{

	private float damage; // damage bullet applies to a target
	private float maxHits; // number of collisions before bullet gets destroyed
	private float bulletGravity;
	private float impactForce; // force applied to a rigid body object
	private float speed; // bullet speed
	public float destroyBulletAfter; // time till bullet is destroyed
	private Vector3 velocity; // bullet velocity
	private Vector3 newPos; // bullet's new position
	private Vector3 oldPos; // bullet's previous position
	private bool hasHit;
	private Vector3 direction;
	private float spread;
	private RaycastHit hit;
	[HideInInspector]
	public HitTypeBullet hitType;
	public GameObject bulletMarksPrefab;
	private int id;
	private int id2;
	public Transform rotate;
	public bool bulletRotation;
	public LayerMask layerMask;
	private bool penetrated;

	void Start()
	{
		rotate.gameObject.SetActive(false);
	}


	public IEnumerator SetUp(float[] info)
	{
		damage = info[0];
		impactForce = info[1];
		maxHits = info[2];
		bulletGravity = info[3];
		spread = info[4];
		speed = info[5];

		if (bulletGravity == 0)
		{
			bulletGravity = speed / 25;
		}

		var x = (1 - 2 * Random.value) * spread;
		var y = (1 - 2 * Random.value) * spread;

		direction = transform.TransformDirection(new Vector3(x, y, 1));

		newPos = transform.position;
		oldPos = newPos;
		velocity = speed * transform.forward;

		Destroy(gameObject, destroyBulletAfter);
		if (speed > 50)
		{
			yield return new WaitForSeconds(0.02f);
		}
		else
		{
			yield return new WaitForSeconds(0.1f);
		}
		rotate.gameObject.SetActive(true);
	}



	public void Update()
	{
		//if (hasHit) return; 
		if (Cursor.lockState != CursorLockMode.Locked)
		{
			Cursor.visible = true;
			return;
		}

		newPos += (velocity + direction) * Time.deltaTime;

		Vector3 dir = newPos - oldPos;
		float dist = dir.magnitude;
		dir /= dist;
		if (dist > 0)
		{

			if (Physics.Raycast(oldPos, dir, out hit, dist, layerMask))
			{

				newPos = hit.point;

				if (hit.collider.GetInstanceID() != id)
				{
					//	if (hit.collider.transform.root.tag != tagToIgnore) 
					OnHit(hit, dir);
				}
			}

			if (penetrated)
			{
				if (Physics.Raycast(newPos, -dir, out hit, dist, layerMask))
				{
					if (hit.collider.GetInstanceID() != id2) OnBackHit(hit);

				}
			}
		}

		if (maxHits <= 0)
		{
			Destroy(gameObject);
		}

		if (hasHit)
		{
			Destroy(gameObject);
		}

		oldPos = transform.position;
		transform.position = newPos;
		velocity.y -= bulletGravity * Time.deltaTime;
		if (bulletRotation)
		{
			if (rotate) rotate.transform.rotation = Quaternion.LookRotation(dir);
		}
	}

	public void OnHit(RaycastHit frontHit, Vector3 hitDir)
	{

		PenetrationNew penetrate = frontHit.transform.GetComponent<PenetrationNew>();
		if (penetrate == null)
		{
			hasHit = true;
		}
		else
		{
			if (maxHits < penetrate.count)
			{
				hasHit = true;
			}
			else
			{
				maxHits -= penetrate.count;
				penetrated = true;
			}
		}


		switch (frontHit.transform.tag)
		{

			case "Untagged":
				hitType = HitTypeBullet.CONCRETE;
				break;
			case "Enemy":
				hitType = HitTypeBullet.BODY;
				break;
			case "SentryGun":
				hitType = HitTypeBullet.SENTRYGUN;
				break;
			case "Player":
				hitType = HitTypeBullet.BODY;
				break;
			case "Wood":
				hitType = HitTypeBullet.WOOD;
				break;
			case "Concrete":
				hitType = HitTypeBullet.CONCRETE;
				break;
			case "Metal":
				hitType = HitTypeBullet.METAL;
				break;
			/*	
			case "glass":
				hitType = HitTypeBullet.GLASS;
				break;
			case "oldMetal":
				hitType = HitTypeBullet.OLD_METAL;
				break;
			case "Ground":
				hitType = HitTypeBullet.CONCRETE;
				break;
			case "water":
				break;
			*/
			default:
				hitType = HitTypeBullet.CONCRETE;
				break;
		}


		Quaternion rotation = Quaternion.FromToRotation(Vector3.up, frontHit.normal);
		GameObject gos = Instantiate(bulletMarksPrefab, frontHit.point, rotation);
		gos.transform.localPosition += .01f * frontHit.normal;
		gos.transform.parent = frontHit.transform;


		BulletMarksNew bm = gos.GetComponent<BulletMarksNew>();
		bm.BulletHitObject(hitType, 1);

		frontHit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
		id = frontHit.collider.GetInstanceID();

		if (frontHit.rigidbody)
		{
			frontHit.rigidbody.AddForceAtPosition(impactForce * hitDir + (Vector3.up * impactForce / 4), frontHit.point);
			hasHit = true;
		}

	}

	public void OnBackHit(RaycastHit backHit)
	{

		switch (backHit.transform.tag)
		{

			case "Untagged":
				hitType = HitTypeBullet.CONCRETE;
				break;
			case "Enemy":
				hitType = HitTypeBullet.BODY;
				break;
			case "Player":
				hitType = HitTypeBullet.BODY;
				break;
			case "Wood":
				hitType = HitTypeBullet.WOOD;
				break;
			case "Concrete":
				hitType = HitTypeBullet.CONCRETE;
				break;
			case "Metal":
				hitType = HitTypeBullet.METAL;
				break;
			/*	
            case "Glass":
                hitType = HitTypeBullet.GLASS;
                break;
            case "OldMetal":
                hitType = HitTypeBullet.OLD_METAL;
                break;
            case "Ground":
                hitType = HitTypeBullet.CONCRETE;
                break;
            case "Water":
                break;
			*/
			default:
				hitType = HitTypeBullet.CONCRETE;
				break;
		}

		GameObject go;
		var backRotation = Quaternion.FromToRotation(Vector3.up, backHit.normal);
		go = Instantiate(bulletMarksPrefab, backHit.point, backRotation);
		go.transform.localPosition += .01f * backHit.normal;
		go.transform.parent = backHit.transform;

		BulletMarksNew bm = go.GetComponent<BulletMarksNew>();
		bm.BulletHitObject(hitType, 3);
		id2 = backHit.collider.GetInstanceID();
		penetrated = false;
	}
}
