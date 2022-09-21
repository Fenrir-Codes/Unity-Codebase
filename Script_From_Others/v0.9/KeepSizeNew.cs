using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepSizeNew : MonoBehaviour {

	public float adjust;
	public Camera cam;
	public Renderer targetRenderer;
	public GameObject col;
	public Material hitTexture;
	public Material hitLockTexture;

	public void Start()
	{
		cam = Camera.main;
	}

	public void Update()
	{
		//adjust size
		float size = (cam.transform.position - transform.position).magnitude * adjust;
		transform.localScale = new Vector3(size, size, 0);
		//rotate in camera direction
		transform.LookAt(transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);

		if (col.name == "Jet")
		{
			GetComponent<Renderer>().enabled = true;
			GetComponent<Renderer>().material = hitTexture;
		}
		else if (col.name == "JetLock")
		{
			GetComponent<Renderer>().material = hitLockTexture;
		}
		else
		{
			GetComponent<Renderer>().enabled = false;
		}
	}
}
