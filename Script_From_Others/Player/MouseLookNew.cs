using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScreenLocker;

public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }

[AddComponentMenu("Camera-Control/Mouse Look New")]
public class MouseLookNew : MonoBehaviour {
	public RotationAxes axes = RotationAxes.MouseXAndY;

	private float sensitivity;
	private float savedSensitivity = 2.0f;

	private float minimumX = -360f;
	private float maximumX = 360f;

	private float minimumY;
	private float maximumY;
	public float normalMinimumY = -90f;
	public float normalMaximumY = 85f;
	public float proneMinimumY = -20f;
	public float proneMaximumY = 40f; 

	private float rotationX = 0f;
	private float rotationY = 0f;

	[HideInInspector]
	public float offsetX = 0f;
	[HideInInspector]
	public float offsetY = 0f;
	[HideInInspector]
	public float kickTime = 0f;

	public Camera FirstCamera;
	public Camera[] SecondCamera;
	public HUDManager HUD;
	public PlayerController controllerScript;
	private Quaternion originalRotation;

	public void Start()
	{
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;

		if (HUD == null && FindObjectOfType<HUDManager>() != null)
			HUD = FindObjectOfType<HUDManager>();

		originalRotation = transform.localRotation;
	}

	public void Update()
	{
		if (!ScreenLock.lockCursor) return;
		if ((controllerScript.state == 0) || (controllerScript.state == 1)) //standing or crouching
		{
			sensitivity = HUD.sensitivity * Time.timeScale; //you can add " * Time.timeScale" if you want affect sensitivity in slow motion
			minimumY = normalMinimumY;
			maximumY = normalMaximumY;
		}
		else
		{
			if (controllerScript.state == 2) //prone
			{
				sensitivity = (HUD.sensitivity / 2) * Time.timeScale; //you can add " * Time.timeScale" if you want affect sensitivity in slow motion
				minimumY = proneMinimumY;
				maximumY = proneMaximumY;
			}
		}
		if (axes == RotationAxes.MouseXAndY)
		{
			rotationX = rotationX + (((Input.GetAxis("Mouse X") * sensitivity) / 30) * (FirstCamera.fieldOfView + offsetX));
			rotationY = rotationY + (((Input.GetAxis("Mouse Y") * sensitivity) / 30) * (FirstCamera.fieldOfView + offsetY));
			rotationX = ClampAngle(rotationX, minimumX, maximumX);
			rotationY = ClampAngle(rotationY, minimumY, maximumY);
			Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
			Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
			transform.localRotation = (originalRotation * xQuaternion) * yQuaternion;
			for (int i = 0; i < SecondCamera.Length; i++)
			{
				SecondCamera[i].transform.localRotation = FirstCamera.transform.localRotation;
			}
		}
		else
		{
			if (axes == RotationAxes.MouseX)
			{
				rotationX += Input.GetAxis("Mouse X") * sensitivity /30 * FirstCamera.fieldOfView + offsetX;
				rotationX = ClampAngle (rotationX, minimumX, maximumX);
				Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
				transform.localRotation = originalRotation * xQuaternion;
				for (int i = 0; i < SecondCamera.Length; i++)
				{
					SecondCamera[i].transform.localRotation = FirstCamera.transform.localRotation;
				}
				//transform.localRotation = originalRotation * xQuaternion;
				//transform.Rotate(0, (((Input.GetAxis("Mouse X") * sensitivity) / 30) * FirstCamera.fieldOfView) + offsetX, 0);
			}
			else
			{
				if ((rotationY > proneMaximumY) && (controllerScript.state == 2))
				{
					transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(-35, 0, 0), Time.deltaTime * 5);
					rotationY = 360 - transform.rotation.eulerAngles.x;
				}
				else
				{
					if ((rotationY < proneMinimumY) && (controllerScript.state == 2))
					{
						transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(15, 0, 0), Time.deltaTime * 4);
						rotationY = -transform.rotation.eulerAngles.x;
					}
					else
					{
						rotationY = rotationY + ((((Input.GetAxis("Mouse Y") * sensitivity) / 60) * FirstCamera.fieldOfView) + offsetY);
						rotationY = ClampAngle(rotationY, minimumY, maximumY);
						var yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
						transform.localRotation = originalRotation * yQuaternion;
					}
				}
				for (int i = 0; i < SecondCamera.Length; i++)
				{
					SecondCamera[i].transform.localRotation = FirstCamera.transform.localRotation;
				}
			}
		}
		//KickBack from weaponscript
		if (kickTime < 0)
		{
			//reset offset
			offsetY = Mathf.SmoothDamp(offsetY, 0f, ref offsetY, Time.deltaTime);
			offsetX = Mathf.SmoothDamp(offsetX, 0f, ref offsetX, Time.deltaTime);
		}
		else
		{
			kickTime = kickTime - Time.deltaTime;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
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
