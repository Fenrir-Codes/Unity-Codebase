using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutNew : MonoBehaviour {

	public float fadeTime = 5f;
	//public Texture fadeTexture : Texture;
	private bool fadeOut = false;
	private float tempTime;
	private float time = 0.0f;
	public AudioClip fadeOutSound;
	private HUDManager HUD;

	void Start(){
		if (HUD == null && FindObjectOfType<HUDManager>() != null)
			HUD = FindObjectOfType<HUDManager>();

		fadeOut = true;
		GetComponent<AudioSource>().clip = fadeOutSound;
		GetComponent<AudioSource>().Play();
	}


	void Update(){
		if (fadeOut){
			if(time < fadeTime) time += Time.deltaTime;
			tempTime = Mathf.InverseLerp(0.0f, fadeTime, time);
			HUD.FadeTexture.color = new Color(1f, 1f, 1f, 1 - tempTime);
		}
	
		if(tempTime >= 1.0) enabled = false;
	}

	/*public void OnGUI(){
		if(fadeOut){
			GUI.color.a = 1 - tempTime;
			GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), fadeTexture);
		}
	}*/
}
