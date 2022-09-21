using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SentryGunHealthNew : MonoBehaviour {

	public float hitPoints;
	public ManagerScore scoreManager;
	public int pointsToAdd;
	public SentryGunAILogicsNew sentryGunAI;
	public GameObject destroyParticles;
	public float selfDestryAfter;

	public IEnumerator Start()
	{
		GameObject GO = GameObject.FindWithTag("ScoreManager");
		scoreManager = GO.GetComponent<ManagerScore>();
		yield return new WaitForSeconds(selfDestryAfter);
		if (hitPoints >= 0f)
		{
			SelfDestruction();
		}
	}

	public void ApplyDamage(float damage)
	{
		if (hitPoints <= 0f)
		{
			return;
		}
		hitPoints = hitPoints - damage;
		scoreManager.DrawCrosshair();
		if (hitPoints <= 0f)
		{
			Detonate();
		}
	}

	public void Detonate()
	{
		GameObject effect = Instantiate(destroyParticles, transform.position, transform.rotation);
		GetComponent<AudioSource>().Play();
		scoreManager.addScore(pointsToAdd);
		sentryGunAI.state = 2;
		Destroy(gameObject, 15);
	}

	public void SelfDestruction()
	{
		hitPoints = 0f;
		GameObject effect = Instantiate(destroyParticles, transform.position, transform.rotation);
		GetComponent<AudioSource>().Play();
		sentryGunAI.state = 2;
		Destroy(gameObject, 15);
	}
}
