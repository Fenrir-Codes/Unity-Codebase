using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScreenLocker;

public class WeaponOnDelay : MonoBehaviour {

	public float amount = 0.02f;
	public float maxAmount = 0.03f;
	public float smooth = 3f;
	private Vector3 def;

	public float tiltAngle = 4.0f;
	public float maxAngle = 10.0f;
	public bool on = true;

	void Start (){
		def = transform.localPosition;
	}

	public void Update ()
	{
		if (!ScreenLock.lockCursor) return;

		if (Input.GetButton("Fire2")){
			maxAmount = 0.02f;
			amount = maxAmount;	
		}else{
			maxAmount = 0.05f;
			amount = maxAmount;
		}

		float factorX = -Input.GetAxis("Mouse X") * amount;
		float factorY = -Input.GetAxis("Mouse Y") * amount;
	
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
	

	
		if(on){
			var TiltZ = Input.GetAxis("Mouse X") * tiltAngle;
		
			if(TiltZ > maxAngle) TiltZ = maxAngle;
			if(TiltZ < -maxAngle) TiltZ = -maxAngle;
		
			var Target = Quaternion.Euler (0, TiltZ/2, 0);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Target, Time.deltaTime * smooth/2);
		}else{
			var Back = Quaternion.Euler (0, 0, 0);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Back, Time.deltaTime * smooth/2);
		}
	
	}
}
