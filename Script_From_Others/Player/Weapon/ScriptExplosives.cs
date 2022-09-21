using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

public class ScriptExplosives : MonoBehaviour {
	public Exposives explType = Exposives.GRENADES; 
	public Rigidbody grenade;
	public GameObject stickyGrenade;
	public Transform spawnPos;
	private float throwForce;
	public float maxThrowForce;
	public AudioClip throwSound;
	public string grenadePull = "GrenadePull";
	public string grenadeThrow = "GrenadeThrow";
	public int grenadeCount = 3; //How many grenades you have
	public GameObject grenadeAnimations;
	public GameObject claymoreAnimations;
	public GameObject stickGrenadeAnim;
	public AudioClip pullSound;
	[HideInInspector]
	public int _event;
	[HideInInspector]
	public bool readyToThrow = false;
	[HideInInspector]
	public bool pull = false;
	[HideInInspector]
	public bool throwing = false;
	public LayerMask layerMask; //make sure we aren't in this layer 
	public GameObject mainCamera;
	public GameObject claymoreGO;
	public GameObject grenadeGO;
	public GameObject stickGrenadeGO;
	public string claymoreDraw = "ClaymoreDraw";
	public string claymorePlant = "ClaymorePlant";
	public GameObject claymore;
	public Transform spawner;
	public Texture grenadeTexture;
	public Texture stickyTexture;
	public Texture claymoreTexture;

	void Start()
	{
		grenadeAnimations.GetComponent<Animation>()[grenadePull].wrapMode = WrapMode.Once;
		grenadeAnimations.GetComponent<Animation>()[grenadeThrow].wrapMode = WrapMode.Once;
		grenadeAnimations.GetComponent<Animation>()[grenadePull].speed = grenadeAnimations.GetComponent<Animation>()[grenadePull].length / .7f;
		grenadeAnimations.GetComponent<Animation>()[grenadeThrow].speed = grenadeAnimations.GetComponent<Animation>()[grenadeThrow].length / .7f;
		stickGrenadeAnim.GetComponent<Animation>()[grenadePull].wrapMode = WrapMode.Once;
		stickGrenadeAnim.GetComponent<Animation>()[grenadeThrow].wrapMode = WrapMode.Once;
		stickGrenadeAnim.GetComponent<Animation>()[grenadePull].speed = stickGrenadeAnim.GetComponent<Animation>()[grenadePull].length / .7f;
		stickGrenadeAnim.GetComponent<Animation>()[grenadeThrow].speed = stickGrenadeAnim.GetComponent<Animation>()[grenadeThrow].length / .7f;
		claymoreAnimations.GetComponent<Animation>()[claymoreDraw].wrapMode = WrapMode.Once;
		claymoreAnimations.GetComponent<Animation>()[claymorePlant].wrapMode = WrapMode.Once;
		claymoreAnimations.GetComponent<Animation>()[claymoreDraw].speed = claymoreAnimations.GetComponent<Animation>()[claymoreDraw].length / 0.8f;
		claymoreAnimations.GetComponent<Animation>()[claymorePlant].speed = claymoreAnimations.GetComponent<Animation>()[claymorePlant].length / 0.5f;
		grenadeAnimations.GetComponent<Animation>().Stop();
		claymoreAnimations.GetComponent<Animation>().Stop();
		var gos = GetComponentsInChildren(typeof(Renderer));
		foreach (Renderer go in gos)
		{
			go.enabled = false;
		}
	}


	public IEnumerator Pull(int status)
	{

		if (explType == Exposives.GRENADES)
		{
			if (pull) yield return false;

			if (status == 1)
			{
				pull = true;

				yield return new WaitForSeconds(0.4f);
				RendererBool(true, grenadeGO);
				GetComponent<AudioSource>().clip = pullSound;
				GetComponent<AudioSource>().volume = 0.7f;
				GetComponent<AudioSource>().Play();
				if (grenadePull != "")
				{
					grenadeAnimations.GetComponent<Animation>()[grenadePull].time = 0.0f;
					grenadeAnimations.GetComponent<Animation>().Play(grenadePull);
				}
				yield return new WaitForSeconds(.4f);
				readyToThrow = true;
				pull = false;
			}
		}
		else if (explType == Exposives.CLAYMORE)
		{
			if (pull) yield return false;

			if (status == 1)
			{
				pull = true;

				yield return new WaitForSeconds(0.6f);
				RendererBool(true, claymoreGO);
				if (grenadePull != "")
				{
					claymoreAnimations.GetComponent<Animation>()[claymoreDraw].time = 0.0f;
					claymoreAnimations.GetComponent<Animation>().Play(claymoreDraw);
				}
				yield return new WaitForSeconds(0.8f);

				readyToThrow = true;
				pull = false;
			}
		}
		else if (explType == Exposives.STICKY)
		{
			if (pull) yield return false;

			if (status == 1)
			{
				pull = true;

				yield return new WaitForSeconds(0.4f);
				RendererBool(true, stickGrenadeGO);
				GetComponent<AudioSource>().clip = pullSound;
				GetComponent<AudioSource>().volume = 0.7f;
				GetComponent<AudioSource>().Play();
				if (grenadePull != "")
				{
					stickGrenadeAnim.GetComponent<Animation>()[grenadePull].time = 0.0f;
					stickGrenadeAnim.GetComponent<Animation>().Play(grenadePull);
				}
				yield return new WaitForSeconds(.4f);

				readyToThrow = true;
				pull = false;
			}
		}
	}



