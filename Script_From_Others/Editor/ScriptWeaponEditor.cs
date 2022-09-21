using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ScriptWeapon))]
public class ScriptWeaponEditor : Editor {


	/*@CustomEditor(WeaponScript)
	class WeaponScriptEditor extends Editor {
		var gunDisplayed : boolean = false;
		var autoReload : boolean = false;
		var FireModes = fireModeGun.Semi;
		var ZoomMode = ZoomingGun.None;
		var BulletMode = BulletType.simpleBullet;
		var AmmoMode = AmmoType.byBullet;
		var KickBackMode = KickBackTypeGun.None;
		var tex : Texture;*/

	public bool gunDisplayed = false;
	public bool autoReload = false;
	public FireModeGun FireModes = FireModeGun.Semi;
	public ZoomingGun ZoomMode = ZoomingGun.None;
	public BulletTypeGun BulletMode = BulletTypeGun.simpleBullet;
	public AmmoTypeGun AmmoMode = AmmoTypeGun.byBullet;
	public KickBackTypeGun KickBackMode = KickBackTypeGun.None;
	public Texture tex;

	public override void OnInspectorGUI () {

			var _target = target as ScriptWeapon;

			bool allowSceneObjects = !EditorUtility.IsPersistent(_target);
			EditorGUILayout.BeginHorizontal("textfield");
			GUILayout.FlexibleSpace();
			if(tex == null) tex = Resources.Load("armedUnity") as Texture;
			GUILayout.Box(tex);
			GUILayout.FlexibleSpace();
			EditorGUILayout.EndHorizontal();
		 
			if(GUILayout.Button("Show/Hide Weapon model",GUILayout.Height(40)) ) {
				if(!gunDisplayed){
					gunDisplayed = true;
					_target.SelectWeapon();
				} else if (gunDisplayed){
					gunDisplayed = false;
					_target.HideWeapon();
				}
			}
		
			EditorGUILayout.Separator();
			GUILayout.FlexibleSpace();
				GUILayout.Label ("Base Settings", EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();	
			EditorGUILayout.BeginVertical("Box");	
			_target.typeOfFireMode = (FireModeGun)EditorGUILayout.EnumPopup("  Fire Mode ", _target.typeOfFireMode);
			FireModes = _target.typeOfFireMode;
			EditorGUILayout.EndVertical();
		
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical("Box");
			_target.typeOfAmmo = (AmmoTypeGun)EditorGUILayout.EnumPopup("  Ammo Mode ", _target.typeOfAmmo);
			AmmoMode = _target.typeOfAmmo;
			EditorGUILayout.EndVertical();
		
		////////////////////////////////////// ZOOM ///////////////////////////////////////////////////	
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical("Box");
			_target.Zoom = (ZoomingGun)EditorGUILayout.EnumPopup("  Zoom Mode ", _target.Zoom);
			ZoomMode = _target.Zoom;
		
			if(ZoomMode == ZoomingGun.Scope){
				_target.scopeFOV = EditorGUILayout.IntField("  Zoom FOV ", _target.scopeFOV);
				_target.zoomInSpeed = EditorGUILayout.FloatField("  Zoom In Speed ", _target.zoomInSpeed);
				_target.zoomOutSpeed = EditorGUILayout.FloatField("  Zoom Out Speed ", _target.zoomOutSpeed);
		
				EditorGUILayout.BeginVertical("textfield");
					EditorGUILayout.BeginHorizontal ("textfield");
				GUILayout.Label (" Scope Texture", EditorStyles.boldLabel); 
				GUILayout.Space(70);	
				_target.scopeTexture = (Sprite)EditorGUILayout.ObjectField("", _target.scopeTexture, typeof(Sprite), allowSceneObjects);
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
			
		
			
				EditorGUILayout.BeginHorizontal ("textfield");
				GUILayout.Label (" Background Texture", EditorStyles.boldLabel);
				GUILayout.Space(30);
				_target.scopeBackgroundText = (Sprite)EditorGUILayout.ObjectField(_target.scopeBackgroundText, typeof(Sprite), allowSceneObjects);
				GUILayout.FlexibleSpace();
				EditorGUILayout.EndHorizontal();
				EditorGUILayout.EndVertical();
			
			
			}
		
			if(ZoomMode == ZoomingGun.Simple){
				_target.scopeFOV = EditorGUILayout.IntField("  Zoom FOV ", _target.scopeFOV);
				_target.zoomInSpeed = EditorGUILayout.FloatField("  Zoom In Speed ", _target.zoomInSpeed);
				_target.zoomOutSpeed = EditorGUILayout.FloatField("  Zoom Out Speed ", _target.zoomOutSpeed);
			}
			EditorGUILayout.EndVertical();
		
		////////////////////////////////////// KICKBACK ///////////////////////////////////////////////////	
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical("Box");
			_target.typeOfKickBack = (KickBackTypeGun)EditorGUILayout.EnumPopup(" KickBack Mode ", _target.typeOfKickBack);
			KickBackMode = _target.typeOfKickBack;
			if(KickBackMode == KickBackTypeGun.KB_Auto){
				_target.kickBackAmountY= EditorGUILayout.FloatField(" KickBack Y ", _target.kickBackAmountY);
				_target.kickBackAmountX = EditorGUILayout.FloatField(" KickBack X ", _target.kickBackAmountX);
				_target.returnSpeed = EditorGUILayout.FloatField(" Return Speed ", _target.returnSpeed);
			}
		
			if(KickBackMode == KickBackTypeGun.KB_Semi){
				_target.kickOffset = EditorGUILayout.FloatField(" Kick Offset ", _target.kickOffset);
				_target.kickAmount = EditorGUILayout.FloatField(" Kick Amount ", _target.kickAmount);
				_target.returnSpeed = EditorGUILayout.FloatField(" Return Speed ", _target.returnSpeed);
			}

			if(KickBackMode == KickBackTypeGun.KB_ADW){
				_target.kbamount = EditorGUILayout.FloatField(" KBamount ", _target.kbamount);
				_target.minOffset = EditorGUILayout.FloatField(" Min Offset ", _target.minOffset);
				_target.maxoffset = EditorGUILayout.FloatField(" Max Offset ", _target.maxoffset);
				_target.kbtime = EditorGUILayout.FloatField(" KB Time ", _target.kbtime);
				_target.returnSpeed = EditorGUILayout.FloatField(" Return Speed ", _target.returnSpeed);
			}		
			EditorGUILayout.EndVertical();
		
			EditorGUILayout.Separator();
			EditorGUILayout.BeginVertical("Box");
			_target.aimDownSights = EditorGUILayout.Toggle("  AimDownSights ", _target.aimDownSights);
			if(_target.aimDownSights == true){
				_target.aimInSpeed = EditorGUILayout.FloatField("  Aim In Speed ", _target.aimInSpeed);
				_target.aimOutSpeed = EditorGUILayout.FloatField("  Aim Out Speed ", _target.aimOutSpeed);
				if(ZoomMode == ZoomingGun.Scope){
					_target.quickReloadTime = EditorGUILayout.FloatField("  Fast Reload Time ", _target.quickReloadTime);
				}
				EditorGUILayout.BeginVertical ("textfield");
				_target.hipPosition = EditorGUILayout.Vector3Field("", _target.hipPosition);
					if(GUILayout.Button(new GUIContent("Save Hip Position"),"miniButton")) {
						_target.hipPosition = _target.transform.localPosition;
					}	
			
				_target.aimPosition = EditorGUILayout.Vector3Field("", _target.aimPosition);
					if(GUILayout.Button(new GUIContent("Save Aim Position"),"miniButton")) {
						_target.aimPosition = _target.transform.localPosition;
					}
				
				EditorGUILayout.Separator();
				if(GUILayout.Button(new GUIContent("Reset Hip Position"),"miniButton")) {
					_target.transform.localPosition = Vector3.zero;
					_target.hipPosition = Vector3.zero;
				}
				
				if(GUILayout.Button(new GUIContent("Set Aim Position"),"miniButton")) {
					_target.transform.localPosition = _target.aimPosition;
				}
				EditorGUILayout.EndVertical();	
			}
			EditorGUILayout.EndVertical();
			EditorGUILayout.Separator(); 
		
		EditorGUILayout.Separator();
		//EditorGUIUtility.LookLikeInspector();
	
			//FIRE MODES
				
			
						GUILayout.Label ("Weapon and Bullet Settings", EditorStyles.boldLabel);
						_target.bullet = (BulletTypeGun)EditorGUILayout.EnumPopup(" Projectile Type ", _target.bullet);
					BulletMode = _target.bullet;
				
					_target.bulletPrefab = (GameObject)EditorGUILayout.ObjectField(" Projectile Prefab ", _target.bulletPrefab, typeof(GameObject), allowSceneObjects);
					_target.bulletPosition = (Transform)EditorGUILayout.ObjectField(" Projectile Spawn Position ", _target.bulletPosition, typeof(Transform), allowSceneObjects);
					_target.bulletSpeed = EditorGUILayout.FloatField(" Projectile Speed ", _target.bulletSpeed);
					_target.bulletGravity = EditorGUILayout.FloatField(" Projectile Gravity ", _target.bulletGravity);
					_target.maxPenetration = EditorGUILayout.FloatField(" Penetration Level ", _target.maxPenetration);
					_target.StartPenetrationRange = EditorGUILayout.FloatField(" Penetration Start Range ", _target.StartPenetrationRange);
					_target.EndPenetrationRange = EditorGUILayout.FloatField(" Penetration End Range ", _target.EndPenetrationRange);
					_target.force = EditorGUILayout.IntField(" Projectile Hit Force ", _target.force);	
					_target.damage = EditorGUILayout.IntField(" Projectile Damage ", (int)_target.damage);
				
					//SPECIFIC BURST VARIABLES
					if(FireModes == FireModeGun.BurstSemi || FireModes == FireModeGun.BurstAuto){
						_target.shotsPerBurst = EditorGUILayout.IntField(" Shots Per Burst ", _target.shotsPerBurst);
						_target.burstTime = EditorGUILayout.FloatField(" Burst Time ", _target.burstTime);
					}
					//_target.bulletRPM = EditorGUILayout.FloatField(" Bullet RPM <fire rate> ", _target.bulletRPM);
					_target.fireRate = EditorGUILayout.FloatField(" Fire Rate ", _target.fireRate);
					_target.bulletsPerClip = EditorGUILayout.IntField(" Bullets Per Clip ", _target.bulletsPerClip);
					_target.numberOfClips = EditorGUILayout.IntField(" Number Of Clips ", _target.numberOfClips);
					_target.maxNumberOfClips = EditorGUILayout.IntField(" Max Number Of Clips ", _target.maxNumberOfClips);
						if(FireModes == FireModeGun.Shotgun){
							_target.SGpelletsPerShot = EditorGUILayout.IntField(" Pellets Per Shot ", _target.SGpelletsPerShot);
						}
					_target.autoReload = EditorGUILayout.Toggle(" Auto Reload ", _target.autoReload);
				
						// ACURACY OF WEAPON
						GUILayout.Label ("Weapon Acuracy", EditorStyles.boldLabel);
					_target.baseSpread = EditorGUILayout.FloatField(" Base Spread ", _target.baseSpread);
					_target.hipMaxSpread = EditorGUILayout.FloatField(" Max Hip Spread ", _target.hipMaxSpread);
					_target.aimMaxSpread = EditorGUILayout.FloatField(" Max Aim Spread ", _target.aimMaxSpread);
					_target.spreadIncrease = EditorGUILayout.FloatField(" Spread Increase ", _target.spreadIncrease);
					_target.spreadDecrease = EditorGUILayout.FloatField(" Spread Decrease ", _target.spreadDecrease);
				
						//CROSSHAIR
					GUILayout.Label ("Crosshair", EditorStyles.boldLabel);
					_target.enableCrosshair = EditorGUILayout.Toggle("  Enable Crisshair ", _target.enableCrosshair);
					if(_target.enableCrosshair){
						_target.updateCrosshair = EditorGUILayout.Toggle("    Dynamic Crisshair? ", _target.updateCrosshair);
						_target.multiplier = EditorGUILayout.FloatField("    Size Multiplier ", _target.multiplier);
						_target.crosshairType = EditorGUILayout.IntField("    Crosshair Type ", _target.crosshairType);
					}
				
					//ANIMATION TIMING
						GUILayout.Label ("Animations", EditorStyles.boldLabel);
					_target.reloadTime = EditorGUILayout.FloatField(" Reload Time ", _target.reloadTime);
					_target.drawAnimTime = EditorGUILayout.FloatField(" Draw Time ", _target.drawAnimTime);

				EditorGUILayout.Separator();
			
			
				//EFFECTS
					GUILayout.Label ("Effects", EditorStyles.boldLabel);
					EditorGUILayout.BeginVertical("box");
				_target.muzzleFlashEnabled = EditorGUILayout.Toggle("Enable MFlash", _target.muzzleFlashEnabled);
				if(_target.muzzleFlashEnabled){
					_target.muzzleFlash = (GameObject)EditorGUILayout.ObjectField(" MuzzleFlash GO", _target.muzzleFlash, typeof(GameObject), allowSceneObjects);
					_target.muzzleFlashRenderer = (Renderer)EditorGUILayout.ObjectField(" MuzzleRenderer", _target.muzzleFlashRenderer,  typeof(Renderer), allowSceneObjects);
				}
				EditorGUILayout.EndVertical();
					EditorGUILayout.BeginVertical("box");
				_target.muzzleLightEnabled = EditorGUILayout.Toggle("Enable MLight ", _target.muzzleLightEnabled);
				if(_target.muzzleLightEnabled){
					_target.muzzleLight = (Light)EditorGUILayout.ObjectField(" Muzzle Light", _target.muzzleLight, typeof(Light), allowSceneObjects);
				}
				EditorGUILayout.EndVertical();
					EditorGUILayout.BeginVertical("box");
				_target.smokeEnabled = EditorGUILayout.Toggle("Enable Smoke ", _target.smokeEnabled);
				if(_target.smokeEnabled){
					_target.smokePosition = (GameObject)EditorGUILayout.ObjectField(" SmokePos GO", _target.smokePosition, typeof(GameObject), allowSceneObjects);
					_target.smoke = (GameObject)EditorGUILayout.ObjectField(" Smoke Prefab", _target.smoke, typeof(GameObject), allowSceneObjects);
				}
				EditorGUILayout.EndVertical();
					EditorGUILayout.BeginVertical("box");
					_target.laserEnabled = EditorGUILayout.Toggle("Enable Laser ", _target.laserEnabled);
				if(_target.laserEnabled){
					_target.laserGO = (Transform)EditorGUILayout.ObjectField(" LaserPos GO", _target.laserGO, typeof(Transform), allowSceneObjects);
					_target.laserHitPoint = (Transform)EditorGUILayout.ObjectField(" LaserHit GO", _target.laserHitPoint, typeof(Transform), allowSceneObjects);
				}
				EditorGUILayout.EndVertical();
					EditorGUILayout.BeginVertical("box");
					_target.shellsEnabled = EditorGUILayout.Toggle("Shell Ejection", _target.shellsEnabled);
				if(_target.shellsEnabled){
					_target.timeBeforeEjection = EditorGUILayout.FloatField(" Time Before Ejection", _target.timeBeforeEjection);
					_target.shellEjectPosition = (Transform)EditorGUILayout.ObjectField(" Eject Position", _target.shellEjectPosition, typeof(Transform), allowSceneObjects);	
					_target.animatedShell = EditorGUILayout.Toggle("Animated Shell? ", _target.animatedShell);
		
					if(_target.animatedShell){
						_target.shellAnim = (Transform)EditorGUILayout.ObjectField(" Anim Shell Prefab", _target.shellAnim, typeof(Transform), allowSceneObjects);
						_target.playAudioEffect = EditorGUILayout.Toggle(" If Audio - not required", _target.playAudioEffect);
					}else{
						_target.shellRigid = (Rigidbody)EditorGUILayout.ObjectField(" Rigid Shell Prefab", _target.shellRigid, typeof(Rigidbody), allowSceneObjects);
					}	
				}
				EditorGUILayout.EndVertical();
			
				//OTHER
					GUILayout.Label ("Other", EditorStyles.boldLabel);
				EditorGUILayout.BeginVertical("textarea");	
				_target.mainCamera = (GameObject)EditorGUILayout.ObjectField(" Main Camera GO ", _target.mainCamera, typeof(GameObject), allowSceneObjects);
				_target.weaponCamera = (GameObject)EditorGUILayout.ObjectField(" Weapon Camera GO ", _target.weaponCamera,  typeof(GameObject), allowSceneObjects);
				_target.kickGO = (Transform)EditorGUILayout.ObjectField(" Kick GO ", _target.kickGO, typeof(Transform), allowSceneObjects);
				_target.codcontroller = (PlayerController)EditorGUILayout.ObjectField("PlayerController", _target.codcontroller, typeof(PlayerController), allowSceneObjects);		
				_target.weaponAnimationScript = (WeaponScriptAnimations)EditorGUILayout.ObjectField("WeaponAnimationScript ", _target.weaponAnimationScript, typeof(WeaponScriptAnimations), true);
				_target.crosshair = (DynamicCrosshairNew)EditorGUILayout.ObjectField("Crosshair Script ", _target.crosshair, typeof(DynamicCrosshairNew), true);
				_target.explosivesScript = (ScriptExplosives)EditorGUILayout.ObjectField("Explosives Script ", _target.explosivesScript, typeof(ScriptExplosives), allowSceneObjects);
				_target.pw = (WeaponsPlayer)EditorGUILayout.ObjectField("Player Weapons ", _target.pw, typeof(WeaponsPlayer), true);
		
				EditorGUILayout.EndVertical();

			EditorGUILayout.Separator();

		}
}
	//EditorGUIUtility.LookLikeInspector();
	//DrawDefaultInspector();
	//EditorGUIUtility.LookLikeControls();
