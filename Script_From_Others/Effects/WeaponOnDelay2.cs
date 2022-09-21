using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScreenLocker;

public class WeaponOnDelay2 : MonoBehaviour
{

	public float amount;
	public float maxAmount;
	public float smooth;
	private Vector3 def;
	public float tiltAngle;
	public bool on;

	public void Start()
	{
		def = transform.localPosition;
	}

	public void Update()
	{
		if (!ScreenLock.lockCursor) return;

		float factorX = -Input.GetAxis("Mouse X") * amount * Time.deltaTime;
		float factorY = -Input.GetAxis("Mouse Y") * amount * Time.deltaTime;

		if (factorX > maxAmount)
			factorX = maxAmount;

		if (factorX < -maxAmount)
			factorX = -maxAmount;

		if (factorY > maxAmount)
			factorY = maxAmount;

		if (factorY < -maxAmount)
			factorY = -maxAmount;


		Vector3 Final = new Vector3(def.x + factorX, def.y + factorY, def.z);
		transform.localPosition = Vector3.Lerp(transform.localPosition, Final, Time.deltaTime * smooth);

		if (on)
		{
			var TiltZ = Input.GetAxis("Mouse X") * tiltAngle;
			var Target = Quaternion.Euler(0, TiltZ / 2, 0);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Target, Time.deltaTime * smooth);
		}
		else
		{
			var Back = Quaternion.Euler(0, 0, 0);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Back, Time.deltaTime * smooth);
		}
	}
}