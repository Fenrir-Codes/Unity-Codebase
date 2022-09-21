using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowMotionNew : MonoBehaviour {

	public AudioClip slowMoIn;
	public AudioClip slowMoOut;

	public void Update()
	{

		if (Input.GetKeyDown(KeyCode.Mouse2))
		{

			if (Time.timeScale == 1.0f)
			{
				Time.timeScale = 0.3f;
			}
			else
			{
				Time.timeScale = 1.0f;
				Time.fixedDeltaTime = 0.02f * Time.timeScale;
			}

			var aSources = FindObjectsOfType(typeof(AudioSource));
			foreach (AudioSource aSource in aSources)
				aSource.pitch = Time.timeScale;

			if (Time.timeScale == 1.0) PlayAudioClip(slowMoOut, transform.position, 1.0f);
			else PlayAudioClip(slowMoIn, transform.position, 1.0f);
		}
	}

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
	{
		var go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = 1.0f;
		source.Play();
		Destroy(go, clip.length);
		return source;
	}
}
