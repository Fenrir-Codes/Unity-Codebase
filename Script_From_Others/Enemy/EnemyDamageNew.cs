using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageNew : MonoBehaviour {

	//#pragma strict
	public float hitPoints;
	public int pointsToAdd;
	public int headShotPoints;
	public int headDamageMultiplier;
	public ManagerScore scoreManager;
	public bool headshot;
	public AudioClip dieSound;
	public AudioClip breathSound;
	//var projector : GameObject;
	public float destryAfter;
	public GameObject destroyEffect;
	public GameObject ragdollCenter;
	public Vector3 previous;
	public Vector3 vel;
	public AudioClip hitPain;

	void Awake()
	{
		GameObject score = GameObject.FindWithTag("ScoreManager");
		scoreManager = score.GetComponent<ManagerScore>();
		//GetComponent<AudioSource>().clip = breathSound;
		//GetComponent<AudioSource>().loop = true;
		//GetComponent<AudioSource>().Play();
		var gosr = gameObject.GetComponentsInChildren(typeof(Rigidbody));
		foreach (Rigidbody gor in gosr)
		{
			gor.isKinematic = true;
			gor.useGravity = false;
		}
		var gos = gameObject.GetComponentsInChildren(typeof(Collider));
		foreach (Collider go in gos)
		{
			if (go.name != "HumanRagdoll")
			{
				go.isTrigger = true;
			}
		}
	}

	void Update()
	{
		if (hitPoints > 0f)
		{
			vel = (transform.position - previous) / Time.deltaTime;
			previous = transform.position;
		}
	}

	//Only for Test
	public void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;
		if (body)
		{
			ApplyDamage(hitPoints + 1);
			GetComponent<AudioSource>().clip = hitPain;
			GetComponent<AudioSource>().Play();
		}
	}

	public void ApplyDamage(float damage)
	{
		if (hitPoints <= 0f)
		{
			return;
		}
		if (headshot)
		{
			hitPoints = hitPoints - (damage * headDamageMultiplier);
		}
		else
		{
			hitPoints = hitPoints - damage;
		}
		scoreManager.DrawCrosshair();
		if (hitPoints <= 0f)
		{
			KillZombie();
		}
		headshot = false;
	}

	public void KillZombie()
	{
		//if(headshot) scoreManager.addScore(headShotPoints);
		//else scoreManager.addScore(pointsToAdd);
		Destroy(GetComponent<AIFollow>());
		Destroy(GetComponent<Seeker>());
		CharacterController myController = GetComponent<CharacterController>();
		myController.enabled = false;
		GetComponent<Animation>().enabled = false;
		var gof = gameObject.GetComponentsInChildren(typeof(Collider));
		foreach (Collider goh in gof)
		{
			if (goh.name != "HumanRagdoll")
			{
				goh.isTrigger = false;
			}
		}
		var gos = gameObject.GetComponentsInChildren(typeof(Rigidbody));
		foreach (Rigidbody go in gos)
		{
			go.isKinematic = false;
			go.useGravity = true;
			go.GetComponent<Rigidbody>().velocity = vel;
		}
		var scripts = gameObject.GetComponentsInChildren(typeof(MonoBehaviour));
		foreach (MonoBehaviour script in scripts)
		{
			script.enabled = false;
		}
		GetComponent<AudioSource>().loop = false;
		//GetComponent<AudioSource>().Stop();
		StartCoroutine(DestroyRagdoll());
	}
	
	public IEnumerator DestroyRagdoll()
	{
		yield return new WaitForSeconds(destryAfter);
		Instantiate(destroyEffect, ragdollCenter.transform.position, ragdollCenter.transform.rotation);
		yield return new WaitForSeconds(0.05f);
		Destroy(gameObject);
	}
}
