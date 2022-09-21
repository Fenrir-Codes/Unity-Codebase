using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;
using ScreenLocker;
//using SG;

public enum StateHUD { Default, Vehicle, Disable }

public class HUDManager : MonoBehaviour {

	[Header("Health")]
	public Text Health;
	public Text HealthVehicle;
	[HideInInspector]
	public float HP_Vehicle_Value;

	public Image DamageReciver;

	[Header("Ammo")]
	public Text Ammo;

	[Header("Crosshair")]
	public RectTransform RootCross;
	public RectTransform CrossUp;
	public RectTransform CrossDown;
	public RectTransform CrossLeft;
	public RectTransform CrossRight;
	public RawImage hitCross;

	[Header("Scope")]
	public RectTransform ScopeRoot;
	public Image BackScope;
	public Image ScopeCenter;

	[Header("Special Gadgets")]
	public Text SentryGunCount;
	public LoopHorizontalScrollRect GadgetList;
	public LoopHorizontalScrollRect GrenadeList;
	public LoopHorizontalScrollRect StickyList;
	public LoopHorizontalScrollRect ClaymoreList;
	public string GrenadeItem = "Grenade_Item";
	public string StickyItem = "Sticky_Item";
	public string ClaymoreItem = "Claymore_Item";

	[Header("Weapon Slots")]
	public Text WeaponName_First;
	public Text WeaponName_Second;
	public Animation Slots;
	public string AnimSlot1 = "SlotFirst";
	public string AnimSlot2 = "SlotSecond";

	[Header("Weapon Info")]
	public RectTransform WeaponInfo;
	public Text WeaponInfo_Text;

	[Header("Fuel & KMH")]
	public Text Fuel;
	public Text KMH;
	[HideInInspector]
	public float _Fuel;
	[HideInInspector]
	public float _KMH;
	public CanvasRenderer KMH_UI;

	[Header("Score")]
	public Text Round;
	public Text Score;
	public Text Points;
	public Animation AddPoints;
	public string AddPointsAnim = "AddPoint";

	[Header("Wave&Round")]
	public RectTransform TimerNextRound;
	public Text _TimerNextRound;
	public Text NumComingRound;
	public Text RoundComplete;

	[Header("Skills")]
	public Text SkillPoints;
	public RectTransform SkillInfo;
	public Image SkillInfoTexture;
	public Text SkillDescription;
	public RectTransform NotEnoughPoints;
	public Text PointsWarning;
	[HideInInspector]
	public int skillPoints = 0;
	[HideInInspector]
	public bool showNotEnoughPoints = false;
	[Serializable]
	public class SkillConfig
	{
		public Sprite skillTexture;
		public string skillName;
		public string skillDscription;
		//@HideInInspector
		public int currentSkillLevel = 0;
		public float maxSkillLevel = 50f;
		public int priceForOneLevel = 1;
	}

	public SkillConfig[] Skills;

	[Header("Console")]
	public Text OutputText;
	public ConsoleInputSubmit Command_Input;
	public ConsoleInputSubmit Value_Input;
	public string[] variable;
	//private float timescale = 1.0f;
	internal string inputField = "";
	internal string value = "";
	internal ArrayList entries = new ArrayList();
	//private Vector2 scrollPosition;
	public bool clearTextFromConsole = false;

	[Header("Parkour")]
	public Text Climb;

	[Header("HUD's")]
	public StateHUD _StateHud;
	public Canvas HUD_Main;
	public Canvas HUD_Vehicle;
	public Canvas HUD_ScoreManager;

	[Header("Menu")]
	public RectTransform Menu;
	[HideInInspector]
	public bool hide = false;

