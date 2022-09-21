using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LauncherNew : MonoBehaviour {

	public float minDamage = 30;
	public float maxDamage = 40;
	public string DamageTargetName = "PlayerController";

	public void OnCollisionEnter (Collision collision){
		if(collision.collider.name == DamageTargetName)
		{
			collision.collider.SendMessage( "ApplyDamage", Random.Range(minDamage, maxDamage));
		}
		Destroy(transform.root.gameObject);
	}
}
