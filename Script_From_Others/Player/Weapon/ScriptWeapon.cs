using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScreenLocker;

public enum FireModeGun { NoWeapon, Semi, Auto, BurstSemi, BurstAuto, Shotgun, Launcher }
public enum ZoomingGun { None, Simple, Scope }
public enum AmmoTypeGun { byClip, byBullet, Shotgun }
public enum KickBackTypeGun { None, KB_Auto, KB_Semi, KB_ADW }
public enum BulletTypeGun { simpleBullet, advancedBullet, Arrow, Rocket }

public class ScriptWeapon : MonoBehaviour {

	public FireModeGun typeOfFireMode = FireModeGun.Semi;
	
	// SEMI - use for single shots (like pistols, snipers....)
	// AUTO - use for mashinegun 
	// BURST SEMI - if you press button it will shoot in burst.
	// BURST AUTO - if you hold down the button it will shoot in burst. (just change your fire rate to higher then 0.1)
	// SHOTGUN - shots pellets in semi mode. By default count of pellets are 10, but you can change it to higher.
	// LAUNCHER - use for rockets, or arrows & etc.

	public ZoomingGun Zoom = ZoomingGun.None;
	
	public AmmoTypeGun typeOfAmmo = AmmoTypeGun.byBullet;
	
	public KickBackTypeGun typeOfKickBack = KickBackTypeGun.None;
	
	public bool canFire = true;
	public bool autoReload = true;                      //if true, weapon will automatically reloaded (if bullets in clip = 0 and you press "Fire" button).
	public bool aimDownSights = false;

	public bool muzzleFlashEnabled = false;
	public GameObject muzzleFlash;
    public Renderer muzzleFlashRenderer;

	public bool muzzleLightEnabled = false;
	public Light muzzleLight;

	//Smoke effect, when weapon shoots.
	public bool smokeEnabled = false;
	public GameObject smokePosition;
	public GameObject smoke;

	//Shell 
	public bool animatedShell = false;
	public bool shellsEnabled = false;
	public Transform shellEjectPosition;                //position, where shell will be instantiated.
	public Transform shellAnim;
	public Rigidbody shellRigid;                            //shell prefab (must have Rigidbody)
	public float timeBeforeEjection = 0.0f;
	public bool playAudioEffect;
	public GameObject mainCamera;                       //object which holds MainCamera
	public GameObject weaponCamera;                     //object which holds weaponCamera
	public GameObject player;                           //root object of your player (where you have "MouseLook" attached)

	//ANIMATION OF YOUR WEAPONS
	public GameObject weaponAnim;                       // drag and drop to this slot >>>"GameObject" on which are all your weapon animations.
	public float drawAnimTime = 0.8f;                       // how long you will not be able to shoot, when you take out your weapon  
	public bool enableOutOfAmmoAnim = false;  		//if true, it will enable "fireEmptyAnim" animation.
	private bool inRun = false;

	//SHOTGUN
	public int SGpelletsPerShot = 10;                   //how many pellets will be shot from shotgun

	//BURST
	public int shotsPerBurst = 3;                     	//how many bullets will be shooted (per round).   
	public float burstTime = 0.1f;                      	//time between every bullet in burst  
    private bool bursting = false;
 
	public float maxPenetration = 6.0f;
	public float damage = 10.0f;                        //how much damage enemy will receive
	public float bulletSpeed = 100.0f;
	public float fireRate = 0.1f; 						//how fast you'll shoot your bullets
	//var bulletRPM = 500;						//find proper RPM for every weapon in google.
	public int force = 50;                               //force, which will be applied to object if you hit it with bullet
	public int bulletsPerClip = 30;                         //how many bullets in your clip
	public int numberOfClips = 3;                       //how many clips you have
	public int maxNumberOfClips = 6;                    //max number of clips you can have.
	public float reloadTime = 3.0f;                     	//how fast your weapon will be reloaded (adjust it with your animation)
	public int bulletsLeft;                
	public bool gunActive;
	[HideInInspector]
	public bool reloading = false;       
    private int m_LastFrameShot = -1;
	private float nextFireTime = 0.0f;
    private bool playing = false;

	// KICKBACK
	public float kickBackAmountY = 0.5f;
	public float kickBackAmountX = 0.5f;
	//Second KICKBACK
	public float kickAmount = 3.0f;
	public float kickOffset = 1.0f;
	//Third KICKBACK
	public float returnSpeed  = 2.0f;
	public Transform kickGO;
	public float kbamount = 10.0f;
	public float minOffset = -6.0f;
	public float maxoffset = 6.0f;
	public float kbtime = 0.1f;

