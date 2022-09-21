using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationAxes2
{
	MouseX = 0,
	MouseY = 1,
	MouseXandY = 2
}

public class JSmouseLookNew : MonoBehaviour {

	public RotationAxes2 axes;
	private RotationAxes MouseX;
	private RotationAxes MouseY;
	private object MouseXandY;
	public float sensitivityX;
	public float sensitivityY;
	public float minimumX;
	public float maximumX;
	public float minimumY;
	public float maximumY;
	public float rotationX;
	public float rotationY;
	public Quaternion originalRotation;
	public void Update()
	{
		if (!Screen.lockCursor)
		{
			return;
		}
		if (axes == RotationAxes2.MouseXandY)
		{
			rotationX = rotationX + (Input.GetAxis("Mouse X") * sensitivityX);
			rotationY = rotationY + (Input.GetAxis("Mouse Y") * sensitivityY);
			rotationX = ClampAngle(rotationX, minimumX, maximumX);
			rotationY = ClampAngle(rotationY, minimumY, maximumY);
			Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
			Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
			transform.localRotation = (originalRotation * xQuaternion) * yQuaternion;
		}
		else
		{
			if (axes == RotationAxes2.MouseX)
			{
				transform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
			}
			else
			{
				rotationY = rotationY + (Input.GetAxis("Mouse Y") * sensitivityY);
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
				transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);
			}
		}
	}

	public void Start()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
		{
			GetComponent<Rigidbody>().freezeRotation = true;
		}
		originalRotation = transform.localRotation;
	}

	public float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle = angle + 360f;
		}
		if (angle > 360f)
		{
			angle = angle - 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
