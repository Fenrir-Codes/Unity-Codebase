using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

public class WeaponScriptAnimations : MonoBehaviour {

	public string drawAnim = "";            // animation of taking out weapon.
	public string drawReturnAnim = "";		// putting back weapon (playing draw animation backwards)
	public string fireAnim = "";				
    public string reloadAnim = "";
	public string fireEmptyAnim = "";  	//if you dont have this specific animation, you can use your "Fire" animation. 
	public string idleAnim = "";
	public float drawAnimSpeed = 1.0f;
	public float drawReturnSpeed = 2.0f;
	public float reloadAnimSpeed = 1.0f;
	public AudioClip fireSound;
	public GameObject soundGO;
	public AudioClip drawSound;
	public AudioClip reloadSound;
	public AudioClip outOfAmmo;
	
	//Shotgun Animations
	public string startReload = "StartReload";
	public string insertBullet = "Insert";
	public string endReload = "AfterReload";
	public AudioClip SGinsert;
	public AudioClip SGpump;
	public ScriptWeapon weaponScript;
	
	//Only for Crossbow
	public string idleLoadedAnim = "";

	public AudioClip audioShellEject;

	public void Start()
	{
		//animation.wrapMode = WrapMode.Once;
		if (idleAnim != "")
			GetComponent<Animation>()[idleAnim].wrapMode = WrapMode.Loop;

		if (idleLoadedAnim != "")
			GetComponent<Animation>()[idleLoadedAnim].wrapMode = WrapMode.Loop;

		if (weaponScript.typeOfFireMode == FireModeGun.Shotgun)
			GetComponent<Animation>()[endReload].layer = 5;
	}

	public void Update()
	{
		if (idleAnim != "")
		{
			if (!GetComponent<Animation>().isPlaying)
			{

				if (weaponScript.typeOfFireMode == FireModeGun.Launcher)
				{
					if (weaponScript.numberOfClips == 0)
					{
						GetComponent<Animation>().CrossFadeQueued(idleLoadedAnim, 0.3f, QueueMode.PlayNow);
					}
					else
					{
						GetComponent<Animation>().CrossFadeQueued(idleAnim, 0.3f, QueueMode.PlayNow);
					}
				}
				else
				{

					if (weaponScript.reloading == false) GetComponent<Animation>().CrossFadeQueued(idleAnim, 0.8f, QueueMode.PlayNow);
				}
			}
		}
	}

	public void PlayOutOfAmmo()
	{
		if (!GetComponent<Animation>().IsPlaying(fireAnim))
		{
			if (fireEmptyAnim != "")
			{
				PlayAudioClip(outOfAmmo, transform.position, 1.0f);
				GetComponent<Animation>().Rewind(fireEmptyAnim);
				GetComponent<Animation>().Play(fireEmptyAnim);
			}
		}
	}

	public void PlayAudioEject()
	{
		GetComponent<AudioSource>().PlayOneShot(audioShellEject, 1.0f);
	}

	public void ReturnReloading()
	{
		if (idleAnim != "")
		{

			if (weaponScript.typeOfFireMode == FireModeGun.Launcher)
			{
				if (weaponScript.bulletsLeft == 0)
				{
					GetComponent<Animation>().CrossFadeQueued(idleLoadedAnim, 0.5f, QueueMode.PlayNow);
					//audio.Stop();
					//animation.Play(idleLoadedAnim);
				}
				else
				{
					GetComponent<Animation>().CrossFadeQueued(idleAnim, 0.01f, QueueMode.PlayNow);
					GetComponent<AudioSource>().Stop();
					GetComponent<Animation>().Play(idleAnim);
				}
				soundGO.GetComponent<AudioSource>().Stop();
				GetComponent<AudioSource>().Stop();
			}
			else if (weaponScript.typeOfFireMode == FireModeGun.Shotgun)
			{
				//animation.CrossFadeQueued(idleAnim, 0.9, QueueMode.PlayNow);
				soundGO.GetComponent<AudioSource>().Stop();
				GetComponent<AudioSource>().Stop();
				GetComponent<Animation>().Play(idleAnim);

			}
			else
			{
				GetComponent<Animation>().CrossFade(idleAnim, 0.2f);
				soundGO.GetComponent<AudioSource>().Stop();
				GetComponent<AudioSource>().Stop();
			}
		}
	}

