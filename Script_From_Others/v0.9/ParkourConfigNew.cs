using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParkourConfigNew : MonoBehaviour {

	public Vector3 endPosition;
	public float climbTime = 0.6f;
	public bool climbOver = false;

	void Start()
	{
		gameObject.GetComponent<MeshRenderer>().enabled = false;
	}
}
