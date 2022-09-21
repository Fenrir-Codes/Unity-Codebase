using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AstarZombieNew : MonoBehaviour {

	private Transform target; //the enemy's target
	public float attackRange;
	public float attackRepeatTime; // delay between attacks
	public float minDamage;
	public float maxDamage;
	public AudioClip attackSound;
	public float attackTime;
	private CharacterController controller;
	private bool isplaying;
	public AudioClip[] walkSound;
	public float audioStepLength;
	public Animation anim;
	public string idleAnim;
	public string walkAnim;
	public string attackAnim;
	public float movementSpeed;
	public AIFollow aipath;
	private Transform myTransform;
	public GameObject bloodParticles;
	public Transform particlePos;
	public GameObject otherTarget;
	public float targetHealth;
	public bool inAttck;

	void Start()
	{
		myTransform = transform;
		target = GameObject.FindWithTag("Player").transform;
		controller = (CharacterController)GetComponent(typeof(CharacterController));
		anim[walkAnim].wrapMode = WrapMode.Loop;
		anim[walkAnim].speed = 1.2f;
		anim[attackAnim].wrapMode = WrapMode.Once;
		anim[attackAnim].layer = 10;
	}

	void Update()
	{
		float distance = (target.position - myTransform.position).magnitude;
		if (controller.velocity.magnitude > 0.1f)
		{
			if (!isplaying && controller.isGrounded)
			{
				StartCoroutine(playWalkSounds());
				anim.CrossFade(walkAnim, 0.2f);
			}
		}
		else
		{
			anim.CrossFade(idleAnim, 0.2f);
		}
		if (((distance < attackRange) && (targetHealth >= 0)) || inAttck)
		{
			myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), 2 * Time.deltaTime);
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
			aipath.speed = 0f;
		}
		else
		{
			aipath.speed = movementSpeed;
		}
		if ((distance < attackRange) && (Time.time > attackTime))
		{
			targetHealth = target.GetComponent<HealthControllerPlayer>().hitPoints;
			if (targetHealth <= 0)
			{
				aipath.target = otherTarget.transform;
			}
			else
			{
				inAttck = true;
				attackTime = Time.time + attackRepeatTime;
				anim.CrossFadeQueued(attackAnim, 0.6f, QueueMode.PlayNow);//CrossFade(attackAnim);
				StartCoroutine(AttackEffects());
			}
		}
	}

	public IEnumerator AttackEffects()
	{
		yield return new WaitForSeconds(0.3f);
		target.SendMessage("ApplyDamage", Random.Range(minDamage, maxDamage));
		PlayAudioClip(attackSound, transform.position, 1f);
		Instantiate(bloodParticles, particlePos.position, particlePos.rotation);
		yield return new WaitForSeconds(1.2f);
		inAttck = false;
	}

	public IEnumerator playWalkSounds()
	{
		isplaying = true;
		PlayAudioClip(walkSound[Random.Range(0, walkSound.Length)], transform.position, 1f);
		yield return new WaitForSeconds(audioStepLength);
		isplaying = false;
	}

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
	{
		GameObject go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = (AudioSource)go.AddComponent(typeof(AudioSource));
		source.clip = clip;
		source.volume = volume;
		source.pitch = Random.Range(0.95f, 1.05f);
		source.Play();
		Destroy(go, clip.length);
		return source;
	}
}