	// ACURACY OF WEAPON
	public float baseSpread = 0.01f;               				
	public float hipMaxSpread = 0.2f;
	public float aimMaxSpread = 0.05f;
	private float spread = 0.0f;           	 //current accuracy
	public float spreadIncrease = 0.3f;           	 //how fast weapon will lose accuracy 
    public float spreadDecrease = 1.0f;        		 //how fast will return accuracy
	
	//AIMING with Scope
	public int scopeFOV = 10;                     	// second zoom position
	private bool show = true;               //Show Scope Texture
	public Sprite scopeTexture;
	public Sprite scopeBackgroundText;
	public float zoomInSpeed = 0.1f;					//How fast FOV will decrease
	public float zoomOutSpeed = 0.1f;					//How fast FOV will increase
	public bool scoped = false;
	public bool aiming = false;

	//AIM Down Sights
	public Vector3 hipPosition;
	public Vector3 aimPosition;
	public float aimInSpeed = 0.12f;					//speed of moving weapon to "aim" position
	public float aimOutSpeed = 0.2f;					//speed of moving weapon back to "hip" position
	public float quickReloadTime = 1.3f;				//how long you will not be able to aim, after every shot. (im using it for quick reload animation)  
	private Vector3 curVect;
	public bool canAim = true;
	public bool aimed = false;

	//LASER
	public bool laserEnabled = false;
	public Transform laserGO;
	public Transform laserHitPoint;

	//Crosshair
	public bool enableCrosshair = false;            //show or hide crosshair.
	public bool updateCrosshair = false;            //if you wanna dynamic crosshair
	public float multiplier = 3.5f; 					//depending on bullet speed, you must adjust multiplier.
	public int crosshairType = 0;
	
	// Scripts
	public PlayerController codcontroller;
	public WeaponScriptAnimations weaponAnimationScript;
	public ScriptExplosives explosivesScript;
	public DynamicCrosshairNew crosshair;
	public WeaponsPlayer pw;
	
	
	public GameObject bulletPrefab;
	public Transform bulletPosition;
	public float bulletGravity = 1.0f;
	
	
	public float scopeTexTime;
	public bool knifeOrGrenade = false;
	public float charMoves;
	
	
	public BulletTypeGun bullet = BulletTypeGun.simpleBullet;

	public float StartPenetrationRange;
	public float EndPenetrationRange;

	void Start()
	{
		//fireRate = 60/bulletRPM;
		player = transform.root.gameObject;
		bulletsLeft = bulletsPerClip;
		if (muzzleFlashEnabled)
		{
			muzzleFlashRenderer.enabled = false;
		}
		if (muzzleLightEnabled)
		{
			muzzleLight.enabled = false;
		}
		if (typeOfAmmo == AmmoTypeGun.byBullet)
		{
			numberOfClips = numberOfClips * bulletsPerClip;
		}
		//rateOfFireTimer = 0f;
	}

