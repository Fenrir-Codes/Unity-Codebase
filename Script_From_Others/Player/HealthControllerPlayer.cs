using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

[ExecuteInEditMode()]
public class HealthControllerPlayer : MonoBehaviour {

	public float maximumHitPoints = 200.0f;
	public float hitPoints;
	public float regenerationSpeed = 0f;
	public int protectFromDamage = 0;
	public GameObject deadReplacement;
	private float time = 0.0f;
	[HideInInspector]
	public float alpha;
	public GUISkin mySkin;
	public GameObject cameraGO;
	public Texture damageTexture;
	public GameObject playsoundsGO;
	public AudioClip dieSound;
	public AudioClip legsHitGround;
	public AudioClip bodyHitGround;
	public AudioClip fallDamage;
	public AudioClip deadFallDamage;
	public AudioClip[] hitSound;
	//DAMGE EFFECT
	public float returnSpeed = 2.0f;
	public Transform kickGO;
	public Transform kickGO2;
	public float kbamount = 10.0f;
	public float minOffset = -6.0f; 
	public float maxoffset = 6.0f;
	public float kbtime = 0.3f;
	public GameObject kickGO3;
	//DAMGE EFFECT
	public FootStepsController footStepScript;

	public void Start()
	{
		alpha = 0;
	}

	public void ApplyDamage(int damage)
	{
		if (hitPoints <= 0f)
		{
			return;
		}
		// Apply damage
		if (damage > protectFromDamage)
		{
			hitPoints = hitPoints - (damage - protectFromDamage);
		}
		else
		{
			return;
		}
		StartCoroutine(HitCameraEffect(kickGO2, new Vector3(Random.Range(-7, 10), Random.Range(-4, 4), 0), kbtime));
		PlayAudioClip(hitSound[Random.Range(0, hitSound.Length)], transform.position, 1f);
		if (!playsoundsGO.GetComponent<AudioSource>().isPlaying)
		{
			playsoundsGO.GetComponent<AudioSource>().clip = hitSound[Random.Range(0, hitSound.Length)];
			playsoundsGO.GetComponent<AudioSource>().volume = 0.4f;
			playsoundsGO.GetComponent<AudioSource>().Play();
		}
		time = 2f;
		// Are we dead?
		if (hitPoints <= 0f)
		{
			Die();
		}
	}

	public void PlayerFallDamage(int damage)
	{
		if (damage > hitPoints)
		{
			AudioSource.PlayClipAtPoint(deadFallDamage, transform.position);
		}
		else
		{
			GetComponent<AudioSource>().PlayOneShot(fallDamage, 1f);
		}
		hitPoints = hitPoints - damage;
		if (hitPoints < 0f)
		{
			Die();
			return;
		}
		StartCoroutine(FallCameraEffect(kickGO, new Vector3(kbamount, Random.Range(minOffset, maxoffset), 0), kbtime));
		StartCoroutine(FallWeaponEffect(kickGO2, new Vector3(kbamount, Random.Range(minOffset, maxoffset), 0), kbtime));
		time = 4f;
	}

	public void Grounded(int note)
	{
		StartCoroutine(FallCameraEffect(kickGO, new Vector3(kbamount / 3, Random.Range(minOffset / 2, maxoffset / 2), 0), kbtime / 0.3f));
		StartCoroutine(FallWeaponEffect(kickGO2, new Vector3(kbamount / 3, Random.Range(minOffset / 2, maxoffset / 2), 0), kbtime / 0.5f));
		if (note == 2)
		{
			GetComponent<AudioSource>().PlayOneShot(bodyHitGround, 0.8f);
		}
		else
		{
			PlayAudioClip(legsHitGround, transform.position, 0.1f);
			//GetComponent<AudioSource>().PlayOneShot(legsHitGround, 0.1);
			StartCoroutine(footStepScript.JumpLand());
		}
	}

	public void Die()
	{
		if (deadReplacement)
		{
			if (!GetComponent<AudioSource>().isPlaying)
			{
				AudioSource.PlayClipAtPoint(dieSound, transform.position);
			}
			// Create the dead body
			GameObject dead = UnityEngine.Object.Instantiate(deadReplacement, transform.position, transform.rotation);
			Vector3 vel = Vector3.zero;
			if (GetComponent<Rigidbody>())
			{
				vel = GetComponent<Rigidbody>().velocity;
			}
			else
			{
				CharacterController cc = (CharacterController)GetComponent(typeof(CharacterController));
				vel = cc.velocity;
			}
			CopyTransformsRecurse(transform, dead.transform, vel);
			gameObject.SetActive(false);
		}
	}

	public void CopyTransformsRecurse(Transform src, Transform dst, Vector3 velocity)
	{
		Rigidbody body = dst.GetComponent<Rigidbody>();
		if (body != null)
		{
			body.velocity = velocity;
			body.useGravity = true;
		}
		dst.position = src.position;
		dst.rotation = src.rotation;
		foreach (Transform child in dst)
		{
			Transform curSrc = src.Find(child.name);
			if (curSrc)
			{
				CopyTransformsRecurse(curSrc, child, velocity);
			}
		}
	}