	public IEnumerator Throw(int status)
	{
		if (explType == Exposives.GRENADES)
		{
			if (throwing) yield break;

			if (status == 2)
			{
				throwing = true;
				yield return new WaitForSeconds(0.3f);
				if (grenadeThrow != "")
				{
					grenadeAnimations.GetComponent<Animation>()[grenadeThrow].time = 0.0f;
					grenadeAnimations.GetComponent<Animation>().CrossFade(grenadeThrow);
				}
				GetComponent<AudioSource>().clip = throwSound;
				GetComponent<AudioSource>().volume = 1.0f;
				GetComponent<AudioSource>().Play();
				yield return new WaitForSeconds(0.3f);
				Rigidbody instantGrenade = Instantiate(grenade, spawnPos.position, spawnPos.rotation);

				Vector3 fwd = spawnPos.forward;
				instantGrenade.AddForce(fwd * 30 * throwForce);
				Physics.IgnoreCollision(instantGrenade.GetComponent<Collider>(), transform.root.GetComponent<Collider>());

				grenadeCount--;

				yield return new WaitForSeconds(.3f);
				RendererBool(false, grenadeGO);

				readyToThrow = false;
				throwing = false;
			}
		}
		else if (explType == Exposives.CLAYMORE)
		{
			if (throwing) yield break;
			RaycastHit hit;

			if (status == 2)
			{
				throwing = true;
				yield return new WaitForSeconds(0.3f);
				if (claymorePlant != "")
				{
					claymoreAnimations.GetComponent<Animation>()[claymorePlant].time = 0.0f;
					claymoreAnimations.GetComponent<Animation>().CrossFade(claymorePlant);
				}
				GetComponent<AudioSource>().clip = pullSound;
				GetComponent<AudioSource>().volume = 0.7f;
				GetComponent<AudioSource>().Play();
				yield return new WaitForSeconds(0.6f);
				if (Physics.Raycast(spawner.position, -Vector3.up, out hit, 5))
				{
					GameObject instantClaymore = Instantiate(claymore, spawner.position, spawner.rotation);
					instantClaymore.transform.position = hit.point;
				}
				else
				{
					GameObject instantClaymoreRB = Instantiate(claymore, spawner.position, spawner.rotation);
					yield return new WaitForSeconds(0.3f);
					instantClaymoreRB.AddComponent<Rigidbody>();
				}
				grenadeCount--;
				yield return new WaitForSeconds(.6f);

				RendererBool(false, claymoreGO);

				readyToThrow = false;
				throwing = false;

			}
		}
		else if (explType == Exposives.STICKY)
		{
			if (throwing) yield break;

			if (status == 2)
			{
				throwing = true;
				yield return new WaitForSeconds(0.3f);
				if (grenadeThrow != "")
				{
					stickGrenadeAnim.GetComponent<Animation>()[grenadeThrow].time = 0.0f;
					stickGrenadeAnim.GetComponent<Animation>().CrossFade(grenadeThrow);
				}
				GetComponent<AudioSource>().clip = throwSound;
				GetComponent<AudioSource>().volume = 0.7f;
				GetComponent<AudioSource>().Play();
				yield return new WaitForSeconds(0.3f);
				GameObject instantStGrenade = Instantiate(stickyGrenade, spawnPos.position, spawnPos.rotation);

				grenadeCount--;
				yield return new WaitForSeconds(.3f);

				RendererBool(false, stickGrenadeGO);

				readyToThrow = false;
				throwing = false;
			}
		}
	}

	public void RendererBool(bool status, GameObject _object)
	{
		var gos = _object.GetComponentsInChildren(typeof(Renderer));
		foreach (Renderer go in gos)
		{
			go.enabled = status;
		}
	}

	public void Update()
	{
		if (throwing)
		{
			float limitForce = (mainCamera.transform.forward.y > -0.6f) ? 1 : -1;
			if (limitForce == 1)
			{
				throwForce = maxThrowForce;
			}
			else
			{
				throwForce = maxThrowForce / 3;
			}
		}
	}

	/*public void OnGUI()
	{
		if (grenadeCount > 0)
		{
			if (explType == Exposives.STICKY)
			{
				if (grenadeCount >= 3) GUI.DrawTexture(new Rect(Screen.width - 260, Screen.height - 65, 50, 50), stickyTexture);
				if (grenadeCount >= 2) GUI.DrawTexture(new Rect(Screen.width - 240, Screen.height - 65, 50, 50), stickyTexture);
				if (grenadeCount >= 1) GUI.DrawTexture(new Rect(Screen.width - 220, Screen.height - 65, 50, 50), stickyTexture);
			}
			else if (explType == Exposives.CLAYMORE)
			{
				if (grenadeCount >= 1) GUI.DrawTexture(new Rect(Screen.width - 270, Screen.height - 95, 100, 100), claymoreTexture);
				if (grenadeCount >= 2) GUI.DrawTexture(new Rect(Screen.width - 300, Screen.height - 95, 100, 100), claymoreTexture);
				if (grenadeCount >= 3) GUI.DrawTexture(new Rect(Screen.width - 330, Screen.height - 95, 100, 100), claymoreTexture);
			}
			else if (explType == Exposives.GRENADES)
			{
				if (grenadeCount >= 3) GUI.DrawTexture(new Rect(Screen.width - 260, Screen.height - 65, 55, 55), grenadeTexture);
				if (grenadeCount >= 2) GUI.DrawTexture(new Rect(Screen.width - 240, Screen.height - 65, 55, 55), grenadeTexture);
				if (grenadeCount >= 1) GUI.DrawTexture(new Rect(Screen.width - 220, Screen.height - 65, 55, 55), grenadeTexture);

			}
		}
	}*/
}