	[Header("Options")]
	public Slider Sensitivity;
	public Slider SoundVolume;
	public Text SensitivityStatus;
	public Text VolumeStatus;
	public float MaxValueSensitivity = 10f;
	public float MaxValueVoulme = 1f;
	[HideInInspector]
	public float sensitivity = 2.0f;
	[HideInInspector]
	public float soundVol = 1.0f;
	[HideInInspector]
	public int graphicsQual = 5;
	internal string[] Qualnames;
	internal Resolution[] resolutions;
	public string[] Fullscreen = { "On", "OFF" };
	[HideInInspector]
	public int Fullscreenstatus = 0;
	internal int res; //Resolution
	internal string[] FPS = { "On", "OFF" };
	internal int FPSstatus = 1;
	internal int qual = 0;
	public string[] Aliasing = { " No AA Filtering ", "AA Filtering x2", "AA Filtering x4", "AA Filtering x8" };
	[HideInInspector]
	public int Antistatus = 0;
	public string[] Anisotropic = { " Disable ", "Enable", "Force Enable" };
	[HideInInspector]
	public int Filtering = 0;

	[Header("Frame Rate")]
	private float updateInterval = 0.5f;
	[HideInInspector]
	public float accum = 0.0f;
	[HideInInspector]
	public int frames = 0;
	private float timeleft;
	private String framerate;
	[HideInInspector]
	public int showFrameRate = 0;
	public RectTransform FrameRate;
	public Text _FrameRate;

	[Header("Other")]
	public RectTransform UseKey;
	public RawImage FadeTexture;
	internal bool ShowInfo;
	private bool UseParkour;
	internal PlayerController Player;
	private ParkourControllerNew PlayerParkour;
	private WeaponsPlayer ManagerWeapon;
	private ManagerScore ScoreManager;
	private SpawnerWithVawesNew SpawnerWithVawes;
	private ScriptExplosives Explosives;
	private CarController Car;
	private HelicopterControllerNew Helicopter;

	void Start()
	{
		ScreenLock.lockCursor = true;

		if (Player == null && FindObjectOfType<PlayerController>() != null)
		{
			Player = FindObjectOfType<PlayerController>();
			PlayerParkour = FindObjectOfType<ParkourControllerNew>();
			ManagerWeapon = Player.PlayerWep;
			Explosives = FindObjectOfType<ScriptExplosives>();
			Sensitivity.maxValue = MaxValueSensitivity;
			SoundVolume.maxValue = MaxValueVoulme;
		}
		else
		{
			Debug.LogError("Player prefab not found!");
			SwitchHUD(StateHUD.Disable);
		}

		if (ScoreManager == null && FindObjectOfType<ManagerScore>() != null)
		{
			ScoreManager = FindObjectOfType<ManagerScore>();
			HUD_ScoreManager.gameObject.SetActive(true);

			if(FindObjectOfType<SpawnerWithVawesNew>() != null)
				SpawnerWithVawes = FindObjectOfType<SpawnerWithVawesNew>();
		}
		else
		{
			Debug.LogError("ScoreManager prefab not found!");
			HUD_ScoreManager.gameObject.SetActive(false);
		}


		if (Car == null && FindObjectOfType<CarController>() != null)
			Car = FindObjectOfType<CarController>();
		else
			Debug.Log("Car not found");

		if (Helicopter == null && FindObjectOfType<HelicopterControllerNew>() != null)
			Helicopter = FindObjectOfType<HelicopterControllerNew>();
		else
			Debug.Log("Helicopter not found");
	}

