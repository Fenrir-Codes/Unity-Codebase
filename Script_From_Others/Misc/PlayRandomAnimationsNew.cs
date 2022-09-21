using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayRandomAnimationsNew : MonoBehaviour {
	public AnimationClip[] anim;
	public GameObject shellObj;
	public AudioClip shellHit;
	//var smokeEffect : GameObject;

	public IEnumerator Start () {
		//Instantiate (smokeEffect, shellObj.transform.position, shellObj.transform.rotation);
		string random = anim[Random.Range(0, anim.Length)].name;
		GetComponent<Animation>().Play(random);
		//Destroy (shellObj, 0.4);
		yield return new WaitForSeconds(0.4f);
		PlayAudioClip(shellHit, transform.position + Vector3.right * 5, .2f);
		Destroy (shellObj);
	}

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
	{
		var go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = Time.timeScale;
		source.Play();
		Destroy(go, clip.length);
		return source;
	}
}
