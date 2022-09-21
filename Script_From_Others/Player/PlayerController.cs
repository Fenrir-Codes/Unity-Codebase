using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using ScreenLocker;

public class PlayerController : MonoBehaviour
{

	public int proneSpeed = 1;
	public int crouchSpeed = 2;
	public int walkSpeed = 4;
	public int runSpeed = 7;
	public float jumpSpeed = 6.0f;
	private float gravity = 20f;
	public float baseGravity = 24f;
	public float proneGravity = 15f;
	public float normalFDTreshold = 10f;
	public float proneFDTreshold = 5f;
	private float fallingDamageThreshold;
	public float fallDamageMultiplier = 5.0f;
	public float slideSpeed = 12.0f;
	private float antiBumpFactor = .75f;
	public int antiBunnyHopFactor = 1;
	public bool airControl = false;
	//private float crouchingHeight = 0.3f;

	[HideInInspector]
	public bool run;
	//[HideInInspector]
	public bool canRun = true;
	[HideInInspector]
	public bool running;

	private GameObject cameraGO;
	private GameObject cameraGOw;
	[HideInInspector]
	public Vector3 moveDirection = Vector3.zero;
	//[HideInInspector]
	public bool grounded = false;
	private Transform myTransform;
	[HideInInspector]
	public float speed;
	private RaycastHit hit;
	private float fallDistance;
	private bool falling = false;
	public float slideLimit = 45.0f;
	public float rayDistance;
	private Vector3 contactPoint;
	private bool limitDiagonalSpeed = true;
	private int jumpTimer;
	private float normalHeight = -0.2f;
	private float crouchHeight = -1.0f;
	private float proneHeight = -1.6f;

	//[HideInInspector]
	public int state = 0;
	// 0 = standing
	// 1 = crouching
	// 2 = prone


	//var moveSpeed = 2.0;
	//var checkCeiling : boolean;
	private float adjustAnimSpeed = 5.0f;

	//Ladders
	[HideInInspector]
	public bool onLadder = false;
	public float ladderJumpSpeed = 5.0f;
	private LadderUse useladder;

	[HideInInspector]
	public int ladderState = 0;

	private Vector3 currentPosition;
	private Vector3 lastPosition;
	private float highestPoint;
	public AnimationsCamera cameraAnimations;
	public HealthControllerPlayer controllerHealth;
	//var playSoundGO : GameObject;
	public GameObject walkRunAnim;
	public CharacterController controller;
	private float crouchProneSpeed = 4f;
	private float distanceToObstacle;

	//var jumpBack : int = 0;
	public int jumpUp = 0;
	private bool sliding = false;
	public float velMagnitude;
	public bool canVault = true;
	public AudioClip jumpUpSound;
	public AudioClip jumpBackSound;

	//Parachute
	public bool parachute = false;
	public GameObject parachutePrefab;
	public Transform parachuteBone;
	public GameObject tempParachute;
	public float parachuteTimer = 1.0f;
	public WeaponsPlayer PlayerWep;
	public PauseMenu Menu;

	void Start()
	{
		//controller = GetComponent(CharacterController);
		cameraGO = GameObject.FindWithTag("MouseControl");
		//cameraGOw = GameObject.FindWithTag("WeaponCamera");
		useladder = GetComponent<LadderUse>();
		myTransform = transform;
		speed = walkSpeed;
		rayDistance = controller.height / 2 + 1.1f;
		slideLimit = controller.slopeLimit - .2f;
		jumpTimer = antiBunnyHopFactor;
		walkRunAnim.GetComponent<Animation>().wrapMode = WrapMode.Loop;
		walkRunAnim.GetComponent<Animation>().Stop();
		ladderState = 0;
	}

	public void Update()
	{
		velMagnitude = controller.velocity.magnitude;

		var inputX = Input.GetAxis("Horizontal");
		var inputY = Input.GetAxis("Vertical");
		var inputModifyFactor = (inputX != 0.0f && inputY != 0.0f && limitDiagonalSpeed) ? .7071f : 1.0f;

		if (!grounded && tempParachute == null)
		{
			parachuteTimer -= Time.deltaTime;
			if (parachuteTimer < 0.0)
			{
				//if (Input.GetButtonDown("Jump"))
				if (Input.GetKeyDown(KeyCode.Space))
				{
					StartCoroutine(SpawnParachute());
				}
			}
		}
		else
		{
			parachuteTimer = 1.0f;
		}

		if (grounded)
		{
			gravity = baseGravity;

			if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance))
			{
				float hitangle = Vector3.Angle(hit.normal, Vector3.up);
				if (hitangle > slideLimit)
				{
					sliding = true;
				}
				else
				{
					sliding = false;
				}
			}

			if (canRun && cameraGO.transform.localPosition.y > normalHeight - 0.1)
			{
				if (Input.GetButton("Run") && Input.GetKey("w"))
				{
					run = true;
				}
				else
				{
					run = false;
				}
			}

