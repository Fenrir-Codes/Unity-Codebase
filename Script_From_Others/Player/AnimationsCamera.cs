using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsCamera : MonoBehaviour {

	public string camRun = "CameraRun";
	public string camIdle = "IdleAnimation";

	public void CameraRunAnim (){
		GetComponent<Animation>()[camRun].speed = GetComponent<Animation>()[camRun].clip.length/0.5f;
		GetComponent<Animation>().CrossFade(camRun);
	}

	public void CameraIdleAnim (){
		GetComponent<Animation>().Stop(camRun);
		GetComponent<Animation>().CrossFade(camIdle);
	}
}
