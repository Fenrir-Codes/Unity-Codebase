using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

public class ParkourControllerNew : MonoBehaviour {

	public bool hitConfig;
	public bool canClimb;
	public bool climbing;
	public bool once;
	private RaycastHit hit;
	private Vector3 endPos;
	private Quaternion endRot;
	public float distance;
	public LayerMask layerMask;
	public PlayerController controller;
	public HealthControllerPlayer codhealth;
	public WeaponsPlayer playerweapons;
	public AudioClip vaultSound;
	public AudioClip vaultSound2;
	public AudioClip vaultSound3;
	public Vector3 adjust;
	public Transform cameraGO;
	//We hide our soldier because we don't have proper animation for parkour
	public SkinnedMeshRenderer legs;
	public SkinnedMeshRenderer shadowLegs;
	public MeshRenderer wep;

	public void Update()
	{
		// just to check if player is in standing position, otherwise disable functionality.
		if (cameraGO.transform.localPosition.y != -0.2f)
		{
			return;
		}
		Vector3 direction = transform.TransformDirection(Vector3.forward);
		Debug.DrawRay(transform.position + adjust, direction * distance, Color.green);
		if (Physics.Raycast(transform.position + adjust, direction, out hit, distance, layerMask))
		{
			ParkourConfigNew pConf = hit.collider.GetComponent<ParkourConfigNew>();
			hitConfig = pConf.climbOver;
			canClimb = true;
			if (Input.GetKey(KeyCode.Space) && !climbing)
			{
				endRot = Quaternion.FromToRotation(-Vector3.forward, hit.normal);
				endPos = pConf.endPosition;
				climbing = true;
				StartCoroutine(MoveObject(transform.position, pConf.climbTime));
			}
		}
		else
		{
			if (Physics.Raycast(transform.position + adjust, direction, out hit, distance))
			{
				if (Input.GetKeyDown(KeyCode.Space) && (controller.velMagnitude > 0.1f))
				{
					controller.JumpFromWall();
				}
				//codhealth.ClimbEffect(0);
				canClimb = false;
				hitConfig = false;
			}
			else
			{
				canClimb = false;
				hitConfig = false;
			}
		}
		if (controller.grounded)
		{
			adjust = new Vector3(0, 0, 0);
		}
		else
		{
			adjust = new Vector3(0, 1.8f, 0);
		}
	}

	public IEnumerator MoveObject(Vector3 sPos, float time)
	{
		if (!controller.grounded && !(playerweapons.weaponsInUse[playerweapons.currentWeapon] == playerweapons.weaponList[0]))
		{
			climbing = false;
			yield break;
		}
		controller.enabled = false;
		legs.enabled = false;
		shadowLegs.enabled = false;
		wep.enabled = false;
		if (controller.grounded)
		{
			if (!(playerweapons.weaponsInUse[playerweapons.currentWeapon] == playerweapons.weaponList[0]) && (playerweapons.currentWeapon != 2))
			{
				playerweapons.QuickDeselectWeapon();
				yield return new WaitForSeconds(0.4f);
			}
		}
		codhealth.ClimbEffect(1);
		//Rotate player 
		float rotTime = 0f;
		float rotRate = 1f / 0.3f;
		while (rotTime < 1f)
		{
			rotTime = rotTime + (Time.deltaTime * rotRate);
			transform.rotation = Quaternion.Slerp(transform.rotation, endRot, rotTime);
			yield return null;
		}
		//Move player
		Vector3 exitPos = (transform.position + (transform.forward * endPos.z)) + (transform.up * endPos.y);
		float t = 0f;
		float rate = 1f / time;
		while (t < 1f)
		{
			t = t + (Time.deltaTime * rate);
			transform.position = Vector3.Lerp(sPos, exitPos, t);
			if ((t > 0.1f) && !once)
			{
				once = true;
				if (adjust.y > 0f)
				{
					PlaySounds(vaultSound, transform.position, 0.8f);
				}
				else
				{
					if (hitConfig)
					{
						codhealth.ClimbEffect(2);
						PlaySounds(vaultSound, transform.position, 0.8f);
					}
					else
					{
						codhealth.ClimbEffect(1);
						PlaySounds(vaultSound2, transform.position, 1f);
					}
				}
			}
			yield return null;
		}
		once = false;
		climbing = false;
		controller.enabled = true;
		controller.moveDirection = Vector3.zero;
		controller.state = 1;
		//codhealth.ClimbEffect(0);
		yield return new WaitForSeconds(0.3f);
		controller.state = 0;
		if (playerweapons.currentWeapon != 2)
		{
			playerweapons.SelectWeapon(playerweapons.currentWeapon, 1);
		}
		legs.enabled = true;
		shadowLegs.enabled = true;
		wep.enabled = true;
	}

	/*public void OnGUI()
	{
		if ((!climbing && canClimb) && controller.grounded)
		{
			if (hitConfig)
			{
				GUI.Label(new Rect((Screen.width / 2) - 161, (Screen.height / 2) + 201, 350, 100), "<color=black><size=24>Press</size> <b><size=30>Space</size></b> <size=24>to climb \n      over obstacle.</size></color>");
				GUI.Label(new Rect((Screen.width / 2) - 160, (Screen.height / 2) + 200, 350, 100), "<color=orange><size=24>Press</size> <b><size=30><color=green>Space</color></size></b> <size=24>to climb \n      over obstacle.</size></color>");
			}
			else
			{
				GUI.Label(new Rect((Screen.width / 2) - 161, (Screen.height / 2) + 201, 350, 100), "<color=black><size=24>Press</size> <b><size=30>Space</size></b> <size=24>to climb \n      on obstacle.</size></color>");
				GUI.Label(new Rect((Screen.width / 2) - 160, (Screen.height / 2) + 200, 350, 100), "<color=orange><size=24>Press</size> <b><size=30><color=green>Space</color></size></b> <size=24>to climb \n      on obstacle.</size></color>");
			}
		}
	}*/

	public AudioSource PlaySounds(AudioClip clip, Vector3 position, float volume)
	{
		GameObject go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = (AudioSource)go.AddComponent(typeof(AudioSource));
		source.clip = clip;
		source.volume = volume;
		//source.pitch = Random.Range(0.9,1.1);
		source.Play();
		Destroy(go, clip.length);
		return source;
	}
}
