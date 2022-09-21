using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

public class RunBreathNew : MonoBehaviour {

	public float vol;
	public CharacterController controller; 
	public PlayerController codCon;

	void Update ()
	{
		if (controller.velocity.magnitude > 8 && codCon.grounded)
		{
			vol = Mathf.Lerp(vol, 0.075f, Time.deltaTime / 5);
		}
		else
		{
			vol = Mathf.Lerp(vol, 0.0f, Time.deltaTime / 2);
		}
		GetComponent<AudioSource>().volume = vol;
	}
}
