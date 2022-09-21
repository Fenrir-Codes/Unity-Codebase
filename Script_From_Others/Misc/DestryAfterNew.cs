using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestryAfterNew : MonoBehaviour {

	public float destroyAfter = 10.0f;

	public void Start () {

		Destroy(gameObject, destroyAfter);
	}
}
