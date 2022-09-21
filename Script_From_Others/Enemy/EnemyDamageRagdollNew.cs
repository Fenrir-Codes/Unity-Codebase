using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageRagdollNew : MonoBehaviour {

	public float hitPoints;
	public int pointsToAdd;
	public int headShotPoints;
	public ManagerScore scoreManager;
	public bool headshot;
	public AudioClip dieSound;

	void Start()
	{
		GameObject GO = GameObject.FindWithTag("ScoreManager");
		scoreManager = GO.GetComponent<ManagerScore>();
	}

	public void Headshot(float headDamage)
	{
		if (hitPoints <= 0f)
		{
			return;
		}
		scoreManager.DrawCrosshair();
		headshot = true;
		hitPoints = hitPoints - headDamage;
		GetComponent<Animation>().enabled = false;
		scoreManager.addScore(headShotPoints);
		if (hitPoints <= 0f)
		{
			StartCoroutine(GetUP());
		}
	}

	public IEnumerator ApplyDamage(float damage)
	{
		if ((hitPoints > 0) && !headshot)
		{
			scoreManager.DrawCrosshair();
			if (hitPoints <= 0f)
			{
				yield break;
			}
			hitPoints = hitPoints - damage;
			GetComponent<Animation>().enabled = false;
			if (dieSound)
			{
				AudioSource.PlayClipAtPoint(dieSound, transform.position);
			}
			yield return new WaitForSeconds(0.5f);
			scoreManager.addScore(pointsToAdd);
			if (hitPoints <= 0f)
			{
				StartCoroutine(GetUP());
			}
		}
	}

	public IEnumerator GetUP()
	{
		if (hitPoints < 0f)
		{
			yield return new WaitForSeconds(5f);
			hitPoints = 10;
			GetComponent<Animation>().enabled = true;
			headshot = false;
		}
	}
}
