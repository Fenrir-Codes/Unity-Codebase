using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayShellDropSoundNew : MonoBehaviour {

	public AudioClip[] shellDropSound;
	public bool once = false;

	public void Start()
	{
		Destroy(gameObject, 10);
	}

	public void OnCollisionEnter(Collision collision) {
	
		if(collision.collider.name == "Ground" && !once){
			PlayAudioClip(shellDropSound[Random.Range(0, shellDropSound.Length)], transform.position, 0.7f);
			once = true;
			enabled = false;
		
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
