using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

public class LadderUse : MonoBehaviour {

	public float climbSpeed = 6.0f;
	private float climbDownThreshold = -0.4f;
	private Vector3 climbDirection = Vector3.zero;
	private Vector3 lateralMove = Vector3.zero;
	//private Vector3 forwardMove = Vector3.zero;
	private Vector3 ladderMovement = Vector3.zero;
	[HideInInspector]
	public LadderEntity currentLadder = null;
	private GameObject mainCamera = null;
	private CharacterController controller = null;
	private PlayerController codcontroller;
	//private float temp = 0.0f;
	public AudioClip jumpfromLadders;
	public AudioClip OnLaddersImpact;

	public void Start()
	{
		mainCamera = GameObject.FindWithTag("MainCamera");
		controller = GetComponent<CharacterController>();
		codcontroller = GetComponent<PlayerController>();
	}

	public void OnTriggerEnter(Collider other)
	{
		if (Input.GetButton("Jump"))
		{
			return;
		}
		if (other.CompareTag("Ladder"))
		{
			if (codcontroller.ladderState == 3)
			{
				AudioSource.PlayClipAtPoint(OnLaddersImpact, transform.position);
			}
			if (!codcontroller.grounded && (codcontroller.ladderState == 0))
			{
				AudioSource.PlayClipAtPoint(OnLaddersImpact, transform.position, 0.3f);
			}
			LatchLadder(other.gameObject);
		}
	}

	public void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Ladder"))
		{
			UnlatchLadder();
		}
	}

	public void LatchLadder(GameObject latchedLadder)
	{
		currentLadder = latchedLadder.GetComponent<LadderEntity>();
		climbDirection = currentLadder.ClimbDirection();
		codcontroller.OnLadder();
	}

	public void UnlatchLadder()
	{
		currentLadder = null;
		codcontroller.OffLadder(ladderMovement);
	}

	public void LadderUpdate()
	{
		if (Input.GetButton("Jump"))
		{
			float currRot = Mathf.DeltaAngle(transform.eulerAngles.y, currentLadder.gameObject.transform.eulerAngles.y);
			if ((currRot <= 90f) && (currRot >= -90f))
			{
				JumpUnlatchLadder();
			}
		}
		var verticalMove = climbDirection.normalized;
		verticalMove = verticalMove * Input.GetAxis("Vertical");
		verticalMove = verticalMove * (mainCamera.transform.forward.y > climbDownThreshold ? 1 : -1);
		lateralMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
		lateralMove = transform.TransformDirection(lateralMove);
		ladderMovement = verticalMove + lateralMove;
		controller.Move((ladderMovement * climbSpeed) * Time.deltaTime);
	}

	public void JumpUnlatchLadder()
	{
		PlayClimbSounds(jumpfromLadders, transform.position, 1f);
		codcontroller.JumpOffLadder();
		codcontroller.Update();
		//latchedToLadder = false;
		currentLadder = null;
	}

	public AudioSource PlayClimbSounds(AudioClip clip, Vector3 position, float volume)
	{
		GameObject go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = (AudioSource)go.AddComponent(typeof(AudioSource));
		source.clip = clip;
		source.volume = volume;
		//source.pitch = Random.Range(0.9,1.1);
		source.Play();
		UnityEngine.Object.Destroy(go, clip.length);
		return source;
	}
}
