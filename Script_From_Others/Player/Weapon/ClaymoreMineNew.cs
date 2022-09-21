using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaymoreMineNew : MonoBehaviour {
	public float hitPoints;
	public float explosionAfter;
	public Transform explosion;
	public AudioClip activate;
	public bool touched;
	public Transform pos;

	public void OnTriggerEnter(Collider other)
	{
		if (!touched)
		{
			if (other)
			{
				StartCoroutine(Explosion());
			}
		}
	}

	public IEnumerator Explosion()
	{
		touched = true;
		GetComponent<AudioSource>().clip = activate;
		GetComponent<AudioSource>().volume = 1.0f;
		GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(explosionAfter);
		Instantiate(explosion, pos.position, pos.rotation);
		yield return new WaitForSeconds(0.1f);
		Destroy(transform.root.gameObject);
	}
}
