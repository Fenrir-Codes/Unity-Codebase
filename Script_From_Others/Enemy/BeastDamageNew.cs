using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastDamageNew : MonoBehaviour {

	public float hitPoints;
	public Rigidbody deadReplacement;
	public GameObject skull;
	public Transform pos;
	private ManagerScore scoreManager;
	public int rewardPoints;
	private SpawnerWithVawesNew spawner;
	public AudioClip[] quakeSound;

	public void Start()
	{
		GameObject sc = GameObject.Find("ScoreManager");
		scoreManager = sc.GetComponent<ManagerScore>();
		spawner = sc.GetComponent<SpawnerWithVawesNew>();
	}

	public void ApplyDamage(float damage)
	{
		if (hitPoints <= 0f)
		{
			return;
		}
		// Apply damage
		hitPoints = hitPoints - damage;
		scoreManager.DrawCrosshair();
		// Are we dead?
		if (hitPoints <= 0f)
		{
			Replace(0);
		}
	}

	public void ApplyKnifeDamage(float damage)
	{
		if (hitPoints <= 0f)
		{
			return;
		}
		hitPoints = 0f;
		if (hitPoints <= 0f)
		{
			int random = Random.Range(0, quakeSound.Length);
			PlayAudioClip(quakeSound[random], transform.position, 0.7f);
			scoreManager.addScore(1000);
			Replace(1);
		}
	}

	public void Replace(int temp)
	{
		if (temp == 0)
		{
			scoreManager.addScore(rewardPoints);
		}
		if (deadReplacement)
		{
			spawner.RecountEnemiesInGame();
			int random = Random.Range(0, 4);
			Rigidbody dead = Instantiate(deadReplacement, pos.position, pos.rotation);
			if (random == 1)
			{
				Instantiate(skull, pos.position, pos.rotation);
			}
			Destroy(gameObject);
		}
	}

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
	{
		GameObject go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = (AudioSource)go.AddComponent(typeof(AudioSource));
		source.clip = clip;
		source.volume = volume;
		source.pitch = Time.timeScale;
		source.Play();
		Destroy(go, clip.length);
		return source;
	}
}
