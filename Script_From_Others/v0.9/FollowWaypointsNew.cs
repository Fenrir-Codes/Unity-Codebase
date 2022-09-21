using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowWaypointsNew : MonoBehaviour {

	public Transform[] waypoint;
	public float speed;
	public int currentWaypoint;
	private Transform myTransform;
	public Transform jetModel;
	public float rotationSpeed;
	public float tiltSpeed;
	public float takeNextWPTimer;
	public Rigidbody antiRocket;
	public bool enabledAntiRocket;
	public string GOTarget = "JetAI";
	public string AntiGOTarget = "AntiRocket";

	public void Start()
	{
		myTransform = transform;
	}

	public void Update()
	{
		Vector3 target = Vector3.zero;
		if (currentWaypoint < waypoint.Length)
		{
			target = waypoint[currentWaypoint].position;
			Vector3 distance = waypoint[currentWaypoint].position - myTransform.position;
			if (distance.magnitude < 15)
			{
				if (currentWaypoint >= (waypoint.Length - 1))
				{
					currentWaypoint = 0;
				}
				else
				{
					currentWaypoint++;
					takeNextWPTimer = 0f;
				}
			}
			else
			{
				myTransform.position = myTransform.position + ((myTransform.forward * speed) * Time.deltaTime);
			}
		}
		else
		{
			target = waypoint[0].position;
		}
		if (takeNextWPTimer < 11f)
		{
			takeNextWPTimer = takeNextWPTimer + Time.deltaTime;
			if (takeNextWPTimer > 10.7f)
			{
				currentWaypoint++;
				takeNextWPTimer = 0f;
			}
		}
		transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(waypoint[currentWaypoint].position - transform.position), rotationSpeed * Time.deltaTime);
		Vector3 targetDir = waypoint[currentWaypoint].position - myTransform.position;
		Vector3 forward = myTransform.forward;
		float angle = Vector3.Angle(targetDir, forward);
		Vector3 cross = Vector3.Cross(targetDir, forward);
		if (cross.y < 0)
		{
			angle = -angle;
		}
		Quaternion tiltTarget = Quaternion.Euler(0, 0, angle);
		jetModel.localRotation = Quaternion.Slerp(jetModel.localRotation, tiltTarget, Time.deltaTime * tiltSpeed);
	}

	public virtual IEnumerator ChangeTarget()
	{
		if (enabledAntiRocket)
		{
			int i = 0;
			while (i < 2)
			{
				i++;
				yield return new WaitForSeconds(0.1f);
				gameObject.name = GOTarget;
				Rigidbody antiR = Instantiate(antiRocket, transform.position, transform.rotation);
				Vector3 moveDirection = transform.TransformDirection(new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 0f), 20));
				antiR.AddForce(moveDirection, ForceMode.Impulse);
				antiR.gameObject.name = AntiGOTarget;
				yield return null;
			}
		}
	}

}