	void Update()
	{
		if (!ScreenLock.lockCursor) return;
		if (gunActive)
		{
			if (Zoom == ZoomingGun.Scope)
			{
				if ((scopeTexture != null) && show)
					pw.HUD_MGR.ScopeRoot.gameObject.SetActive(true);
				else
					pw.HUD_MGR.ScopeRoot.gameObject.SetActive(false);
			}

			//Update bools
			if (knifeOrGrenade)
			{
				codcontroller.canRun = false;
				codcontroller.run = false;
				canFire = false;
				canAim = false;
				pw.canSwitch = false;
			}
			else
			{
				if (reloading)
				{
					canFire = false;
					canAim = false;
					pw.canSwitch = false;
				}
				else
				{
					if (inRun)
					{
						canFire = false;
						canAim = false;
						pw.canSwitch = false;
					}
					else
					{
						if (aiming)
						{
							pw.canSwitch = false;
						}
						else
						{
							canFire = true;
							canAim = true;
							pw.canSwitch = true;
							codcontroller.canRun = true;
						}
					}
				}
			}
			if (laserEnabled)
			{
				Laser();
			}
			//update ammo
			pw.bullets = bulletsLeft;
			pw.mags = numberOfClips;
			//update crosshair
			if (enableCrosshair)
			{
				if (updateCrosshair)
				{
					if (codcontroller.controller.velocity.magnitude > 3)
					{
						if (charMoves < (hipMaxSpread + 40))
						{
							charMoves = charMoves + (Time.deltaTime * 200);
						}
					}
					else
					{
						if (charMoves > (baseSpread + 10))
						{
							charMoves = charMoves - (Time.deltaTime * 200);
						}
					}
					if (!aiming)
					{
						crosshair.spread = (spread * multiplier) + charMoves;
					}
				}
			}
			if (autoReload)
			{
				if (((bulletsLeft <= 0) && !reloading) && codcontroller.grounded)
				{
					StartCoroutine("Reload");
				}
				else
				{
					if (Input.GetKeyDown("r"))
					{
						StartCoroutine("Reload");
					}
				}
			}
			else
			{
				if (Input.GetKeyDown("r"))
				{
					StartCoroutine("Reload");
				}
			}
			if (!aiming && !knifeOrGrenade)
			{
				if (Input.GetKeyDown("q"))
				{
					StartCoroutine(FastKnife());
					if (reloading)
					{
						StopCoroutine("Reload");
					}
				}
				if (Input.GetKeyDown("g") && (explosivesScript.explType != Exposives.NONE))
				{
					if (((explosivesScript.pull == false) && (explosivesScript.readyToThrow == false)) && (explosivesScript.grenadeCount > 0))
					{
						StartCoroutine(EventGrenadeThrow(1));
					}
				}
			}
			if (explosivesScript.readyToThrow == true)
			{
				if (Input.GetKeyUp("g"))
				{
					StartCoroutine(EventGrenadeThrow(2));
				}
				else
				{
					if (!Input.GetKey("g"))
					{
						StartCoroutine(EventGrenadeThrow(2));
					}
				}
			}
			if (Input.GetKeyDown("h"))
			{
				FireMode();
			}
			/*if (typeOfFireMode == fireModeGun.Semi)
			{
				if ((Input.GetButtonDown("FireMode") && (bulletsLeft == 0)) || ((Input.GetButtonDown("FireMode") && autoReload) && (numberOfClips == 0)))
				{
					StartCoroutine(OutOfAmmo());
				}
			}
			if (typeOfFireMode == fireModeGun.Auto)
			{
				if ((Input.GetButton("FireMode") && (bulletsLeft == 0)) || ((Input.GetButton("FireMode") && autoReload) && (numberOfClips == 0)))
				{
					StartCoroutine(OutOfAmmo());
				}
			}*/
			if (codcontroller.ladderState == 1)
			{
				if (reloading)
				{
					StopCoroutine("Reload");
					weaponAnimationScript.ReturnReloading();
					StartCoroutine(EnableFunctionality(2));
				}
				StartCoroutine(DisableFunctionality(1, 0f));
				codcontroller.ladderState = 3;
			}
			if (codcontroller.ladderState == 2)
			{
				StartCoroutine(EnableFunctionality(1));
				codcontroller.ladderState = 0;
			}
			if (canFire)
			{
				if (typeOfFireMode == FireModeGun.Semi)
				{
					if (Input.GetButtonDown("Fire1"))
					{
						SemiFire();
					}
				}
				if (typeOfFireMode == FireModeGun.Auto)
				{
					if (Input.GetButton("Fire1"))
					{
						AutoFire();
					}
				}
				if (typeOfFireMode == FireModeGun.Shotgun)
				{
					if (Input.GetButtonDown("Fire1"))
					{
						ShotgunFire();
					}
				}
				if (typeOfFireMode == FireModeGun.BurstSemi)
				{
					if (Input.GetButtonDown("Fire1"))
					{
						StartCoroutine(BurstFire());
					}
				}
				if (typeOfFireMode == FireModeGun.BurstAuto)
				{
					if (Input.GetButton("Fire1"))
					{
						StartCoroutine(BurstFire());
					}
				}
				if (typeOfFireMode == FireModeGun.Launcher)
				{
					if (Input.GetButtonDown("Fire1"))
					{
						SemiFire();
					}
				}
			}
			if (Zoom == ZoomingGun.Scope)
			{
				if (show)
				{
					mainCamera.GetComponent<Camera>().fieldOfView = mainCamera.GetComponent<Camera>().fieldOfView - ((scopeFOV * Time.deltaTime) / zoomInSpeed);
					weaponCamera.GetComponent<Camera>().fieldOfView = weaponCamera.GetComponent<Camera>().fieldOfView - ((scopeFOV * Time.deltaTime) / zoomInSpeed);
					if (mainCamera.GetComponent<Camera>().fieldOfView < scopeFOV)
					{
						mainCamera.GetComponent<Camera>().fieldOfView = scopeFOV;
					}
					if (weaponCamera.GetComponent<Camera>().fieldOfView < scopeFOV)
					{
						weaponCamera.GetComponent<Camera>().fieldOfView = scopeFOV;
					}
				}
				else
				{
					mainCamera.GetComponent<Camera>().fieldOfView = mainCamera.GetComponent<Camera>().fieldOfView + ((60f * Time.deltaTime) / zoomOutSpeed);
					weaponCamera.GetComponent<Camera>().fieldOfView = weaponCamera.GetComponent<Camera>().fieldOfView + ((45f * Time.deltaTime) / zoomOutSpeed);
					if (mainCamera.GetComponent<Camera>().fieldOfView > 60f)
					{
						mainCamera.GetComponent<Camera>().fieldOfView = 60f;
					}
					if (weaponCamera.GetComponent<Camera>().fieldOfView > 45f)
					{
						weaponCamera.GetComponent<Camera>().fieldOfView = 45f;
					}
				}
			}
			if (Zoom == ZoomingGun.Simple)
			{
				if (aiming)
				{
					mainCamera.GetComponent<Camera>().fieldOfView = mainCamera.GetComponent<Camera>().fieldOfView - ((scopeFOV * Time.deltaTime) / zoomInSpeed);
					weaponCamera.GetComponent<Camera>().fieldOfView = weaponCamera.GetComponent<Camera>().fieldOfView - ((scopeFOV * Time.deltaTime) / zoomInSpeed);
					if (mainCamera.GetComponent<Camera>().fieldOfView < scopeFOV)
					{
						mainCamera.GetComponent<Camera>().fieldOfView = scopeFOV;
					}
					if (weaponCamera.GetComponent<Camera>().fieldOfView < scopeFOV)
					{
						weaponCamera.GetComponent<Camera>().fieldOfView = scopeFOV;
					}
				}
				else
				{
					mainCamera.GetComponent<Camera>().fieldOfView = mainCamera.GetComponent<Camera>().fieldOfView + ((60f * Time.deltaTime) / zoomOutSpeed);
					weaponCamera.GetComponent<Camera>().fieldOfView = weaponCamera.GetComponent<Camera>().fieldOfView + ((45f * Time.deltaTime) / zoomOutSpeed);
					if (mainCamera.GetComponent<Camera>().fieldOfView > 60f)
					{
						mainCamera.GetComponent<Camera>().fieldOfView = 60f;
					}
					if (weaponCamera.GetComponent<Camera>().fieldOfView > 45f)
					{
						weaponCamera.GetComponent<Camera>().fieldOfView = 45f;
					}
				}
			}
			kickGO.localRotation = Quaternion.Slerp(kickGO.localRotation, Quaternion.identity, Time.deltaTime * returnSpeed);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.identity, Time.deltaTime * 3);
			if (aimDownSights)
			{
				if (Input.GetButton("Fire2") && canAim)
				{
					if (!aiming)
					{
						aiming = true;
						curVect = aimPosition - transform.localPosition;
						scopeTexTime = Time.time + aimInSpeed;
					}
					if (((transform.localPosition != aimPosition) && aiming) && canAim)
					{
						if (Mathf.Abs(Vector3.Distance(transform.localPosition, aimPosition)) < ((curVect.magnitude / aimInSpeed) * Time.deltaTime))
						{
							transform.localPosition = aimPosition;
						}
						else
						{
							transform.localPosition = transform.localPosition + ((curVect / aimInSpeed) * Time.deltaTime);
						}
					}
					if (((Time.time >= scopeTexTime) && !show) && (Zoom == ZoomingGun.Scope))
					{
						RendererBool(false);
						show = true;
					}
					crosshair.showCrosshair = false;
				}
				else
				{
					if (aiming)
					{
						aiming = false;
						curVect = hipPosition - transform.localPosition;
						RendererBool(true);
						if (Zoom == ZoomingGun.Scope)
						{
							show = false;
						}
					}
					if (Mathf.Abs(Vector3.Distance(transform.localPosition, hipPosition)) < ((curVect.magnitude / aimOutSpeed) * Time.deltaTime))
					{
						transform.localPosition = hipPosition;
					}
					else
					{
						transform.localPosition = transform.localPosition + ((curVect / aimOutSpeed) * Time.deltaTime);
					}
					if (enableCrosshair)
					{
						crosshair.showCrosshair = true;
					}
				}
			}
		}
	}

	void LateUpdate()
	{
		if (m_LastFrameShot == Time.frameCount)
		{
			if (muzzleFlashEnabled)
			{
				muzzleFlash.transform.localRotation = Quaternion.AngleAxis(Random.Range(-10, 10), Vector3.forward);
				muzzleFlashRenderer.enabled = true;
			}
			if (muzzleLightEnabled)
			{
				muzzleLight.enabled = true;
			}
			if (smokeEnabled)
			{
				EmitSmoke();
			}
		}
		else
		{
			if (muzzleFlashEnabled)
			{
				muzzleFlashRenderer.enabled = false;
			}
			if (muzzleLightEnabled)
			{
				muzzleLight.enabled = false;
			}
		}
		if (aiming)
		{
			spread = Mathf.Clamp(spread, 0.01f, aimMaxSpread);
			if (spread > 0.01f)
			{
				spread = spread - (spreadDecrease * Time.deltaTime);
			}
		}
		else
		{
			spread = Mathf.Clamp(spread, baseSpread, hipMaxSpread);
			if (spread > baseSpread)
			{
				spread = spread - (spreadDecrease * Time.deltaTime);
			}
		}
	}

	/*void OnGUI()
	{
		//int adjustSize = 0;
		//int adjustSize2 = 0;
		if (gunActive)
		{
			if (Zoom == ZoomingGun.Scope)
			{
				if ((scopeTexture != null) && show)
				{
					if (Screen.height > 900)
					{
						adjustSize = 600;
						adjustSize2 = 1000;
					}
					else
					{
						adjustSize = 220;
						adjustSize2 = 300;
					}
					//GUI.DrawTexture(new Rect(((Screen.width - scopeTexture.width) / 2) - (adjustSize / 2), ((Screen.height - scopeTexture.height) / 2) - (adjustSize / 2), scopeTexture.width + adjustSize, scopeTexture.height + adjustSize), scopeTexture);
					//GUI.DrawTexture(new Rect(((Screen.width - scopeBackgroundText.width) / 2) - (adjustSize2 / 2), ((Screen.height - scopeBackgroundText.height) / 2) - (adjustSize2 / 2), scopeBackgroundText.width + adjustSize2, scopeBackgroundText.height + adjustSize2), scopeBackgroundText);
				}
			}
		}
	}*/

	public IEnumerator FastKnife()
	{
		RendererBool(false); //Hide weapon
		StartCoroutine(DisableFunctionality(1, 0.3f));
		mainCamera.SendMessage("KnifeAttack");
		yield return new WaitForSeconds(0.4f);
		StartCoroutine(EnableFunctionality(1));
	}

	public IEnumerator EventGrenadeThrow(int status)
	{
		if (status == 1) //Prepare to throw
		{
			if (reloading)
			{
				StopCoroutine("Reload");
				weaponAnimationScript.ReturnReloading();
				StartCoroutine(EnableFunctionality(2));
			}
			StartCoroutine(DisableFunctionality(1, 0.3f));
			StartCoroutine(explosivesScript.Pull(status));
		}
		if (status == 2) //Throw Grenade
		{
			if (explosivesScript.throwing == true)
			{
				yield break;
			}
			StartCoroutine(explosivesScript.Throw(status));
			yield return new WaitForSeconds(1f);
			StartCoroutine(EnableFunctionality(1));
		}
	}

	public IEnumerator DisableFunctionality(int reason, float waitTime)
	{
		if (reason == 1) //Throwing grenade, or using knife.
		{
			knifeOrGrenade = true;
			//readyToThrow = true;
			mainCamera.GetComponent<Camera>().fieldOfView = 60;
			weaponCamera.GetComponent<Camera>().fieldOfView = 45;
			weaponAnimationScript.QuickPutDownWeapon();
			yield return new WaitForSeconds(waitTime);
			weaponAnimationScript.ReturnReloading();
			RendererBool(false);
		}
		if (reason == 2) //Reloading weapon
		{
			show = false;
		}
	}

	public IEnumerator EnableFunctionality(int reason)
	{
		if (codcontroller.onLadder)
		{
			yield break;
		}
		if (reason == 1) //Returning from grenade, or knife attack.
		{
			RendererBool(true);
			weaponAnimationScript.DrawWeapon();
			show = false;
			reloading = false;
			yield return new WaitForSeconds(drawAnimTime);
			knifeOrGrenade = false;
		}
		if (reason == 2) //Returning from reload
		{
			reloading = false;
		}
	}

	void SemiFire()
	{
		if ((bulletsLeft <= 0) || reloading)
		{
			if (autoReload)
			{
				StartCoroutine("Reload");
			}
			StartCoroutine(OutOfAmmo());
			return;
		}
		if (nextFireTime < Time.time)
		{
			FireOneShot();
			Kick();
			nextFireTime = Time.time + fireRate;
			if (shellsEnabled)
			{
				StartCoroutine(EjectShell());
			}
		}
	}

	void AutoFire()
	{
		if ((bulletsLeft <= 0) || reloading)
		{
			if (autoReload)
			{
				StartCoroutine("Reload");
			}
			StartCoroutine(OutOfAmmo());
			return;
		}
		if (nextFireTime < Time.time)
		{
			FireOneShot();
			Kick();
			nextFireTime = Time.time + fireRate;
			if (shellsEnabled)
			{
				StartCoroutine(EjectShell());
			}
		}
	}

	public IEnumerator BurstFire()
	{
		int shotCounter = 0;
		if (bursting)
		{
			yield break;
		}
		if ((bulletsLeft <= 0) || reloading)
		{
			if (autoReload)
			{
				StartCoroutine("Reload");
			}
			StartCoroutine(OutOfAmmo());
			yield break;
		}
		if (Time.time > nextFireTime)
		{
			while (shotCounter < shotsPerBurst)
			{
				bursting = true;
				shotCounter++;
				if (bulletsLeft > 0)
				{
					FireOneShot();
				}
				Kick();
				if (shellsEnabled)
				{
					StartCoroutine(EjectShell());
				}
				yield return new WaitForSeconds(burstTime);
			}
			nextFireTime = Time.time + fireRate;
		}
		bursting = false;
	}

	void ShotgunFire()
	{
		int sgb = 0;
		if ((bulletsLeft <= 0) || reloading)
		{
			if (autoReload)
			{
				StartCoroutine("Reload");
			}
			StartCoroutine(OutOfAmmo());
			return;
		}
		if (Time.time > nextFireTime)
		{
			while (sgb < SGpelletsPerShot)
			{
				FireOneShot();
				sgb++;
			}
			nextFireTime = Time.time + fireRate;
			Kick();
			bulletsLeft--;
			if (shellsEnabled)
			{
				StartCoroutine(EjectShell());
			}
		}
	}

	public IEnumerator EjectShell()
	{
		yield return new WaitForSeconds(timeBeforeEjection);
		if (animatedShell)
		{
			Transform instantiateShell = Instantiate(shellAnim, shellEjectPosition.position, shellEjectPosition.rotation);
			instantiateShell.parent = shellEjectPosition;
		}
		else
		{
			Rigidbody instantiateShellRigid = Instantiate(shellRigid, shellEjectPosition.position, shellEjectPosition.rotation);
			instantiateShellRigid.AddForce(transform.right * Random.Range(200f, 400f));
		}
		if (playAudioEffect)
		{
			weaponAnimationScript.PlayAudioEject();
		}
	}

	void EmitSmoke()
	{
		Instantiate(smoke, smokePosition.transform.position, smokePosition.transform.rotation);
	}

	void Kick()
	{
		if (typeOfKickBack == KickBackTypeGun.KB_Auto)
		{
			float randomKick = Random.Range(0.05f, 0.1f);
			float kickY = Random.Range(0, kickBackAmountY);
			MouseLookNew mainC = mainCamera.GetComponent<MouseLookNew>();
			mainC.offsetY = kickY;
			mainC.kickTime = randomKick;
			MouseLookNew wCam = weaponCamera.GetComponent<MouseLookNew>();
			wCam.offsetY = kickY;
			wCam.kickTime = randomKick;
			float kickX = Random.Range(-kickBackAmountX, kickBackAmountX);
			MouseLookNew rCam = player.GetComponent<MouseLookNew>();
			rCam.offsetX = kickX;
			rCam.kickTime = randomKick;
		}
		if (typeOfKickBack == KickBackTypeGun.KB_Semi)
		{
			kickGO.localRotation = Quaternion.Euler(kickGO.localRotation.eulerAngles + new Vector3(-kickAmount, Random.Range(-kickOffset, kickOffset), 0));
		}
		if (typeOfKickBack == KickBackTypeGun.KB_ADW)
		{
			StartCoroutine(Kick3(kickGO, new Vector3(-kbamount, Random.Range(minOffset, maxoffset), 0), kbtime));
		}
	}

	public IEnumerator Kick3(Transform goTransform, Vector3 kbDirection, float time)
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

	public IEnumerator Reload()
	{
		if ((inRun || reloading) || knifeOrGrenade)
		{
			yield break;
		}
		if (typeOfAmmo == AmmoTypeGun.byClip)
		{
			if ((numberOfClips > 0) && (bulletsLeft != bulletsPerClip))
			{
				reloading = true;
				while (reloading)
				{
					StartCoroutine(DisableFunctionality(2, 0f));
					weaponAnimationScript.ReloadWeapon();
					yield return new WaitForSeconds(reloadTime);
					numberOfClips--;
					bulletsLeft = bulletsPerClip;
					StartCoroutine(EnableFunctionality(2));
					yield break;
				}
			}
		}
		if (typeOfAmmo == AmmoTypeGun.byBullet)
		{
			if ((numberOfClips > 0) && (bulletsLeft != bulletsPerClip))
			{
				if (numberOfClips > bulletsPerClip)
				{
					reloading = true;
					while (reloading)
					{
						StartCoroutine(DisableFunctionality(2, 0f));
						weaponAnimationScript.ReloadWeapon();
						yield return new WaitForSeconds(reloadTime);
						numberOfClips = numberOfClips - (bulletsPerClip - bulletsLeft);
						bulletsLeft = bulletsPerClip;
						StartCoroutine(EnableFunctionality(2));
						yield break;
					}
				}
				else
				{
					reloading = true;
					while (reloading)
					{
						StartCoroutine(DisableFunctionality(2, 0f));
						weaponAnimationScript.ReloadWeapon();
						yield return new WaitForSeconds(reloadTime);
						int wepBullet = Mathf.Clamp(bulletsPerClip, numberOfClips, bulletsLeft + numberOfClips);
						numberOfClips = numberOfClips - (wepBullet - bulletsLeft);
						bulletsLeft = wepBullet;
						StartCoroutine(EnableFunctionality(2));
						yield break;
					}
				}
			}
		}
		if (typeOfAmmo == AmmoTypeGun.Shotgun)
		{
			if ((numberOfClips > 0) && (bulletsLeft != bulletsPerClip))
			{
				reloading = true;
				if (reloading)
				{
					StartCoroutine(DisableFunctionality(2, 0f));
					weaponAnimationScript.ReloadStart();
					yield return new WaitForSeconds(0.8f);
					while (((numberOfClips > 0) && (bulletsLeft != bulletsPerClip)) && !Input.GetButton("Fire1"))
					{
						weaponAnimationScript.InsertBullet();
						yield return new WaitForSeconds(0.9f);
						numberOfClips = numberOfClips - 1;
						bulletsLeft = bulletsLeft + 1;
					}
					StartCoroutine(weaponAnimationScript.ReloadStop());
					yield return new WaitForSeconds(0.9f);
					StartCoroutine(EnableFunctionality(2));
				}
			}
		}
	}

	public void SelectWeapon()
	{
		if (enableCrosshair)
		{
			crosshair.showCrosshair = true;
			crosshair.crosshairType = crosshairType;
		}
		else
		{
			crosshair.showCrosshair = false;
		}
		RendererBool(true);
		if (!Application.isPlaying)
		{
			return;
		}
		weaponAnimationScript.DrawWeapon();
		show = false;
		reloading = false;
		StartCoroutine(DrawTime());
		pw.switching = false;
	}

	public IEnumerator DeselectWeapon()
	{
		pw.canSwitch = false;
		canAim = false;
		gunActive = false;
		aiming = false;
		mainCamera.GetComponent<Camera>().fieldOfView = 60;
		weaponCamera.GetComponent<Camera>().fieldOfView = 45;
		weaponAnimationScript.PutDownWeapon();
		codcontroller.canRun = false;
		yield return new WaitForSeconds(0.5f);
		RendererBool(false);
	}

	public void QuickDeselectWeapon()
	{
		RendererBool(false);
		if (reloading)
		{
			StopCoroutine("Reload");
		}
		pw.canSwitch = false;
		canAim = false;
		gunActive = false;
		aiming = false;
		mainCamera.GetComponent<Camera>().fieldOfView = 60;
		weaponCamera.GetComponent<Camera>().fieldOfView = 45;
		codcontroller.canRun = false;
	}

	public void HideWeapon()
	{
		RendererBool(false);
	}

	void FireMode()
	{
		switch(typeOfFireMode)
		{
			case FireModeGun.Auto:
				typeOfFireMode = FireModeGun.BurstAuto;
				break;
			default:
				typeOfFireMode = FireModeGun.Auto;
				break;
		}
	}

	public IEnumerator OutOfAmmo()
	{
		if ((((reloading || playing) || inRun) || autoReload) || knifeOrGrenade)
		{
			yield break;
		}
		playing = true;
		weaponAnimationScript.PlayOutOfAmmo();
		yield return new WaitForSeconds(0.2f);
		playing = false;
	}

	void FireOneShot()
	{
		GameObject clone = Instantiate(bulletPrefab, bulletPosition.position, bulletPosition.rotation);
		if (bullet == BulletTypeGun.simpleBullet)
		{
			float[] simple = new float[4];
			simple[0] = damage;
			simple[1] = force;
			simple[2] = (spread / 1.5f) * (bulletSpeed / 100);
			simple[3] = bulletSpeed;
			clone.SendMessageUpwards("SetUp", simple);
		}
		else
		{
			if (bullet == BulletTypeGun.advancedBullet)
			{
				float[] bulletInfo = new float[8];
				bulletInfo[0] = damage;
				bulletInfo[1] = force;
				bulletInfo[2] = maxPenetration;
				bulletInfo[3] = bulletGravity;
				bulletInfo[4] = (spread / 1.5f) * (bulletSpeed / 100);
				bulletInfo[5] = bulletSpeed;
				bulletInfo[6] = StartPenetrationRange;
				bulletInfo[7] = EndPenetrationRange;
				clone.SendMessageUpwards("SetUp", bulletInfo);
			}
			else
			{
				if (bullet == BulletTypeGun.Arrow)
				{
					float[] arrow = new float[4];
					arrow[0] = damage;
					arrow[1] = force;
					arrow[2] = (spread / 1.5f) * (bulletSpeed / 100);
					arrow[3] = bulletSpeed;
					clone.SendMessageUpwards("SetUp", arrow);
				}
				else
				{
					if (bullet == BulletTypeGun.Rocket)
					{
						float[] rocket = new float[2];
						rocket[0] = damage;
						rocket[1] = bulletSpeed;
						clone.SendMessageUpwards("SetUp", rocket);
					}
				}
			}
		}
		if (typeOfAmmo != AmmoTypeGun.Shotgun)
		{
			bulletsLeft--;
		}
		m_LastFrameShot = Time.frameCount;
		if (typeOfFireMode == FireModeGun.Auto)
		{
			weaponAnimationScript.PlayFireAnimAuto();
		}
		else
		{
			weaponAnimationScript.PlayFireAnimSemi();
		}
		if (aiming)
		{
			if (spread < aimMaxSpread)
			{
				spread = spread + spreadIncrease;
			}
		}
		else
		{
			if (spread < hipMaxSpread)
			{
				spread = spread + spreadIncrease;
			}
			//when you fire weapon is shaking 
			Quaternion target = Quaternion.Euler(Random.insideUnitSphere * 30);
			transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime);
		}
	}

	public void Running()
	{
		inRun = true;
		if (reloading)
		{
			StopCoroutine("Reload");
			if (typeOfAmmo == AmmoTypeGun.Shotgun)
			{
				weaponAnimationScript.ResetReloading();
			}
			else
			{
				weaponAnimationScript.ReturnReloading();
			}
			StartCoroutine(EnableFunctionality(2));
		}
	}

	public void StopRunning()
	{
		inRun = false;
	}

	public IEnumerator DrawTime()
	{
		float startTime = Time.time;
		while (Time.time < (startTime + drawAnimTime))
		{
			pw.canSwitch = false;
			canFire = false;
			canAim = false;
			yield return null;
		}
		codcontroller.canRun = true;
		canFire = true;
		pw.canSwitch = true;
		canAim = true;
		gunActive = true;
	}

	void Laser()
	{
		RaycastHit hit = default(RaycastHit);
		LineRenderer lineRenderer = (LineRenderer)laserGO.GetComponent(typeof(LineRenderer));
		lineRenderer.useWorldSpace = false;
		lineRenderer.positionCount = 2;
		lineRenderer.startWidth = 0.01f;
		lineRenderer.endWidth = 0.002f;
		//lineRenderer.SetVertexCount(2);
		//lineRenderer.SetWidth(0.01f, 0.002f);
		int layerMask = ~(1 << 8);
		Physics.Raycast(laserGO.position, laserGO.forward, out hit, 50, layerMask);
		if (hit.collider)
		{
			lineRenderer.SetPosition(0, new Vector3(0, 0, 0.6f));
			lineRenderer.SetPosition(1, new Vector3(0, 0, hit.distance));
			laserHitPoint.position = laserGO.position + (laserGO.forward * (hit.distance - 0.1f));
			laserHitPoint.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
		}
		else
		{
			lineRenderer.SetPosition(1, new Vector3(0, 0, 1000));
			laserHitPoint.position = laserGO.position + (laserGO.forward * 100);
		}
	}

	void RendererBool(bool status)
	{
		var gos = GetComponentsInChildren(typeof(Renderer));
		foreach (Renderer go in gos)
		{
			if (go.name != "MuzzleFlash")
			{
				go.enabled = status;
			}
		}
	}
}
