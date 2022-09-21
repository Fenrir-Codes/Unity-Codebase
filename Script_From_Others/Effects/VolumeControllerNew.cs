using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeControllerNew : MonoBehaviour {

	public float vol;
	public CharacterController controller;

	void Update (){
		if(Time.timeScale == 1){
			if(controller.velocity.y < -5 || controller.velocity.y > 5){
				vol = Mathf.Lerp(vol, 1.0f, Time.deltaTime * 2);
			}else{
				vol = Mathf.Lerp(vol, 0.0f, Time.deltaTime * 5);
			} 
			GetComponent<AudioSource>().volume = vol + .25f;
			GetComponent<AudioSource>().pitch = 1 + vol; 
		}else{
			GetComponent<AudioSource>().pitch = Time.timeScale;
		}	
	}
}
