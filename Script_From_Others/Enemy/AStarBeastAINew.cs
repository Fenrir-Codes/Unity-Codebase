using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarBeastAINew : MonoBehaviour {

	public AIPath aipath;
	public Animation anim;
	public float sleepVelocity;
	public float animationSpeed;
	public Transform myTransform;
	public CharacterController controller;
	private float vel;
	//@HideInInspector
	public Transform target;
	public float attackRange;
	public LayerMask layerMask;
	[HideInInspector]
	public int @event;
	//0 = follow player
	//1 = prepare to attack
	//2 = attack
	//3 = run away
	private float prepareToAttackTime;
	public int attackChoice;
	// 1 = jump attack
	// 2 = shoot powerBall
	// 3 = bite attack
	public float attackTime;
	private float tempAttackTime;
	public Transform backUpPoints;
	private Vector3 moveDirection;
	public Rigidbody projectile;
	public Transform launchPosition;
	private float distance;
	private float tempTime;
	public AudioClip prepareToAttackSound;
	public AudioClip biteSound;
	public float minDamage;
	public float maxDamage;
	public float attackRepeatTime;
	public float normalSpeed;
	public float runAwaySpeed;
	private bool once;
	public int tempAttackChoice;
	public string NameTarget = "PlayerController";

	public void Start()
	{
		target = GameObject.FindWithTag("Player").transform;
		aipath.target = target;
		aipath.speed = normalSpeed;
		myTransform = transform;
		tempAttackChoice = attackChoice;
		anim["walk"].wrapMode = WrapMode.Loop;
		anim["idle"].wrapMode = WrapMode.Loop;
		anim["crouchLook"].wrapMode = WrapMode.ClampForever;
		anim["crouchLook"].speed = 0.7f;
	}

	public void Update()
	{
		if (!target)
		{
			return;
		}
		distance = (target.position - myTransform.position).magnitude;
		vel = controller.velocity.magnitude;
		if (((distance <= attackRange) && (@event == 0)) && (aipath.target.name == NameTarget))
		{
			if (CanSeePlayer())
			{
				prepareToAttackTime = 0.7f;
				@event = 1;
			}
		}
		if ((@event == 1) || ((@event == 2) && (attackChoice != 3)))
		{
			myTransform.rotation = Quaternion.Slerp(myTransform.rotation, Quaternion.LookRotation(target.position - myTransform.position), 4 * Time.deltaTime);
			transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
		}
		if (@event == 0)
		{
			if (vel <= sleepVelocity)
			{
				anim.CrossFade("idle", 0.5f);
			}
			else
			{
				anim.CrossFade("walk", 0.5f);
				AnimationState state = anim["walk"];
				float speed = vel;
				state.speed = speed * animationSpeed;
			}
		}
		else
		{
			if (@event == 1)
			{
				AttckSound();
				aipath.canMove = false;
				anim.CrossFade("crouchLook", 0.5f);
				prepareToAttackTime = prepareToAttackTime - Time.deltaTime;
				if (prepareToAttackTime <= 0f)
				{
					tempAttackTime = attackTime;
					if (attackChoice != 3)
					{
						PlayerController targetState = target.GetComponent<PlayerController>();
						if (targetState.state != 0)
						{
							attackChoice = 3;
							tempAttackTime = 1.3f;
						}
					}
					@event = 2;
				}
			}
			else
			{
				if (@event == 2)
				{
					tempAttackTime = tempAttackTime - Time.deltaTime;
					if (attackChoice == 1)
					{
						if (controller.isGrounded)
						{
							moveDirection = new Vector3(0, 0, 1);
							moveDirection = myTransform.TransformDirection(moveDirection);
							moveDirection = moveDirection * 10;
							moveDirection.y = 5;
						}
						else
						{
							if (distance < 1f)
							{
								moveDirection = new Vector3(0, 0, -1);
								moveDirection = myTransform.TransformDirection(moveDirection);
								moveDirection = moveDirection * 10;
								if (Time.time > tempTime)
								{
									target.SendMessage("ApplyDamage", Random.Range(minDamage, maxDamage), SendMessageOptions.DontRequireReceiver);
									tempTime = Time.time + attackRepeatTime;
									PlayAudioClip(biteSound, transform.position, 1f);
								}
							}
						}
						moveDirection.y = moveDirection.y - (10 * Time.deltaTime);
						controller.Move(moveDirection * Time.deltaTime);
						if ((tempAttackTime <= 0) && controller.isGrounded)
						{
							@event = 3;
						}
						anim.CrossFade("attack", 0.2f);
					}
					else
					{
						if (attackChoice == 2)
						{
							anim.CrossFade("attack", 0.2f);
							if (tempAttackTime < 0.3f)
							{
								Rigidbody proj = Instantiate(projectile, launchPosition.position, launchPosition.rotation);
								proj.velocity = transform.TransformDirection(new Vector3(0, 5, 22));
								@event = 3;
							}
						}
						else
						{
							if (attackChoice == 3)
							{
								tempAttackTime = tempAttackTime - Time.deltaTime;
								if (controller.isGrounded)
								{
									moveDirection = new Vector3(0, 0, 1);
									moveDirection = myTransform.TransformDirection(moveDirection);
									moveDirection = moveDirection * 20;
									moveDirection.y = moveDirection.y - (10 * Time.deltaTime);
									controller.Move(moveDirection * Time.deltaTime);
									anim.Play("walk");
									anim["walk"].speed = 5f;
									if ((Time.time > tempTime) && (distance < 1.5f))
									{
										target.SendMessage("ApplyDamage", Random.Range(minDamage, maxDamage), SendMessageOptions.DontRequireReceiver);
										tempTime = Time.time + attackRepeatTime;
										PlayAudioClip(biteSound, transform.position, 1f);
										@event = 3;
									}
								}
								if (tempAttackTime <= 0)
								{
									@event = 3;
								}
							}
						}
					}
				}
				else
				{
					if (@event == 3)
					{
						once = false;
						attackChoice = tempAttackChoice;
						aipath.target = backUpPoints;//[Random.Range(0, backUpPoints.length)];
						aipath.speed = runAwaySpeed;
						aipath.canMove = true;
						@event = 0;
						StartCoroutine(AttackAgain());
					}
				}
			}
		}
	}

	public IEnumerator AttackAgain()
	{
		yield return new WaitForSeconds(2f);
		aipath.speed = normalSpeed;
		aipath.target = target;
	}

	public void AttckSound()
	{
		if (once)
		{
			return;
		}
		once = true;
		GetComponent<AudioSource>().clip = prepareToAttackSound;
		GetComponent<AudioSource>().Play();
	}

	public bool CanSeePlayer()
	{
		RaycastHit hit = default(RaycastHit);
		if (Physics.Linecast(myTransform.position, target.position + new Vector3(0, -0.3f, 0), out hit, layerMask.value))
		{
			Debug.DrawRay(myTransform.position, target.position, Color.red);
			if (hit.transform.tag == "Player")
			{
				return true;
			}
		}
		return false;
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
