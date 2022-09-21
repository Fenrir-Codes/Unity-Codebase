using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScreenLocker;

public class TiltWeapon2New : MonoBehaviour {

	public float smooth = 4.0f;
	public float tiltAngle = 5.0f;
	public bool on;

	public void Update()
	{
		if (!ScreenLock.lockCursor) return;

		if (on)
		{
			var TiltZ = -Input.GetAxis("Mouse X") * tiltAngle;
			var Target = Quaternion.Euler(0, TiltZ, TiltZ);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Target, Time.deltaTime * smooth);
		}
		else
		{
			var Back = Quaternion.Euler(0, 0, 0);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Back, Time.deltaTime * smooth);
		}

		if (on)
		{
			var TiltZ2 = Input.GetAxis("Mouse Y") * tiltAngle;
			var Target2 = Quaternion.Euler(TiltZ2, 0, 0);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Target2, Time.deltaTime * smooth);
		}
		else
		{
			var Back2 = Quaternion.Euler(0, 0, 0);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Back2, Time.deltaTime * smooth);
		}
	}

}
