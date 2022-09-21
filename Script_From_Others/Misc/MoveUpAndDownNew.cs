using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUpAndDownNew : MonoBehaviour {

	public float amount;
	public float speed;
	public IEnumerator Start()
	{
		Vector3 pointA = transform.position;
		Vector3 pointB = transform.position + new Vector3(0, amount, 0);
		while (true)
		{
			yield return StartCoroutine(MoveObject(transform, pointA, pointB, speed));
			yield return StartCoroutine(MoveObject(transform, pointB, pointA, speed));
		}
	}

	public IEnumerator MoveObject(Transform thisTransform, Vector3 startPos, Vector3 endPos, float time)
	{
		float i = 0f;
		float rate = 1f / time;
		while (i < 1f)
		{
			i = i + (Time.deltaTime * rate);
			thisTransform.position = Vector3.Lerp(startPos, endPos, i);
			yield return null;
		}
	}
}
