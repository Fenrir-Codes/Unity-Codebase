using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMarksNew : MonoBehaviour {

	//BULLET DECALS
	public Texture2D[] concrete;
	public Texture2D[] wood;
	public Texture2D[] metal;
	//var oldMetal : Texture2D[];
	//var glass : Texture2D[];
	public Texture2D transparent;
	//BULLET HIT PARTICLES
	public GameObject[] ConcreteParticles;
	public GameObject[] WoodParticles;
	public GameObject[] MetalParticles;
	//  var GlassParticles : GameObject[];
	public GameObject[] BloodParticles;
	//HIT SOUNDS
	public AudioClip[] hitWoodSound;
	public AudioClip[] hitMetalSound;
	public AudioClip[] hitConcreteSound;
	public float hitVolume;
	public float destroyAfter;

	public void BulletHitObject(HitTypeBullet HitObject, int TypeOfProjectile)
	{
		//TypeOfProjectile 1 = bullet
		//TypeOfProjectile 2 = arrow
		//TypeOfProjectile 3 = don't play sounds

		Texture2D useTexture;

		switch (HitObject)
		{
			case HitTypeBullet.SENTRYGUN:
				useTexture = transparent;

				if (MetalParticles == null || MetalParticles.Length == 0) return;
				Instantiate(MetalParticles[Random.Range(0, MetalParticles.Length)], transform.position, transform.rotation);
				GetComponent<AudioSource>().PlayOneShot(hitMetalSound[Random.Range(0, hitMetalSound.Length)], hitVolume);
				break;

			case HitTypeBullet.BODY:
				useTexture = transparent;

				if (BloodParticles == null || BloodParticles.Length == 0) return;
				Instantiate(BloodParticles[Random.Range(0, BloodParticles.Length)], transform.position, transform.rotation);
				break;

			case HitTypeBullet.CONCRETE:
				if (concrete == null || concrete.Length == 0) return;
				useTexture = concrete[Random.Range(0, concrete.Length)];
				if (TypeOfProjectile == 1) GetComponent<AudioSource>().PlayOneShot(hitConcreteSound[Random.Range(0, hitConcreteSound.Length)], hitVolume);

				if (ConcreteParticles == null || ConcreteParticles.Length == 0) return;
				Instantiate(ConcreteParticles[Random.Range(0, ConcreteParticles.Length)], transform.position, transform.rotation);
				break;

			case HitTypeBullet.WOOD:
				if (wood == null || wood.Length == 0) return;
				useTexture = wood[Random.Range(0, wood.Length)];
				if (TypeOfProjectile == 1) GetComponent<AudioSource>().PlayOneShot(hitWoodSound[Random.Range(0, hitWoodSound.Length)], hitVolume);

				if (WoodParticles == null || WoodParticles.Length == 0) return;
				Instantiate(WoodParticles[Random.Range(0, WoodParticles.Length)], transform.position, transform.rotation);
				break;

			case HitTypeBullet.METAL:
				if (metal == null || metal.Length == 0) return;
				useTexture = metal[Random.Range(0, metal.Length)];

				if (TypeOfProjectile == 1) GetComponent<AudioSource>().PlayOneShot(hitMetalSound[Random.Range(0, hitMetalSound.Length)], 1.0f);
				else if (TypeOfProjectile == 2) GetComponent<AudioSource>().PlayOneShot(hitMetalSound[0], 1.0f);

				if (MetalParticles == null || MetalParticles.Length == 0) return;
				Instantiate(MetalParticles[Random.Range(0, MetalParticles.Length)], transform.position, transform.rotation);
				break;
			/*	
			case HitTypeBullet.OLD_METAL:
				if(oldMetal == null || oldMetal.Length == 0) return;
				useTexture = oldMetal[Random.Range(0, oldMetal.Length)];
				
				if(MetalParticles == null || MetalParticles.Length == 0) return;
				Instantiate (MetalParticles[Random.Range(0, MetalParticles.Length)], transform.position, transform.rotation);
				break;
				
			case HitTypeBullet.GLASS:
				if(glass == null || glass.Length == 0) return;
				useTexture = glass[Random.Range(0, glass.Length)];
				
				if(GlassParticles == null || GlassParticles.Length == 0) return;
				Instantiate (GlassParticles[Random.Range(0, GlassParticles.Length)], transform.position, transform.rotation);
				break;
			*/
			default:
				if (concrete == null || concrete.Length == 0) return;
				useTexture = concrete[Random.Range(0, concrete.Length)];
				if (TypeOfProjectile == 1) GetComponent<AudioSource>().PlayOneShot(hitConcreteSound[Random.Range(0, hitConcreteSound.Length)], hitVolume);

				if (ConcreteParticles == null || ConcreteParticles.Length == 0) return;
				Instantiate(ConcreteParticles[Random.Range(0, ConcreteParticles.Length)], transform.position, transform.rotation);
				return;
		}


		GetComponent<Renderer>().material.mainTexture = useTexture;
		Destroy(gameObject, destroyAfter);
	}
}
