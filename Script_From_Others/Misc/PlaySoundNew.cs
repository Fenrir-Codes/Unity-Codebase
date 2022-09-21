using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundNew : MonoBehaviour {

	public AudioClip playSound;

	public void Start ()
	{
		GetComponent<AudioSource>().clip = playSound;
		GetComponent<AudioSource>().Play();
	}
}