	public void PlayFireAnimAuto()
	{
		if (fireAnim != "")
		{
			GetComponent<Animation>().Rewind(fireAnim);
			GetComponent<Animation>().Play(fireAnim);
			//audio.PlayOneShot(fireSound,1.0);
			PlayAudioClip(fireSound, transform.position, 1.0f);
		}
	}

	public void PlayFireAnimSemi()
	{
		if (fireAnim != "")
		{
			GetComponent<Animation>().CrossFadeQueued(fireAnim, 0.1f, QueueMode.PlayNow);
			//PlayAudioClip(fireSound, transform.position, 0.7);
			GetComponent<AudioSource>().clip = fireSound;
			GetComponent<AudioSource>().volume = 1.0f;
			GetComponent<AudioSource>().Play();
		}
	}

	public void DrawWeapon()
	{
		if (drawAnim != "")
		{
			soundGO.GetComponent<AudioSource>().Stop();
			GetComponent<AudioSource>().clip = drawSound;
			GetComponent<AudioSource>().volume = 0.7f;
			GetComponent<AudioSource>().Play();
			GetComponent<Animation>().Stop(drawAnim);
			GetComponent<Animation>()[drawAnim].speed = drawAnimSpeed;
			GetComponent<Animation>().Play(drawAnim);
		}
	}

	public void PutDownWeapon()
	{
		if (drawReturnAnim != "")
		{
			GetComponent<AudioSource>().clip = drawSound;
			GetComponent<AudioSource>().volume = 0.7f;
			GetComponent<AudioSource>().Play();
			GetComponent<Animation>()[drawReturnAnim].speed = drawReturnSpeed;
			GetComponent<Animation>().Play(drawReturnAnim);
		}
	}

	public void QuickPutDownWeapon()
	{
		if (drawReturnAnim != "")
		{
			GetComponent<Animation>()[drawReturnAnim].speed = drawReturnSpeed * 2;
			GetComponent<Animation>().CrossFade(drawReturnAnim);
		}
	}

	public void ReloadWeapon()
	{
		if (reloadAnim != "")
		{
			soundGO.GetComponent<AudioSource>().clip = reloadSound;
			soundGO.GetComponent<AudioSource>().volume = 1.0f;
			soundGO.GetComponent<AudioSource>().Play();
			GetComponent<Animation>()[reloadAnim].speed = reloadAnimSpeed;
			GetComponent<Animation>().Play(reloadAnim);
		}
	}

	//Shotgun
	public void ReloadStart()
	{
		GetComponent<Animation>()[startReload].time = 0.0f;
		GetComponent<Animation>().Play(startReload);
	}

	public void InsertBullet()
	{
		GetComponent<Animation>()[insertBullet].time = 0.0f;
		//animation.Rewind(insertBullet);
		GetComponent<Animation>().PlayQueued(insertBullet, QueueMode.PlayNow);
		//animation.Play(insertBullet);
		GetComponent<AudioSource>().PlayOneShot(SGinsert, 1.0f);
	}

	public IEnumerator ReloadStop()
	{
		GetComponent<Animation>()[endReload].speed = 2;
		GetComponent<Animation>().Play(endReload);
		yield return new WaitForSeconds(0.2f);//delay, before sound will start play.
		GetComponent<AudioSource>().clip = SGpump;
		GetComponent<AudioSource>().volume = 1.0f;
		GetComponent<AudioSource>().Play();

	}
	public void ResetReloading()
	{
		soundGO.GetComponent<AudioSource>().Stop();
		GetComponent<AudioSource>().Stop();
		GetComponent<Animation>().Stop();
		GetComponent<Animation>()[insertBullet].time = 0.0f;
		GetComponent<Animation>()[startReload].time = 0.0f;
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
