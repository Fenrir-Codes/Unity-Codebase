using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiltWeaponSidewaysNew : MonoBehaviour
{

	public float smooth = 4.0f;
	public float tiltAngle = 5.0f;

	void Update()
	{
		if (!Input.GetButton("Fire2"))
		{
			var TiltZ = -Input.GetAxis("Horizontal") * tiltAngle;
			var Target = Quaternion.Euler(0, 0, TiltZ);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Target, Time.deltaTime * smooth);
		}
		else
		{
			var Back = Quaternion.Euler(0, 0, 0);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Back, Time.deltaTime * smooth);
		}
	}
}