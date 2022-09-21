using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSkullsNew : MonoBehaviour {

	public AudioClip pickUpSound;
	private HUDManager HUD;

	void Start()
	{
		HUD = FindObjectOfType<HUDManager>();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			HUD.addPoints();
			//BroadcastMessage("addPoints");
			PlayAudioClip(pickUpSound, transform.position, 1f);
			Destroy(gameObject);
		}
	}

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
	{
		GameObject go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = Random.Range(0.95f, 1.05f);
		source.Play();
		Destroy(go, clip.length);
		return source;
	}
}