	public void Update()
	{
		kickGO.localRotation = Quaternion.Slerp(kickGO.localRotation, Quaternion.identity, (Time.deltaTime * returnSpeed) / 2);
		kickGO2.localRotation = Quaternion.Slerp(kickGO2.localRotation, Quaternion.identity, (Time.deltaTime * returnSpeed) / 4);
		//REGENERATION
		if ((hitPoints < maximumHitPoints) && (hitPoints > 0f))
		{
			hitPoints = hitPoints + (Time.deltaTime * regenerationSpeed);
		}
		if (hitPoints > maximumHitPoints)
		{
			hitPoints = maximumHitPoints;
		}
		if (time > 0)
		{
			time = time - Time.deltaTime;
		}
		alpha = time;
	}

	public void ClimbEffect(int effect)
	{
		if (effect == 1)
		{
			StartCoroutine(FallCameraEffect(kickGO, new Vector3(40, Random.Range(minOffset, maxoffset), 0), 0.3f));
			StartCoroutine(FallWeaponEffect(kickGO2, new Vector3(40, Random.Range(minOffset, maxoffset), 0), 0.3f));
		}
		else
		{
			if (effect == 2)
			{
				StartCoroutine(FallCameraEffect(kickGO, new Vector3(0, 0, 40), 0.6f));
				StartCoroutine(FallWeaponEffect(kickGO2, new Vector3(0, 0, 40), 0.6f));
			}
			else
			{
				StartCoroutine(FallCameraEffect(kickGO, new Vector3(15, Random.Range(-5, 5), 10), 0.5f));
				StartCoroutine(FallWeaponEffect(kickGO2, new Vector3(15, Random.Range(-5, 5), 10), 0.5f));
			}
		}
	}

	public IEnumerator FallWeaponEffect(Transform goTransform, Vector3 kbDirection, float time)
	{
		Quaternion startRotation = goTransform.localRotation;
		Quaternion endRotation = goTransform.localRotation * Quaternion.Euler(kbDirection);
		float rate = 1f / time;
		float t = 0f;
		while (t < 1f)
		{
			t = t + (Time.deltaTime * rate);
			goTransform.localRotation = Quaternion.Slerp(startRotation, endRotation, t);
			yield return null;
		}
	}

	public IEnumerator FallCameraEffect(Transform goTransform, Vector3 kbDirection, float time)
	{
		Quaternion startRotation2 = goTransform.localRotation;
		Quaternion endRotation2 = goTransform.localRotation * Quaternion.Euler(kbDirection);
		float rate = 1f / time;
		float t = 0f;
		while (t < 1f)
		{
			t = t + (Time.deltaTime * rate);
			goTransform.localRotation = Quaternion.Slerp(startRotation2, endRotation2, t);
			yield return null;
		}
	}

	public IEnumerator HitCameraEffect(Transform goTransform, Vector3 kbDirection, float time)
	{
		Quaternion startRotation3 = goTransform.localRotation;
		Quaternion endRotation3 = goTransform.localRotation * Quaternion.Euler(kbDirection);
		float rate = 1f / time;
		float t = 0f;
		while (t < 1f)
		{
			t = t + (Time.deltaTime * rate);
			goTransform.localRotation = Quaternion.Slerp(startRotation3, endRotation3, t);
			yield return null;
		}
	}

	public IEnumerator Shake(float shakeTime)
	{
		float shakePower = 0.0f;
		float rate = 1f / shakeTime;
		float ta = 1f;
		while (ta > 0f)
		{
			ta = ta - (Time.deltaTime * rate);
			shakePower = (ta / 50) * Time.timeScale;
			var kickGO3Rot = kickGO3.transform.rotation;
			if (shakePower > 0f)
			{
				kickGO3Rot.x = kickGO3.transform.rotation.x + Random.Range(-shakePower, shakePower);
				kickGO3Rot.y = kickGO3.transform.rotation.y + Random.Range(-shakePower, shakePower);
				kickGO3Rot.z = kickGO3.transform.rotation.z + Random.Range(-shakePower, shakePower);
				kickGO3.transform.rotation = kickGO3Rot;
			}
			yield return null;
		}
	}

	/*public void OnGUI()
	{
		GUI.skin = mySkin;
		//GUIStyle style1 = mySkin.customStyles[0];
		GUIStyle style2 = mySkin.customStyles[3];
		GUI.Label(new Rect(75, Screen.height - 75, 100, 60), "+ " + hitPoints.ToString("F0"), style2);
		GUI.color = new Color(1f, 1f, 1f, alpha); //Color (r,g,b,a)
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), damageTexture);
	}*/

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
	{
		GameObject go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = (AudioSource)go.AddComponent(typeof(AudioSource));
		source.clip = clip;
		source.volume = volume;
		source.pitch = Time.timeScale;
		source.Play();
		UnityEngine.Object.Destroy(go, clip.length);
		return source;
	}
}
