using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionAdvancedNew : MonoBehaviour {

	public float explosionRadius = 5.0f;
	public float explosionPower = 10.0f;
	public float explosionDamage = 100.0f;
	public float explosionTimeout = 2.0f;

	public void Start () {
	
		var explosionPosition = transform.position;

		// Apply damage to close by objects first
		Collider[] colliders = Physics.OverlapSphere (explosionPosition, explosionRadius);
		RaycastHit hit;
		foreach (Collider col in colliders){

			HealthControllerPlayer healthScript = col.GetComponent<HealthControllerPlayer>();
		
			//var findTarget = col.gameObject.Find("TargetPoint");
			if (healthScript != null){
				StartCoroutine(healthScript.Shake(1));
				Vector3 dir = healthScript.transform.position - explosionPosition;
			
				if (Physics.Raycast (explosionPosition, dir, out hit, explosionRadius/3)) {
					if(hit.transform.GetComponent<Collider>() == healthScript.GetComponent<Collider>()){

						// Calculate distance from the explosion position to the closest point on the collider
						//var closestPoint = hit.collider.ClosestPointOnBounds(explosionPosition);
						var distance = Vector3.Distance(hit.collider.transform.position + new Vector3(0,1,0), explosionPosition);
						//Debug.DrawLine ( explosionPosition, hit.collider.transform.position + Vector3(0,1,0), Color.red, 2.0);
						// The hit points we apply fall decrease with distance from the explosion point
						var hitPoints = 1.0f - Mathf.Clamp01(distance / explosionRadius);
					
						hitPoints *= explosionDamage;
						healthScript.ApplyDamage((int)hitPoints/2);
					}
				}
			}
		}
	

		// Apply explosion forces to all rigidbodies
		// This needs to be in two steps for ragdolls to work correctly.
		// (Enemies are first turned into ragdolls with ApplyDamage then we apply forces to all the spawned body parts)
		colliders = Physics.OverlapSphere (explosionPosition, explosionRadius/3);
		foreach (var physicsHit in colliders) {
	
			
			//if(physicsHit.collider.tag == "Glass" || physicsHit.collider.tag == "Metal"){
				// physicsHit.collider.transform.root.GetComponent("GlassScript").explPos = explosionPosition;
				if(physicsHit.gameObject.name == "DamageReceiver"){
					physicsHit.GetComponent<Collider>().SendMessageUpwards("ApplyDamage", 10000, SendMessageOptions.DontRequireReceiver);
				}	
			//}
	
			if (physicsHit.GetComponent<Rigidbody>()){
			
				if (Physics.Raycast (explosionPosition, physicsHit.GetComponent<Rigidbody>().position - explosionPosition, out hit, explosionRadius/3)) {
					if(hit.rigidbody){
					
						var hitDistance = Vector3.Distance(hit.collider.transform.position, explosionPosition);
						var damage = 1.0 - Mathf.Clamp01(hitDistance / explosionRadius);
						damage *= explosionDamage;
						//if(hit.collider.tag == "Glass") hit.collider.transform.root.GetComponent("GlassScript").explPos = explosionPosition;
						hit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
						hit.rigidbody.AddExplosionForce(explosionPower, explosionPosition, explosionRadius, 0.0f);
						//Debug.DrawLine ( explosionPosition, hit.rigidbody.transform.position, Color.yellow, 2.0);
					
					}	
				}
			}		
		}
	
		// destroy the explosion after a while
		Destroy (gameObject, explosionTimeout);
	}

	public IEnumerator ApplyForce (Rigidbody body) {
		yield return new WaitForSeconds (.4f);
		var direction = body.transform.position - transform.position;
		body.AddForceAtPosition(direction.normalized, transform.position);
	}
}