	void Update()
	{
		if (Player != null)
		{

			//Frame Rate
			if (showFrameRate == 1)
			{
				timeleft -= Time.deltaTime;
				accum += Time.timeScale / Time.deltaTime;
				++frames;

				if (timeleft <= 0.0f)
				{
					framerate = "" + (accum / frames).ToString("F0");
					timeleft = updateInterval;
					accum = 0.0f;
					frames = 0;
				}
			}

			//Menu Show & Hide
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				if (hide == false)
				{
					hide = true;
				}
				else
				{
					hide = false;
				}
			}

			if (!hide)
			{
				Menu.gameObject.SetActive(false);
				ScreenLock.lockCursor = true;
			}
			else
			{
				Menu.gameObject.SetActive(true);
				ScreenLock.lockCursor = false;
			}

			//Slider Options (Sensitivity Mouse & Volume)
			sensitivity = Sensitivity.value;
			soundVol = SoundVolume.value;

			SensitivityStatus.text = Sensitivity.value.ToString("F2");
			VolumeStatus.text = SoundVolume.value.ToString("F1");

			AudioListener.volume = soundVol;

			if(Car != null || Helicopter != null)
			if (!Car.controlsEnabled || !Helicopter.controlsEnabled)
				SwitchHUD(StateHUD.Default);

			if (_StateHud != StateHUD.Vehicle)
			{
				//Health
				Health.text = string.Format("+ {0}", Player.controllerHealth.hitPoints.ToString("F0"));

				//Blood Screen
				DamageReciver.color = new Color(1f, 1f, 1f, Player.controllerHealth.alpha);

				//SentryGun Special Gadget
				SentryGunCount.text = ManagerWeapon.sentrygunUnits.ToString();

				//Score & Round
				Round.text = string.Format("Round: <color=#FFF24D>{0}</color>", ScoreManager.roundNR);
				Score.text = string.Format("Score: <color=#FFF24D>{0}</color>", ScoreManager.currentScore.ToString("F0"));
				hitCross.color = new Color(1.0f, 1.0f, 1.0f, ScoreManager.alphaHit);

				//SecondGadget
				if (Player != null)
				{
					GadgetList.totalCount = ManagerWeapon.gadgetController.count;
					GadgetList.RefillCells();
					SecondGadgetPool();
				}

				//Points Show
				if (ScoreManager.show)
				{
					Points.gameObject.SetActive(true);
					AddPoints.PlayQueued(AddPointsAnim, QueueMode.CompleteOthers);
					Points.text = string.Format("+{0}", ScoreManager.pointToAdd);
					Points.color = new Color(Points.color.r, Points.color.g, Points.color.b, ScoreManager.alpha);
				}
				else
				{
					Points.gameObject.SetActive(false);
				}

				//Weapon names
				WeaponName_First.text = ManagerWeapon.weaponsInUse[0].weaponName;
				WeaponName_Second.text = ManagerWeapon.weaponsInUse[1].weaponName;

				//Ammo
				if (ManagerWeapon.weaponsInUse[ManagerWeapon.currentWeapon].showAmmo == true)
				{

					Ammo.text = string.Format("<size=38><color=#FFF24D>{0}</color></size> <size=30>/{1}</size>", ManagerWeapon.bullets, ManagerWeapon.mags);
				}
				else
				{
					Ammo.text = "__  / __";
				}

				//Weapon Info (Pickup / BUY & etc...)
				if (ManagerWeapon.showInfo)
				{
					WeaponInfo.gameObject.SetActive(true);
					//Pickup
					if (ManagerWeapon.pickupGUI)
					{
						//if weapon used
						if (ManagerWeapon.wepInUseGUI)
						{
							WeaponInfo_Text.text = string.Format("{0} is already equipped", ManagerWeapon.textWepName);
						}
						else //else if not used
						{
							WeaponInfo_Text.text = string.Format("<size=8>{0}</size>", ManagerWeapon.textWepName);
						}
					}
					else
					{
						//Buy Info
						if (ManagerWeapon.buyWepGUI)
						{
							//if weapon used
							if (ManagerWeapon.wepInUseGUI)
							{
								//if we have "money"
								if (ManagerWeapon.enoughMoney)
								{
									WeaponInfo_Text.text = string.Format("{0} is already equipped \n Ammo Price = {1}", ManagerWeapon.textWepName, ManagerWeapon.ammoPrice);
								}
								else //else if we not enough "money"
								{
									WeaponInfo_Text.text = string.Format("{0} is already equipped \n Not enough money for AMMO", ManagerWeapon.textWepName);
								}
							}
							else
							{
								//if we have "money"
								if (ManagerWeapon.enoughMoney)
								{
									WeaponInfo_Text.text = string.Format("Press key  [ E ]  to buy > {0} \n  Price: {1}", ManagerWeapon.textWepName, ManagerWeapon.wepPrice);
								}
								else //else if we not enough "money"
								{
									WeaponInfo_Text.text = string.Format("{0} \n Not enough money   [ Price = {1} ] ", ManagerWeapon.textWepName, ManagerWeapon.wepPrice);
								}
							}
						}
					}
				}
				else
					WeaponInfo.gameObject.SetActive(false);

				//Parkour
				if ((!PlayerParkour.climbing && PlayerParkour.canClimb) && Player.grounded)
				{
					Climb.gameObject.SetActive(true);
					if (PlayerParkour.hitConfig)
					{
						Climb.text = "<color=orange><size=24>Press</size> <b><size=30><color=green>Space</color></size></b> <size=24>to climb \n over obstacle.</size></color>";
					}
					else
					{
						Climb.text = "<color=orange><size=24>Press</size> <b><size=30><color=green>Space</color></size></b> <size=24>to climb \n on obstacle.</size></color>";
					}
				}
				else
					Climb.gameObject.SetActive(false);

				SkillPoints.text = string.Format("Skill Points: {0}", skillPoints);

				//Skills
				if (showNotEnoughPoints)
					NotEnoughPoints.gameObject.SetActive(true);
				else
					NotEnoughPoints.gameObject.SetActive(false);

				//Console Fields
				inputField = Command_Input.text; //Command
				value = Value_Input.text; //Value command

				if (showFrameRate == 1)
				{
					FrameRate.gameObject.SetActive(true);
					_FrameRate.text = string.Format("Frame Rate : {0}", framerate);
				}
				else
					FrameRate.gameObject.SetActive(false);

				//Waves & Round
				if (SpawnerWithVawes != null && SpawnerWithVawes.enabled == true)
				{
					if (SpawnerWithVawes.countdown)
					{
						RoundComplete.gameObject.SetActive(false);
						TimerNextRound.gameObject.SetActive(true);
						NumComingRound.gameObject.SetActive(false);
						_TimerNextRound.text = string.Format("Rest Time : {0}", SpawnerWithVawes.cTimer.ToString("F2"));
					}
					if (SpawnerWithVawes.waveInfo)
					{
						TimerNextRound.gameObject.SetActive(false);
						NumComingRound.gameObject.SetActive(true);
						NumComingRound.text = SpawnerWithVawes.waveNrTimer.ToString("F0");
						//GUI.Label(new Rect(Screen.width / 2, (Screen.height / 2) - 150, 100, 100), "" + waveNrTimer.ToString("F0"), style4);
					}
					if (SpawnerWithVawes.showRoundCompleted)
					{
						RoundComplete.gameObject.SetActive(true);
						RoundComplete.text = string.Format(" Round {0} completed", SpawnerWithVawes.waveNr);
						//GUI.Label(new Rect(Screen.width / 2, 30, 100, 100), (" Round " + waveNr) + " completed", style5);
					}

					if (!SpawnerWithVawes.waveInfo)
						NumComingRound.gameObject.SetActive(false);
				}
			}

			if (ShowInfo)
				UseKey.gameObject.SetActive(true);
			else
				UseKey.gameObject.SetActive(false);

			//Car & Helicopter
			if (Car != null || Helicopter != null)
			{
				if (Car.controlsEnabled)
				{
					UseKey.gameObject.SetActive(false);
					SwitchHUD(StateHUD.Vehicle);
					KMH_UI.gameObject.SetActive(true);
					HealthVehicle.text = string.Format(" HP: {0}", Car.hitPoints.ToString("F0"));
					KMH.text = string.Format(" KM/H: <color=#FFF24D>{0}</color>", Car._Speed.ToString("F0"));
					Fuel.text = string.Format(" Fuel: <color=#FFF24D>{0}</color>", Car.fuel.ToString("F2"));
				}
				else if (Helicopter.controlsEnabled)
				{
					UseKey.gameObject.SetActive(false);
					SwitchHUD(StateHUD.Vehicle);
					KMH_UI.gameObject.SetActive(false);
					HealthVehicle.text = string.Format(" HP: {0}", Helicopter.HP.hitPoints.ToString("F0"));
					Fuel.text = string.Format(" Fuel: <color=#FFF24D>{0}</color>", Helicopter.fuel.ToString("F2"));
				}
			}
		}
	}

	public void ChangeValueCommand()
	{
		StartCoroutine(ApplyVariable(inputField, value));
		Command_Input.text = inputField = "";
		Value_Input.text = value = "";
		//Console
		foreach (ConsoleEntryFps entry in entries)
		{
			OutputText.text += string.Format("<color=green>{0}</color><color=yellow>{1}</color> <color=cyan>{2}</color>\n", entry.text, entry.note, entry.text2);
		}
		entries.Clear();
	}


	public void CloseConsole()
	{

		inputField = "";  //removing all text from input fields
		value = "";    //removing all text from input fields

		if (clearTextFromConsole)
			entries = new ArrayList();

	}


	public IEnumerator ApplyVariable(string str, string value)
	{
		var entry = new ConsoleEntryFps
		{
			text = str,
			text2 = "",
			note = ""
		};
		float newValue;
		//scrollPosition.y = 100000;

		if (str == variable[0])
		{ //bullet speed
			entries.Add(entry);
			if (float.TryParse(value, out newValue))
			{
				Player.PlayerWep.weaponsInUse[Player.PlayerWep.currentWeapon].weapon.bulletSpeed = newValue;
				entry.note = "- speed changed to";
				entry.text2 = newValue.ToString();
			}
		}
		else if (str == variable[1])
		{ //exit
			Application.Quit();
			print("exit");
		}
		else if (str == variable[2])
		{ //max quality
			entries.Add(entry);
			QualitySettings.SetQualityLevel(6, true);
			entry.note = " - quality changed to ";
			entry.text2 = "Fantastic";
		}
		else if (str == variable[3])
		{ //min quality
			entries.Add(entry);
			QualitySettings.SetQualityLevel(0, true);
			entry.note = " - quality changed to ";
			entry.text2 = "Fastest";
		}
		else if (str == variable[4])
		{ //ammo
			if (float.TryParse(value, out newValue))
			{
				Player.PlayerWep.weaponsInUse[Player.PlayerWep.currentWeapon].weapon.numberOfClips = (int)newValue;
				entry.note = " - update ammo to ";
				entry.text2 = newValue.ToString();
			}
		}
		else if (str == variable[5])
		{
			if (float.TryParse(value, out newValue))
			{ // max frame rate
				Application.targetFrameRate = (int)newValue;
				entry.note = " - max frame rate set to ";
				entry.text2 = newValue.ToString();
				entries.Add(entry);
			}
		}
		else if (str == variable[6])
		{ //timescale	
			if (float.TryParse(value, out newValue))
			{
				Time.timeScale = newValue;
				entry.note = " - TimeScale set to ";
				entry.text2 = newValue.ToString();
				entries.Add(entry);
				CloseConsole();
			}
		}
		else if (str == variable[7])
		{ //grenade	 
			Explosives.explType = Exposives.GRENADES;
			Explosives.grenadeCount = 3;
			entry.note = " - Explosives changed to ";
			entry.text2 = "Grenades";
			entries.Add(entry);

		}
		else if (str == variable[8])
		{ //claymore	
			Explosives.explType = Exposives.CLAYMORE;
			Explosives.grenadeCount = 3;
			entry.note = " - Explosives changed to ";
			entry.text2 = "Claymore";
			entries.Add(entry);

		}
		else if (str == variable[9])
		{ //sticky
			Explosives.explType = Exposives.STICKY;
			Explosives.grenadeCount = 3;
			entry.note = " - Explosives changed to Sticky ";
			entry.text2 = "Sticky";
			entries.Add(entry);

		}
		else if (str == variable[10])
		{ //cash
			if (float.TryParse(value, out newValue))
			{
				ScoreManager.addScore((int)newValue);
				entry.note = " Cash + ";
				entry.text2 = newValue.ToString();
				entries.Add(entry);
			}
		}
		else if (str == variable[11])
		{ //cash
			Player.controllerHealth.ApplyDamage(500);
		}
		else if (str == variable[12])
		{ //commands
			entry = new ConsoleEntryFps();
			entries.Add(entry);
			entry.text = "List of available commands:";

			for (int c = 0; c < variable.Length - 1; c++)
			{
				entry = new ConsoleEntryFps
				{
					text2 = c + ":  " + variable[c]
				};
				entries.Add(entry);
			}
		}
		else if (str == variable[13])
		{ //health
			if (float.TryParse(value, out newValue))
			{
				Player.controllerHealth.hitPoints = newValue;
				Player.controllerHealth.maximumHitPoints = newValue;
				entry.note = " - update health to ";
				entry.text2 = newValue.ToString();
			}
		}
		else if (str == variable[14])
		{ //clear
			OutputText.text = "";
		}
		else
		{
			entries.Add(entry);
			entry.note = " - is not recognized command";
			yield return null;
		}

		if (entries.Count > 50) entries.RemoveAt(0);

		yield return new WaitForSeconds(1.0f);
	}

	public void Resume(bool _hide)
	{
		hide = _hide;
	}

	public void OpenURL(string URL)
	{
		Application.OpenURL(URL);
	}

	public void Quit()
	{
		Application.Quit();
	}

	public void _showNotEnoughPoints(bool Value)
	{
		showNotEnoughPoints = Value;
	}

	public void addPoints()
	{
		skillPoints += 20;
	}

	public void UpdateSkills(int index)
	{
		switch(index)
		{
			case 0:
				Player.runSpeed += 1;
				break;

			case 1:
				Player.baseGravity -= 1;
				break;

			case 2:
				Player.controllerHealth.regenerationSpeed += 1;
				break;

			case 3:
				Player.jumpSpeed += 1;
				break;

			case 4:
				Player.controllerHealth.maximumHitPoints += 10;
				break;

			case 5:
				Player.controllerHealth.protectFromDamage += 1;
				break;

			case 6:
				Player.airControl = true;
				break;


		}
	}

	//Add Grenades / Sticky & Claymore in pool with update count
	void SecondGadgetPool()
	{
		switch (Explosives.explType)
		{
			case Exposives.GRENADES:
				GrenadeList.gameObject.SetActive(true);
				StickyList.gameObject.SetActive(false);
				ClaymoreList.gameObject.SetActive(false);

				GrenadeList.prefabSource.prefabName = GrenadeType();
				GrenadeList.totalCount = Explosives.grenadeCount;
				GrenadeList.RefillCells();
				break;
			case Exposives.STICKY:
				GrenadeList.gameObject.SetActive(false);
				StickyList.gameObject.SetActive(true);
				ClaymoreList.gameObject.SetActive(false);

				StickyList.prefabSource.prefabName = GrenadeType();
				StickyList.totalCount = Explosives.grenadeCount;
				StickyList.RefillCells();
				break;
			case Exposives.CLAYMORE:
				GrenadeList.gameObject.SetActive(false);
				StickyList.gameObject.SetActive(false);
				ClaymoreList.gameObject.SetActive(true);

				ClaymoreList.prefabSource.prefabName = GrenadeType();
				ClaymoreList.totalCount = Explosives.grenadeCount;
				ClaymoreList.RefillCells();
				break;

			default:
				GrenadeList.gameObject.SetActive(false);
				StickyList.gameObject.SetActive(false);
				ClaymoreList.gameObject.SetActive(false);
				break;
		}
	}

	//GetType Explosives
	string GrenadeType()
	{
		switch(Explosives.explType)
		{
			case Exposives.NONE:
				return "None";
			case Exposives.GRENADES:
				return GrenadeItem;
			case Exposives.STICKY:
				return StickyItem;
			case Exposives.CLAYMORE:
				return ClaymoreItem;

			default:
				return null;
		}
	}

	//Switcher HUD
	public void SwitchHUD (StateHUD State)
	{
		switch(State)
		{
			case StateHUD.Default:
				HUD_Main.gameObject.SetActive(true);
				HUD_Vehicle.gameObject.SetActive(false);
				break;

			case StateHUD.Vehicle:
				HUD_Main.gameObject.SetActive(false);
				HUD_Vehicle.gameObject.SetActive(true);
				break;

			case StateHUD.Disable:
				HUD_Main.gameObject.SetActive(false);
				HUD_Vehicle.gameObject.SetActive(false);
				break;
		}
	}
}
