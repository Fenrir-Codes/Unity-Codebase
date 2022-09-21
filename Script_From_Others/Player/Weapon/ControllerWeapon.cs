using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerWeapon : MonoBehaviour {

	public Vector3 moveTo;
	public Vector3 rotateTo;
	public float movementSpeed;

	public string sway = "AnimationName";
	public string idle = "AnimationName";

	public GameObject anim;
	public float animSpeed = 1.0f;

	public PlayerController codcontroller;
	public ScriptWeapon weaponScript;
	public bool runStatus;
	public bool callOnce;
	public bool withWeapon = true;

	void Start()
	{
		anim.GetComponent<Animation>().wrapMode = WrapMode.Loop;
	}

	public void Update()
	{
		runStatus = codcontroller.running;
		if (runStatus == true)
		{
			var runTarget = Quaternion.Euler(rotateTo);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, runTarget, Time.deltaTime * movementSpeed);
			transform.localPosition = Vector3.Lerp(transform.localPosition, moveTo, Time.deltaTime * movementSpeed);
			anim.GetComponent<Animation>()[sway].speed = animSpeed;
			anim.GetComponent<Animation>().CrossFade(sway);
			weaponScript.Running();
			callOnce = false;
		}
		else if (codcontroller.state == 2 && withWeapon && codcontroller.velMagnitude > 0.1 && codcontroller.grounded)
		{
			if (Mathf.Abs(Input.GetAxis("Vertical")) != 0 || Mathf.Abs(Input.GetAxis("Horizontal")) != 0)
			{
				var proneTarget = Quaternion.Euler(new Vector3(7, -85, -40));
				transform.localRotation = Quaternion.Slerp(transform.localRotation, proneTarget, Time.deltaTime * 3);
				transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(0.5f, -0.12f, -0.15f), Time.deltaTime * 2);
				weaponScript.Running();
				callOnce = false;
			}
			else
			{
				transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * movementSpeed * 2);
				transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * movementSpeed * 2);
				anim.GetComponent<Animation>().CrossFade(idle, 0.3f);
				weaponScript.StopRunning();
				callOnce = true;
			}

		}
		else
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, 0, 0), Time.deltaTime * movementSpeed * 2);
			transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * movementSpeed * 2);
			anim.GetComponent<Animation>().CrossFade(idle);
			if (!callOnce)
			{
				weaponScript.StopRunning();
				callOnce = true;
			}
		}
	}
}
