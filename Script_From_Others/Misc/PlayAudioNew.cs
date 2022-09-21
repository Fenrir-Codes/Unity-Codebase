using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioNew : MonoBehaviour {

	public AudioClip[] bulletWhizSound;

	public void OnTriggerEnter (Collider other) {
	
		if(other.GetComponent<Collider>().tag != "Player"){
			PlayAudioClip(bulletWhizSound[Random.Range(0, bulletWhizSound.Length)], transform.position, 0.3f);
		}
	}

	public AudioSource PlayAudioClip (AudioClip clip, Vector3 position, float volume) {
		var go = new GameObject ("One shot audio");
		go.transform.position = position;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = Time.timeScale;
		source.Play ();
		Destroy (go, clip.length);
		return source;
	}
}
