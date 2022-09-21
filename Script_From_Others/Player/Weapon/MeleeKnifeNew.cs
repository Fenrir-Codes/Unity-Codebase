using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeKnifeNew : MonoBehaviour {

	public float force = 500;
	public float knifeAttackRange = 3.0f;
	public float damage = 300;
	public Animation anim;
	public Animation cameraKnAnim;
	public AudioClip slash;

	public string knifeAnimation = "";
	public string cameraKnifeAnim = "";
	public float waitTime = 1.0f;
	public GameObject knifeGO;

	public GameObject defaultParticles;
	public GameObject bloodParticles;
	public GameObject woodParticles;
	public GameObject metalParticles;
	public GameObject concreteParticles;
	public LayerMask layerMask;
	public Transform go;
	public bool attack = false;
	public AudioClip audioConcrete;
	public AudioClip audioEnemy;
	public AudioClip audioMetal;
	private int id;


	void Start()
	{
		RendererBool(false);
	}

	public IEnumerator KnifeAttack()
	{
		id = 0;
		attack = true;
		PlayAudioClip(slash, transform.position + new Vector3(0, 1, 0), 0.4f);

		anim[knifeAnimation].speed = 3.0f;
		anim.Play(knifeAnimation);
		cameraKnAnim[cameraKnifeAnim].speed = 1.0f;
		cameraKnAnim.Play(cameraKnifeAnim);

		yield return new WaitForSeconds(0.3f);
		attack = false;
		GetComponent<AudioSource>().Stop();
		yield return new WaitForSeconds(waitTime);
		RendererBool(false);

	}

	public void Update()
	{
		if (attack)
		{
			RendererBool(true);

			var direction = go.position - transform.position;
			RaycastHit hit;
			if (Physics.Raycast(transform.position, direction, out hit, 100, layerMask))
			{
				var contact = hit.point;
				var rotation = Quaternion.FromToRotation(Vector3.up, transform.right);

				if (hit.distance < knifeAttackRange)
				{
					if (hit.transform.tag == "Untagged")
					{
						var def = Instantiate(defaultParticles, contact, rotation) as GameObject;
						def.transform.parent = hit.transform;
						if (!GetComponent<AudioSource>().isPlaying)
						{
							GetComponent<AudioSource>().clip = audioConcrete;
							GetComponent<AudioSource>().volume = 0.3f;
							//audio.loop = true;
							GetComponent<AudioSource>().Play();
						}
					}

					if (hit.transform.tag == "Enemy")
					{
						if (hit.collider.GetInstanceID() != id)
						{
							id = hit.collider.GetInstanceID();
							hit.collider.SendMessageUpwards("ApplyKnifeDamage", damage, SendMessageOptions.DontRequireReceiver);
							var blood = Instantiate(bloodParticles, contact, rotation) as GameObject;
							blood.transform.parent = hit.transform;
							PlayAudioClip(audioEnemy, transform.position, 1.0f);
						}
					}

					if (hit.transform.tag == "Wood")
					{
						KnifeDamage(hit);
						var wood = Instantiate(woodParticles, contact, rotation) as GameObject;
						wood.transform.parent = hit.transform;
					}

					if (hit.transform.tag == "Glass")
					{

						hit.collider.SendMessageUpwards("ApplyDamage", 10000, SendMessageOptions.DontRequireReceiver);
					}

					if (hit.transform.tag == "Metal")
					{
						KnifeDamage(hit);
						var metal = Instantiate(metalParticles, contact, rotation) as GameObject;
						metal.transform.parent = hit.transform;
						if (!GetComponent<AudioSource>().isPlaying)
						{
							GetComponent<AudioSource>().clip = audioMetal;
							GetComponent<AudioSource>().volume = 1.0f;
							//audio.loop = true;
							GetComponent<AudioSource>().Play();
						}
					}

					if (hit.transform.tag == "Concrete")
					{
						var concrete = Instantiate(concreteParticles, contact, rotation);
						concrete.transform.parent = hit.transform;
						if (!GetComponent<AudioSource>().isPlaying)
						{
							GetComponent<AudioSource>().clip = audioConcrete;
							GetComponent<AudioSource>().volume = 0.3f;
							GetComponent<AudioSource>().loop = true;
							GetComponent<AudioSource>().Play();
						}
					}

					if (hit.transform.tag == "SentryGun")
					{
						KnifeDamage(hit);
						var sentry = Instantiate(metalParticles, contact, rotation) as GameObject;
						sentry.transform.parent = hit.transform;
					}

					if (hit.rigidbody)
					{
						hit.rigidbody.AddForceAtPosition(force * transform.TransformDirection(1, 0, 0), hit.point);
					}

				}
				else
				{
					GetComponent<AudioSource>().Stop();
				}
			}
		}
	}

	public void KnifeDamage(RaycastHit knifeHit)
	{
		if (knifeHit.collider.GetInstanceID() != id)
		{
			id = knifeHit.collider.GetInstanceID();
			knifeHit.collider.SendMessageUpwards("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
		}
	}

	public void RendererBool(bool status)
	{
		var gos = knifeGO.GetComponentsInChildren(typeof(Renderer));
		foreach (Renderer go in gos)
		{
			go.enabled = status;
		}
	}

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
	{
		var go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.pitch = Time.timeScale;
		source.Play();
		Destroy(go, clip.length);
		return source;
	}
}
