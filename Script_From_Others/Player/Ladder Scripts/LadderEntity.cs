using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderEntity : MonoBehaviour {

	private Vector3 climbDirection = Vector3.zero;

	void Start ()
	{
		climbDirection = (gameObject.transform.position + new Vector3(0,GetComponent<Collider>().bounds.size.y/2,0)) - (gameObject.transform.position - new Vector3(0,GetComponent<Collider>().bounds.size.y/2,0));
	}

	public Vector3 ClimbDirection ()
	{
		return climbDirection;
	}
}
