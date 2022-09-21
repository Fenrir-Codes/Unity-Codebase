using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPartsDamageNew : MonoBehaviour {

	public EnemyDamageRagdollNew healthScript;
	public float multiplier = 2.0f;

	public void ApplyDamage (float damage) {
		healthScript.Headshot(damage * multiplier);
	}
}