			if (falling)
			{
				if (state == 2) fallingDamageThreshold = proneFDTreshold;
				else fallingDamageThreshold = normalFDTreshold;

				falling = false;
				fallDistance = highestPoint - currentPosition.y;
				if (fallDistance > fallingDamageThreshold)
				{
					ApplyFallingDamage(fallDistance);
				}
				if (fallDistance < fallingDamageThreshold && fallDistance > 0.1)
				{
					if (state == 2) controllerHealth.Grounded(2);
					else controllerHealth.Grounded(1);
				}
			}

			if (sliding)
			{
				var hitNormal = hit.normal;
				moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
				Vector3.OrthoNormalize(ref hitNormal, ref moveDirection);
				moveDirection *= slideSpeed;
			}
			else
			{

				if (state == 0)
				{
					if (run)
					{
						speed = runSpeed;
					}
					else
					{
						if (Input.GetButton("Fire2"))
						{
							speed = crouchSpeed;
						}
						else
						{
							speed = walkSpeed;
						}
					}
				}
				else if (state == 1)
				{
					speed = crouchSpeed;
					run = false;
				}
				else if (state == 2)
				{
					speed = proneSpeed;
					run = false;
				}
				if (ScreenLock.lockCursor) moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
				else moveDirection = new Vector3(0, -antiBumpFactor, 0);
				moveDirection = myTransform.TransformDirection(moveDirection);
				moveDirection *= speed;

				if (!Input.GetButton("Jump"))
				{
					jumpTimer++;
				}
				else if (jumpTimer >= antiBunnyHopFactor)
				{
					jumpTimer = 0;
					if (state == 0)
					{
						if (Input.GetAxis("Mouse Y") > 3.0)
						{
							Debug.Log(Input.GetAxis("Mouse Y"));
							moveDirection.y = jumpSpeed + jumpSpeed / 3;
						}
						else
						{
							moveDirection.y = jumpSpeed;
						}
					}

					if (state > 0)
					{
						CheckDistance();
						if (distanceToObstacle > 1.6)
						{
							state = 0;
						}
					}
				}
			}

		}
		else
		{
			//Calculate highest point when we jump
			currentPosition = myTransform.position;
			if (currentPosition.y > lastPosition.y)
			{
				highestPoint = myTransform.position.y;
				falling = true;
			}
			//Save fall start point
			if (!falling)
			{
				highestPoint = myTransform.position.y;
				falling = true;
			}

			if (airControl)
			{

				moveDirection.x = inputX * speed;
				moveDirection.z = inputY * speed;
				moveDirection = myTransform.TransformDirection(moveDirection);
				Debug.Log(controller.velocity.z);
			}

			if (parachute)
			{
				moveDirection.x = 0.0f;
				moveDirection.z = 4.0f + (inputY * 8.0f);
				moveDirection = myTransform.TransformDirection(moveDirection);
				if (moveDirection.y > -10.0f)
				{
					moveDirection.y = -10.0f;
				}
			}
		}

		if (grounded)
		{
			if (velMagnitude > crouchSpeed && !running && !Input.GetButton("Fire2"))
			{
				walkRunAnim.GetComponent<Animation>()["WalkAnimation"].speed = velMagnitude / adjustAnimSpeed;
				walkRunAnim.GetComponent<Animation>().CrossFade("WalkAnimation");
			}
			else
			{
				walkRunAnim.GetComponent<Animation>().CrossFade("IdleAnimation");
			}

			if (run && velMagnitude > walkSpeed)
			{
				cameraAnimations.CameraRunAnim();
				running = true;
			}
			else
			{
				cameraAnimations.CameraIdleAnim();
				running = false;
			}
			//Reset parkour jump
			jumpUp = 0;
			//jumpBack = 0;

		}
		else
		{
			walkRunAnim.GetComponent<Animation>().CrossFade("IdleAnimation");
			cameraAnimations.CameraIdleAnim();
			running = false;
			run = false;
		}


		if (Input.GetButtonDown("Crouch"))
		{
			CheckDistance();

			if (state == 0)
			{
				state = 1;

			}
			else if (state == 1)
			{
				if (distanceToObstacle > 1.6)
				{
					state = 0;
				}
			}
			else if (state == 2)
			{
				if (distanceToObstacle > 1)
				{
					state = 1;
				}
			}
		}

		if (Input.GetButtonDown("GoProne"))
		{
			CheckDistance();
			if (state == 0 || state == 1)
			{
				state = 2;
			}
			else if (state == 2)
			{
				if (distanceToObstacle > 1.6)
				{
					state = 0;
				}
			}
			if (!grounded) gravity = proneGravity;
		}

		Vector3 cameraGOPos = cameraGO.transform.localPosition; //C# Why?

		if (state == 0)
		{ //Stand Position
			controller.height = 2.0f;
			controller.center = Vector3.zero;

			if (cameraGOPos.y > normalHeight)
			{
				cameraGOPos.y = normalHeight;
			}
			else if (cameraGOPos.y < normalHeight)
			{
				cameraGOPos.y += Time.deltaTime * crouchProneSpeed;
			}

			cameraGO.transform.localPosition = cameraGOPos;

		}
		else if (state == 1)
		{ //Crouch Position

			controller.height = 1.4f;
			controller.center = new Vector3(0, -0.3f, 0);
			if (cameraGO.transform.localPosition.y != crouchHeight)
			{
				if (cameraGOPos.y > crouchHeight)
				{
					cameraGOPos.y -= Time.deltaTime * crouchProneSpeed;
				}
				if (cameraGOPos.y < crouchHeight)
				{
					cameraGOPos.y += Time.deltaTime * crouchProneSpeed;
				}

				cameraGO.transform.localPosition = cameraGOPos;
			}

		}
		else if (state == 2)
		{ //Prone Position

			controller.height = 1f;
			controller.center = new Vector3(0, -0.5f, 0);

			if (cameraGOPos.y < proneHeight)
			{
				cameraGOPos.y = proneHeight;
			}
			else if (cameraGOPos.y > proneHeight)
			{
				cameraGOPos.y -= Time.deltaTime * crouchProneSpeed;
			}

			cameraGO.transform.localPosition = cameraGOPos;
		}

		//Vector3 cameraGOwPos = cameraGOw.transform.localPosition;

		//cameraGOwPos.y = cameraGO.transform.localPosition.y;

		//cameraGOw.transform.localPosition = cameraGOwPos;

		if (ladderState == 3 && grounded)
		{
			ladderState = 2;
		}

		// Apply gravity
		if (!parachute)
		{
			moveDirection.y -= gravity * Time.deltaTime;
		}
	}

	public void CheckDistance()
	{
		Vector3 pos = transform.position + controller.center - new Vector3(0, controller.height / 2, 0);
		RaycastHit hit;
		if (Physics.SphereCast(pos, controller.radius, transform.up, out hit, 10))
		{
			distanceToObstacle = hit.distance;
			Debug.DrawLine(pos, hit.point, Color.red, 2.0f);
		}
		else
		{
			distanceToObstacle = 3;
		}
	}

	public void LateUpdate()
	{
		lastPosition = currentPosition;

		if (onLadder)
		{
			useladder.LadderUpdate();
			highestPoint = myTransform.position.y;
			running = false;
			fallDistance = 0.0f;
			grounded = false;
			walkRunAnim.GetComponent<Animation>().CrossFade("IdleAnimation");
			cameraAnimations.CameraIdleAnim();
			return;
		}

		grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
	}

	public IEnumerator SpawnParachute()
	{

		RaycastHit hits;
		if (Physics.Raycast(myTransform.position, -Vector3.up, out hits, 15f))
			yield break;

		parachuteTimer = 1.0f;
		moveDirection.y = -2.0f;

		tempParachute = Instantiate(parachutePrefab, parachuteBone.position, parachuteBone.rotation);
		tempParachute.transform.parent = parachuteBone;
		parachute = true;
		StartCoroutine(controllerHealth.Shake(0.5f));
		yield return new WaitForSeconds(0.2f);
		controllerHealth.Grounded(1);
	}


	void DetachParachute()
	{
		tempParachute.transform.parent = null;
		tempParachute.BroadcastMessage("parachuteDestroy");
		parachute = false;
	}

	void ApplyFallingDamage(float fallDistance)
	{

		if (parachute)
		{
			controllerHealth.Grounded(1);
			DetachParachute();
		}
		else
		{
			controllerHealth.PlayerFallDamage((int)(fallDistance * fallDamageMultiplier));
		}
	}

	public void JumpOffLadder()
	{
		onLadder = false;
		//Jump in look direction.
		Vector3 jump = cameraGO.transform.forward + new Vector3(0, 0.2f, 0);
		moveDirection = jump * ladderJumpSpeed;
	}

	public void OnLadder()
	{
		onLadder = true;
		moveDirection = Vector3.zero;
		grounded = false;
		if (ladderState == 0)
		{
			ladderState = 1;
		}
	}

	public void OffLadder(object ladderMovement)
	{
		onLadder = false;
		Vector3 dir = gameObject.transform.forward;
		if (Input.GetAxis("Vertical") > 0)
		{
			moveDirection = dir.normalized * 5;
		}
	}
	//Parkour jumps
	public void JumpFromWall()
	{
		if (!onLadder)
		{
			if (Input.GetKey(KeyCode.W))
			{

				if (jumpUp == 0)
				{
					Vector3 jumpwallup = new Vector3(0, 3.5f, 0);
					moveDirection = jumpwallup * 3.0f;
					jumpUp++;
					PlayJumpSounds(jumpUpSound, transform.position, .5f);
				}
			}
			else
			{

				Vector3 jumpwall = -transform.forward + new Vector3(0, 2.5f, 0);
				moveDirection = jumpwall * 7.0f;

				PlayJumpSounds(jumpBackSound, transform.position, 1.0f);

			}
		}
	}

	AudioSource PlayJumpSounds(AudioClip clip, Vector3 position, float volume)
	{
		var go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = go.AddComponent<AudioSource>();
		source.clip = clip;
		source.volume = volume;
		source.Play();
		Destroy(go, clip.length);
		return source;
	}
}
