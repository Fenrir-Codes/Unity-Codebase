using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryGunAILogicsNew : MonoBehaviour {
	//#pragma strict
	public Transform[] acc;
	public GameObject target;
	public float targetHealth;
	private Transform player;
	public GameObject defaultTarget;
	public GameObject destryedTarget;
	public string[] targetNames;
	public float maxAngle;
	public float rotationSpeed;
	public Transform muzle;
	public float aimSpeed;
	public GameObject searchTargetRotator;
	public GameObject searchTarget;
	//BULLET VARS
	public GameObject projectile;
	public float fireRate;
	private float nextFire;
	public float[] bulletInfo;
	public float damage;
	public float impactForce;
	public float maxPenetration;
	public float spread;
	public float bulletSpeed;
	public float bulletGravity;
	public GameObject barrel;
	public float rotSpeed;
	public GameObject minigunFiring;
	public AudioClip fireSound;
	public AudioClip rotatorStops;
	public AudioClip lookRotation;
	public ParticleSystem[] muzleFlash;
	public GameObject smokePrefab;
	public Transform laserGO;
	public Transform laserHitPoint;
	public bool checking;
	public int count;
	public Transform rayCheckGO;
	public LayerMask layerMask;
	public int state;
	/**
 0 = Searching for Target
 1 = Shooting at Target
 2 = SG Destryed
 3 = Activate SG
**/
	public IEnumerator Start()
	{
		//Activate SG
		state = 3;
		yield return new WaitForSeconds(5.5f);
		if (state != 2)
		{
			state = 0;
		}
	}

	public void Update()
	{
		RaycastHit hit = default(RaycastHit);
		if (state == 2)//Sentry gun destroyed
		{
			Laser(2);
			target = destryedTarget;
			laserGO.gameObject.SetActive(false);
			muzle.gameObject.SetActive(false);
			muzleFlash[1].Stop();
			muzleFlash[0].Stop();
			GetComponent<AudioSource>().Stop();
			GetComponent<Animation>().Stop();
			Quaternion rotateToDestroyed = Quaternion.LookRotation((target.transform.position + new Vector3(0, 0.5f, 0)) - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotateToDestroyed, Time.deltaTime * aimSpeed);
			return;
		}
		if (state <= 1)
		{
			// rotate defaultTarget	
			float angles = maxAngle * Mathf.Sin(rotationSpeed * Time.time);
			searchTargetRotator.transform.localEulerAngles = Vector3.up * angles;
		}
		if (state == 0) //Searching for target
		{
			Laser(1);
			if (!GetComponent<AudioSource>().isPlaying)
			{
				GetComponent<AudioSource>().volume = 0.1f;
				GetComponent<AudioSource>().loop = true;
				GetComponent<AudioSource>().clip = lookRotation;
				GetComponent<AudioSource>().Play();
			}
			target = defaultTarget;
			Quaternion rotateToSearch = Quaternion.LookRotation(searchTarget.transform.position - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotateToSearch, Time.deltaTime);
			if (Physics.Raycast(muzle.position + new Vector3(0, -1, 0), muzle.forward, out hit))
			{
				int i = 0;
				while (i < targetNames.Length)
				{
					if (hit.transform.root.name == targetNames[i])
					{
						target = hit.transform.root.gameObject;
						state = 1;
					}
					i++;
				}
			}
		}
		if (state == 1)
		{
			Laser(2);
			//Limit rotation of turret (if target is in range)
			if ((transform.localEulerAngles.y < maxAngle) || (transform.localEulerAngles.y > (360 - maxAngle)))
			{
				if (target)
				{
					Transform aimPoint = target.transform.Find("TargetPoint");
					Quaternion rotate = Quaternion.LookRotation(aimPoint.position - transform.position);
					transform.rotation = Quaternion.Slerp(transform.rotation, rotate, Time.deltaTime * aimSpeed);
				}
				else
				{
					SentryGunStop();
				}
			}
			else
			{
				SentryGunStop();
			}
		}
		//barrelRoatation
		if (state == 1)
		{
			PrepareToShoot();
			if (rotSpeed < 50)
			{
				rotSpeed = rotSpeed + (Time.deltaTime * 50);
			}
			barrel.transform.Rotate(Vector3.forward * rotSpeed, Space.Self);
		}
		else
		{
			if (state == 0)
			{
				if (rotSpeed >= 0)
				{
					rotSpeed = rotSpeed - (Time.deltaTime * 20);
				}
				barrel.transform.Rotate(Vector3.forward * rotSpeed, Space.Self);
			}
		}
	}

	public void PrepareToShoot()
	{
		if (rotSpeed > 30)
		{
			if (!minigunFiring.GetComponent<AudioSource>().isPlaying)
			{
				minigunFiring.GetComponent<AudioSource>().loop = true;
				minigunFiring.GetComponent<AudioSource>().clip = fireSound;
				minigunFiring.GetComponent<AudioSource>().Play();
			}
			Shoot();
			StartCoroutine(CheckIfCanSee());
		}
	}

	public void Shoot()
	{
		if (state == 2)
		{
			return;
		}
		if (Time.time > nextFire)
		{
			nextFire = Time.time + fireRate;
			bulletInfo[0] = damage;
			bulletInfo[1] = impactForce;
			bulletInfo[2] = maxPenetration;
			bulletInfo[3] = bulletGravity;
			bulletInfo[4] = spread;
			bulletInfo[5] = bulletSpeed;
			GameObject clone = Instantiate(projectile, muzle.position, muzle.rotation);
			clone.SendMessageUpwards("SetUp", bulletInfo);
			muzleFlash[1].Emit(1);
			muzleFlash[0].Emit(1);
			Instantiate(smokePrefab, muzle.position, muzle.rotation);
		}
		if (target != null)
		{
			if (target.name == "CODcontroller")
			{
				return;
			}
			BeastDamageNew enemyHealth = target.GetComponent<BeastDamageNew>();
			targetHealth = enemyHealth.hitPoints;
			if ((targetHealth <= 0) || (target == null))
			{
				SentryGunStop();
			}
		}
	}

	public IEnumerator CheckIfCanSee()
	{
		RaycastHit hit = default(RaycastHit);
		if (checking)
		{
			yield break;
		}
		checking = true;
		if (Physics.Raycast(rayCheckGO.position, muzle.forward, out hit))
		{
			if (hit.transform.root.name == target.name)
			{
				count = 0;
			}
			else
			{
				count++;
				if (count > 10)
				{
					SentryGunStop();
				}
			}
		}
		yield return new WaitForSeconds(0.2f);
		checking = false;
	}

	public void SentryGunStop()
	{
		state = 0;
		count = 0;
		minigunFiring.GetComponent<AudioSource>().loop = false;
		minigunFiring.GetComponent<AudioSource>().Stop();
		minigunFiring.GetComponent<AudioSource>().PlayOneShot(rotatorStops);
		muzleFlash[1].Stop();
		muzleFlash[0].Stop();
	}

	public void Laser(int status)
	{
		RaycastHit hit = default(RaycastHit);
		LineRenderer lineRenderer = (LineRenderer)laserGO.GetComponent(typeof(LineRenderer));
		if (status == 1)
		{
			lineRenderer.enabled = true;
			lineRenderer.useWorldSpace = false;
			lineRenderer.positionCount = 2;
			lineRenderer.startWidth = 0.05f;
			lineRenderer.endWidth = 0.04f;
			//lineRenderer.SetVertexCount(2);
			//lineRenderer.SetWidth(0.05f, 0.04f);
			Physics.Raycast(laserGO.position, laserGO.forward, out hit);
			if (hit.collider)
			{
				lineRenderer.SetPosition(0, new Vector3(0, 0, 0));
				lineRenderer.SetPosition(1, new Vector3(0, 0, hit.distance - 0.03f));
			}
			else
			{
				lineRenderer.SetPosition(1, new Vector3(0, 0, 100));
			}
		}
		if (status == 2)
		{
			lineRenderer.enabled = false;
		}
	}
}
