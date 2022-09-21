using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationBreath : MonoBehaviour {

	private void LateUpdate()
	{
		if (!Input.GetKey("mouse 1"))
			GetComponent<Animation>().CrossFade("Breath2");
		else
			GetComponent<Animation>().CrossFade("IdleAnimation");
		//transform.localPosition = Vector3(0, 0, 0);
	}
}
