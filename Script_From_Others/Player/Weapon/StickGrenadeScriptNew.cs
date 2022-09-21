using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickGrenadeScriptNew : MonoBehaviour {
	public LayerMask layerMask;
	public AudioClip soundHit;
	private Vector3 velocity;
	private Vector3 newPos;
	private Vector3 oldPos;
	private bool hasHit;
	private Vector3 direction;
	private RaycastHit hit;
	public float speed;
	public float forceToApply;
	public float grenadeGravity;
	private GameObject follow;
	public Transform explosion;
	public float timer;
	public float timer2;
	public AudioClip soundBeep;
	public GameObject lightPos;
	private GameObject @object;
	public float explodeAfter;

	public void Start()
	{
		timer2 = timer;
		newPos = transform.position;
		oldPos = newPos;
		velocity = speed * transform.forward;
	}

	public void Update()
	{
		if (hasHit)
		{
			if (!follow)
			{
				Destroy(gameObject);
			}
			transform.position = follow.transform.position;
			transform.rotation = follow.transform.rotation;
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
				if (hit.collider)
				{
					if (GetComponent<AudioSource>())
					{
						GetComponent<AudioSource>().PlayOneShot(soundHit, 0.3f);
					}
					GameObject hitPoint = Instantiate(new GameObject(), hit.point, transform.root.rotation);
					hitPoint.transform.parent = hit.transform;
					hitPoint.transform.localPosition = hitPoint.transform.localPosition + (0.007f * hit.normal);
					follow = hitPoint;
					GetComponent<Collider>().isTrigger = true;
					hasHit = true;
					StartCoroutine(Activate(hitPoint.transform.gameObject));
				}
			}
		}
		oldPos = transform.position;
		transform.position = newPos;
		velocity.y = velocity.y - (grenadeGravity * Time.deltaTime);
	}

	public IEnumerator Activate(GameObject connectedObject)
	{
		StartCoroutine(DestroyNow());
		@object = connectedObject;
		while (true)
		{
			GetComponent<Collider>().isTrigger = true;
			yield return new WaitForSeconds(timer);
			timer = timer - (timer2 / 10);
			GetComponent<AudioSource>().PlayOneShot(soundBeep, 0.5f);
			lightPos.GetComponent<Light>().enabled = true;
			yield return new WaitForSeconds(0.1f);
			lightPos.GetComponent<Light>().enabled = false;
			if (@object == null)
			{
				Destroy(gameObject);
			}
		}
	}

	public IEnumerator ApplyDamage()
	{
		yield return new WaitForSeconds(0.1f);
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject);
	}

	public IEnumerator DestroyNow()
	{
		yield return new WaitForSeconds(explodeAfter);
		Instantiate(explosion, transform.position, transform.rotation);
		Destroy(gameObject);
	}
}
