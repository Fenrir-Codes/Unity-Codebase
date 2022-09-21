using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuggyAdjustRotationNew : MonoBehaviour {

	public float returnSpeed;
	public CarController car;
	public Transform camHolder;
	public Texture damageTexture;
	public float alpha;
	public void Update()
	{
		if (car.fuel > 0f)
		{
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
		}
		camHolder.localRotation = Quaternion.Slerp(camHolder.localRotation, Quaternion.identity, Time.deltaTime * 2);
		if (alpha > 0f)
		{
			alpha = alpha - (Time.deltaTime / 4);
		}
	}

	public void Crash()
	{
		StartCoroutine(CrashHit(camHolder, new Vector3(Random.Range(-5, 5), 20, 0), 0.05f));
	}

	public IEnumerator CrashHit(Transform goTransform, Vector3 dir, float time)
	{
		alpha = 1f;
		Quaternion startRotation = goTransform.localRotation;
		Quaternion endRotation = goTransform.localRotation * Quaternion.Euler(dir);
		float rate = 1f / time;
		float t = 0f;
		while (t < 1f)
		{
			t = t + (Time.deltaTime * rate);
			goTransform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
			yield return null;
		}
	}

	public void OnGUI()
	{
		GUI.color = new Color(1f, 1f, 1f, alpha); //Color (r,g,b,a)
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), damageTexture);
	}
}
