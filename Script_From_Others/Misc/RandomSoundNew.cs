using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundNew : MonoBehaviour {
	public AudioClip[] bulletSounds;
	public float audioRicochetteLength = 0.2f;

	public void Start ()
	{
		StartCoroutine(PlaySounds());
	}

	public IEnumerator PlaySounds () {
		GetComponent<AudioSource>().clip = bulletSounds[Random.Range(0, bulletSounds.Length)];
		GetComponent<AudioSource>().Play();
		yield return new WaitForSeconds(10);
		//yield WaitForSeconds(audio.clip.length);
		Destroy(gameObject);
	}
}
