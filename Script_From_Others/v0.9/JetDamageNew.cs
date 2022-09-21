using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JetDamageNew : MonoBehaviour {

	public float hitPoints;
	public Rigidbody deadReplacement;
	public GameObject explosion;
	public Transform pos;
	private ManagerScore scoreManager;
	public int rewardPoints;
	public string ScoreGOName = "ScoreManager";

	public void Start()
	{
		GameObject sc = GameObject.Find(ScoreGOName);
		scoreManager = sc.GetComponent<ManagerScore>();
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

	public void Replace(int temp)
	{
		if (temp == 0)
		{
			scoreManager.addScore(rewardPoints);
		}
		if (deadReplacement)
		{
			int random = Random.Range(0, 4);
			Rigidbody dead = Instantiate(deadReplacement, pos.position, pos.rotation);
			dead.AddForce(transform.forward * 6000, ForceMode.Impulse);
			Instantiate(explosion, pos.position, pos.rotation);
			Destroy(gameObject);
		}
	}
}
