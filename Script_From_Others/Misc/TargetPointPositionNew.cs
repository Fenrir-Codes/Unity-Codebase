using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPointPositionNew : MonoBehaviour {

	public GameObject target;
	public Vector3 offsetY;

	void Update () {
		gameObject.transform.position = target.transform.position - offsetY;
	}
}
