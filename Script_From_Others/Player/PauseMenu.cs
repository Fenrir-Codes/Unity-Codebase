using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScreenLocker;

/**
*  Script written by OMA [www.armedunity.com]
*  Rewritten on C# & adapted for latest version Unity3D by DeadZone [vk.com/id160454360] || [Discord: DeadZoneGarry#3474] || [Skype: vanya197799] || [steamcommunity.com/profiles/76561198121485860]
**/

//////////////CONSOLE
[Serializable]
public class ConsoleEntryFps
{
	public string text;
	public string text2;
	public string note;
}

public class PauseMenu : MonoBehaviour {

	private float hideWindowMenu;
	[HideInInspector]
	public bool hide = false;
	private float menuWinPos;
	private float settingsWinPos;
	private bool inRange = false;
	public float speed = 700.0f;
	private int window = 0;

	//OPTIONS
	public float sensitivity = 2.0f;
	public float soundVol = 1.0f;
	public int graphicsQual = 5;
	public string[] Fullscreen = { "On", "OFF" };
	public int Fullscreenstatus = 0;
	internal int res; //Resolution
	internal string[] FPS = { "On", "OFF" };
	private int FPSstatus = 1;
	internal int qual = 0;
	public string[] Aliasing = { " No AA Filtering ", "AA Filtering x2", "AA Filtering x4", "AA Filtering x8" };
	public int Antistatus = 0;

	public string[] Anisotropic = { " Disable ", "Enable", "Force Enable" };
	public int Filtering = 0;

	//CONSOLE
	public string[] variable;
	//private float timescale = 1.0f;
	internal string inputField = "";
	internal string value = "";

	internal ArrayList entries = new ArrayList();
	private Vector2 scrollPosition;
	public bool clearTextFromConsole = false;
	//public GUIStyle consoleSkin;

	//FrameRate
	private float updateInterval = 0.5f;
	public float accum = 0.0f; 
	public int frames = 0; 
	private float timeleft;
	private String framerate;
	public int showFrameRate = 0;

	//Skills
	private Vector2 scrollPositionSkills;
	private Vector2 scrollPositionOptions;
	internal bool showTip = false;
	internal bool showNotEnoughPoints = false;
	private Vector2 pos;
	internal int skillInfo = 0;
	internal int skillSelected = 0;

	/*public GUIStyle PBGuiStyle;
	public GUIStyle PBGuiStyle2;
	public GUIStyle PBGuiStyle3;
	public GUIStyle PBGuiStyle4;
	public GUIStyle PBGuiStyle5;
	public GUIStyle PBGuiStyle6;
	public GUIStyle SettingsGuiStyleBody;*/
	public Texture tex;
	public Texture tex2;
	public Texture tex3;
	public Texture tex4;
	public Texture tex6;
	public Texture buttonClose;
	//public GUISkin mySkin;

	public int skillPoints = 0;
	//public GUISkin SettingsSkin;
	public Texture sliderBG;
	public Texture sliderThumb;

	//public GUIStyle FPSSkin;

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
	public PlayerController codController;
	public HealthControllerPlayer codControllerHealth;
	public ScriptExplosives grenades;
	private ScriptWeapon weaponScript;
	public WeaponsPlayer playerweapons;
	public ManagerScore scoreManager;

	void Start()
	{
		menuWinPos = -200;
		settingsWinPos = Screen.width + 200;
		pos.y = -200;
		window = 1;
		inRange = false;
		ScreenLock.lockCursor = true;
		//	Application.targetFrameRate = 120;
		timeleft = updateInterval;
		hide = true;
	}

	/*public void Update()
	{

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

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (hide == false)
			{
				hide = true;
				ScreenLock.lockCursor = true;
			}
			else
			{
				hide = false;
				//inRange = true;
				ScreenLock.lockCursor = false;
			}
		}

		if (inRange)
		{
			float menuTarget;
			float settingsTarget;
			if (hide)
			{
				menuTarget = -200;
				settingsTarget = Screen.width + 200;
				if (menuWinPos < -180) inRange = false;
				showTip = false;
				showNotEnoughPoints = false;
			}
			else
			{
				menuTarget = Screen.width / 2 - 325;
				settingsTarget = Screen.width / 2 - 175;
			}

			menuWinPos = Mathf.MoveTowards(menuWinPos, menuTarget, speed * Time.deltaTime);
			settingsWinPos = Mathf.MoveTowards(settingsWinPos, settingsTarget, (speed * 1.7f) * Time.deltaTime);

			if (showTip) pos.y = Mathf.MoveTowards(pos.y, 0, 200 * Time.deltaTime);
			else pos.y = Mathf.MoveTowards(pos.y, -200, 300 * Time.deltaTime);
		}
	}


	public void OnGUI()
	{

		if (inRange)
		{
			if (showNotEnoughPoints)
			{
				Rect NotEnoughPoints = new Rect(Screen.width / 2 - 145, Screen.height / 2 - 50, 350, 100);
				NotEnoughPoints = GUI.Window(0, NotEnoughPoints, WarningWindow, "");
			}
			var windowtoolTip = new Rect(Screen.width / 2 - 325, pos.y, 650, 100);
			windowtoolTip = GUI.Window(3, windowtoolTip, ToolTips, "");

			var windowMenu = new Rect(menuWinPos, Screen.height / 2 - 170, 150, 340);
			var windowSettings = new Rect(settingsWinPos, Screen.height / 2 - 170, 500, 340);


			windowMenu = GUI.Window(1, windowMenu, MenuButtons, "");
			windowSettings = GUI.Window(2, windowSettings, MenuSettings, "");


		}

		if (showFrameRate == 1)
		{
			GUI.Box(new Rect(Screen.width - 130, 1, 130, 30), "Frame Rate : " + framerate, FPSSkin);
		}
	}

	public void WarningWindow(int windowID)
	{
		GUI.skin = mySkin;
		GUI.DrawTexture(new Rect(0, 0, 350, 100), tex3);
		if (GUI.Button(new Rect(255, 15, 80, 30), "Close")) showNotEnoughPoints = false;
		GUI.Label(new Rect(10, 45, 300, 40), "Points required: " + Skills[skillSelected].priceForOneLevel + "  |  " + "Points available: " + skillPoints, PBGuiStyle6);
	}

	public void ToolTips(int windowID)
	{
		GUI.BringWindowToFront(0);
		GUI.DrawTexture(new Rect(0, 0, 650, 100), tex6);
		GUILayout.BeginArea(new Rect(3, 4, 640, 90), "");
		GUILayout.BeginHorizontal("");
		//GUILayout.Box(Skills[skillInfo].skillTexture, GUILayout.Width(85), GUILayout.Height(85));
		GUILayout.Space(8);
		GUILayout.Label(Skills[skillInfo].skillDscription, GUILayout.Width(503), GUILayout.Height(80));


		if (GUILayout.Button(buttonClose, GUILayout.Width(30), GUILayout.Height(30)))
		{
			showTip = false;
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea();
	}

	public void MenuButtons(int windowID)
	{
		GUI.BringWindowToFront(3);
		GUI.skin = mySkin;
		var style1 = mySkin.customStyles[0];
		var style2 = mySkin.customStyles[1];

		GUI.DrawTexture(new Rect(0, 0, 150, 340), tex2);

		if (GUI.Button(new Rect(15, 35, 120, 30), "GUI_Resume"))
		{
			hide = true;
			ScreenLock.lockCursor = true;
		}

		if (GUI.Button(new Rect(15, 70, 120, 30), "GUI_Skills")) window = 1;

		if (GUI.Button(new Rect(15, 105, 120, 30), "GUI_Options")) window = 2;

		if (GUI.Button(new Rect(15, 140, 120, 30), "GUI_Console")) window = 3;

		if (GUI.Button(new Rect(15, 175, 120, 30), "Button_GUITEST")) window = 4;

		if (GUI.Button(new Rect(15, 260, 120, 30), "Button_GUITEST", style2)) Application.OpenURL("http://google.com/");


		if (Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.WebGLPlayer)
		{
			if (GUI.Button(new Rect(15, 295, 120, 30), "GUI_Reload Level", style1)) SceneManager.LoadScene(SceneManager.GetActiveScene().name);//Application.LoadLevel(Application.loadedLevel);
		}
		else
		{
			if (GUI.Button(new Rect(15, 295, 120, 30), "GUI_Quit Game", style1)) Application.Quit(); // if standalone
		}
	}

	public void MenuSettings(int windowID)
	{
		if (showNotEnoughPoints) GUI.BringWindowToFront(3);

		GUI.skin = SettingsSkin;
		//var skin1 = SettingsSkin.customStyles[0];
		var skin2 = SettingsSkin.customStyles[1];
		var skin3 = SettingsSkin.customStyles[2];
		var skin4 = SettingsSkin.customStyles[3];
		var skin5 = SettingsSkin.customStyles[4];
		var skin6 = SettingsSkin.customStyles[5];
		var skin7 = SettingsSkin.customStyles[6];

		GUI.DrawTexture(new Rect(0, 0, 500, 340), tex);

		if (window == 0)
		{
			//In this window you can show stats, or some information...
		}
		else if (window == 1)
		{
			//SKILL SYSTEM	
			GUILayout.Space(10);
			GUILayout.BeginVertical("Label");

			GUILayout.BeginHorizontal("", PBGuiStyle4, GUILayout.Width(480), GUILayout.Height(38));
			GUILayout.Label("Skill Points: " + skillPoints, PBGuiStyle6, GUILayout.Width(150), GUILayout.Height(35));

			GUILayout.FlexibleSpace();

			GUILayout.EndHorizontal();
			GUILayout.EndVertical();

			GUILayout.BeginArea(new Rect(15, 77, 473, 250), "");

			GUILayout.BeginVertical("Label");
			scrollPositionSkills = GUILayout.BeginScrollView(scrollPositionSkills);
			for (int i = 0; i < Skills.Length; i++){
				GUILayout.BeginHorizontal(PBGuiStyle, GUILayout.Width(455), GUILayout.Height(60));
				if (GUILayout.Button(Skills[i].skillTexture, GUILayout.Width(50), GUILayout.Height(50)))
				{
					skillInfo = i;
					showTip = true;
					pos.y = -110;
				}
				GUILayout.Space(7);
				GUILayout.BeginVertical("");
				GUILayout.Space(7);

				GUILayout.Label(Skills[i].skillName.ToString(), GUILayout.MaxWidth(80), GUILayout.MinWidth(80));
				GUILayout.Label("LVL  " + Skills[i].currentSkillLevel + " / " + Skills[i].maxSkillLevel, GUILayout.MaxWidth(80), GUILayout.MinWidth(80));
				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();

				GUILayout.Space(10);

				GUILayout.BeginVertical("");
				GUILayout.Space(15);

				float barSize = Mathf.Clamp01(Skills[i].currentSkillLevel / Skills[i].maxSkillLevel);
				GUILayout.Box("", PBGuiStyle2, GUILayout.Width(barSize * 220), GUILayout.MinHeight(30));


				GUILayout.FlexibleSpace();
				GUILayout.EndVertical();
				GUILayout.FlexibleSpace();


				GUILayout.BeginVertical("");
				GUILayout.Space(10);

				if (GUILayout.Button("", PBGuiStyle3, GUILayout.Width(45), GUILayout.Height(45)))
				{
					skillSelected = i;
					if (Skills[i].currentSkillLevel < Skills[i].maxSkillLevel && skillPoints >= 0)
					{
						if (Skills[i].priceForOneLevel <= skillPoints)
						{
							Skills[i].currentSkillLevel += 1;
							skillPoints -= Skills[i].priceForOneLevel;
							//showTip = false;
							UpdateSkills(i);
						}
						else
						{
							//showTip = true;
							showNotEnoughPoints = true;
						}
					}
				}

				GUILayout.EndVertical();
				GUILayout.Space(9);
				GUILayout.EndHorizontal();
				GUILayout.Space(2);
			}


			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.EndArea();

		}
		else if (window == 2)
		{
			showTip = false;
			//OPTIONS

			GUI.Label(new Rect(30, 40, 100, 20), "Sensitivity: ");
			sensitivity = GUI.HorizontalSlider(new Rect(130, 45, 250, 30), sensitivity, 0.0f, 10.0f);
			GUI.DrawTexture(new Rect(130, 47, sensitivity * 25, 7), sliderBG);
			GUI.DrawTexture(new Rect(130 + (sensitivity * 24), 45, 12, 12), sliderThumb);
			GUI.Box(new Rect(390, 40, 70, 20), sensitivity.ToString("F2"));

			GUI.Label(new Rect(30, 70, 100, 20), "Sound Volume: ");
			soundVol = GUI.HorizontalSlider(new Rect(130, 75, 250, 30), soundVol, 0.0f, 1.0f);
			GUI.DrawTexture(new Rect(130, 77, soundVol * 250, 7), sliderBG);
			GUI.DrawTexture(new Rect(130 + (soundVol * 240), 75, 12, 12), sliderThumb);
			GUI.Box(new Rect(390, 70, 70, 20), soundVol.ToString("F1"));
			AudioListener.volume = soundVol;

			GUILayout.BeginArea(new Rect(20, 95, 467, 225), "");
			GUILayout.BeginVertical();
			scrollPositionOptions = GUILayout.BeginScrollView(scrollPositionOptions);

			//Quality	
			GUILayout.BeginHorizontal("", skin6, GUILayout.Width(430), GUILayout.Height(40));

			GUILayout.Label("Quality", skin2, GUILayout.Width(180), GUILayout.Height(35));
			GUILayout.Space(20);
			var names = QualitySettings.names;

			if (GUILayout.Button("", skin3, GUILayout.Width(30), GUILayout.Height(30)))
			{
				if (qual < names.Length)
				{
					qual--;
					if (qual < 0) qual = names.Length - 1;
				}
			}

			if (GUILayout.Button(names[qual], skin5, GUILayout.Width(140), GUILayout.Height(30)))
			{
				QualitySettings.SetQualityLevel(qual, false);
			}

			if (GUILayout.Button("", skin4, GUILayout.Width(30), GUILayout.Height(30)))
			{
				if (qual < names.Length)
				{
					qual++;
					if (qual > (names.Length - 1)) qual = 0;

				}
			}

			GUILayout.EndHorizontal();

			//Resolution		
			GUILayout.BeginHorizontal("", skin6, GUILayout.Width(430));

			GUILayout.Label("Resolution ", skin2, GUILayout.Width(180), GUILayout.Height(40));
			GUILayout.Space(20);
			Resolution[] resolutions = Screen.resolutions;

			if (GUILayout.Button("", skin3, GUILayout.Width(30), GUILayout.Height(30)))
			{
				if (res < resolutions.Length)
				{
					res--;
					if (res < 0) res = resolutions.Length - 1;
				}
			}

			if (GUILayout.Button(resolutions[res].width + "x" + resolutions[res].height, skin5, GUILayout.Width(140), GUILayout.Height(30)) || Event.current.type == EventType.KeyDown && Event.current.character == char.Parse("\n"))
			{
				Screen.SetResolution(resolutions[res].width, resolutions[res].height, false);
			}

			if (GUILayout.Button("", skin4, GUILayout.Width(30), GUILayout.Height(30)))
			{
				if (res < resolutions.Length)
				{
					res++;
					if (res > (resolutions.Length - 1)) res = 0;

				}
			}

			GUILayout.EndHorizontal();

			//AntiAliasing
			GUILayout.BeginHorizontal("", skin6, GUILayout.Width(430));

			GUILayout.Label("AntiAliasing ", skin2, GUILayout.Width(180), GUILayout.Height(40));
			GUILayout.Space(20);
			if (GUILayout.Button("", skin3, GUILayout.Width(30), GUILayout.Height(30)))
			{
				Antistatus--;
				if (Antistatus < 0) Antistatus = 4 - 1;
			}

			if (GUILayout.Button(Aliasing[Antistatus], skin5, GUILayout.Width(140), GUILayout.Height(30)))
			{
				if (Antistatus == 0)
				{
					QualitySettings.antiAliasing = 0;
				}
				else if (Antistatus == 1)
				{
					QualitySettings.antiAliasing = 2;
				}
				else if (Antistatus == 2)
				{
					QualitySettings.antiAliasing = 4;
				}
				else if (Antistatus == 3)
				{
					QualitySettings.antiAliasing = 8;
				}
			}

			if (GUILayout.Button("", skin4, GUILayout.Width(30), GUILayout.Height(30)))
			{
				Antistatus++;
				if (Antistatus > (4 - 1)) Antistatus = 0;
			}

			GUILayout.EndHorizontal();

			//Anisotropic Filtering		
			GUILayout.BeginHorizontal("", skin6, GUILayout.Width(430));

			GUILayout.Label("Anisotropic Filtering ", skin2, GUILayout.Width(180), GUILayout.Height(40));
			GUILayout.Space(20);
			if (GUILayout.Button("", skin3, GUILayout.Width(30), GUILayout.Height(30)))
			{
				Filtering--;
				if (Filtering < 0) Filtering = 3 - 1;
			}

			if (GUILayout.Button(Anisotropic[Filtering], skin5, GUILayout.Width(140), GUILayout.Height(30)))
			{
				if (Filtering == 0)
				{
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
				}
				else if (Filtering == 1)
				{
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
				}
				else if (Filtering == 2)
				{
					QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
				}
			}

			if (GUILayout.Button("", skin4, GUILayout.Width(30), GUILayout.Height(30)))
			{
				Filtering++;
				if (Filtering > (3 - 1)) Filtering = 0;
			}


			GUILayout.EndHorizontal();

			//Fullsreen	
			GUILayout.BeginHorizontal("", skin6, GUILayout.Width(430));

			GUILayout.Label("Fullsreen ", skin2, GUILayout.Width(180), GUILayout.Height(36));
			GUILayout.Space(50);

			if (GUILayout.Button(Fullscreen[Fullscreenstatus], skin5, GUILayout.Width(140), GUILayout.Height(27)))
			{
				if (Fullscreenstatus == 1)
				{
					Screen.fullScreen = true;
					Fullscreenstatus = 0;
				}
				else
				{
					Screen.fullScreen = false;
					Fullscreenstatus = 1;
				}
			}
			GUILayout.EndHorizontal();

			//Show Frame Rate
			GUILayout.BeginHorizontal("", skin6, GUILayout.Width(430));

			GUILayout.Label("Show Frame Rate ", skin2, GUILayout.Width(180), GUILayout.Height(36));
			GUILayout.Space(50);

			if (GUILayout.Button(FPS[FPSstatus], skin5, GUILayout.Width(140), GUILayout.Height(27)))
			{
				if (FPSstatus == 1)
				{
					showFrameRate = 1;
					FPSstatus = 0;
				}
				else
				{
					showFrameRate = 0;
					FPSstatus = 1;
				}
			}

			GUILayout.EndHorizontal();

			/*
				
				GUILayout.BeginHorizontal("", skin6, GUILayout.Width(430));
					
					GUILayout.Label("Other Options ", skin2, GUILayout.Width(180), GUILayout.Height(40));
					GUILayout.Space(20);
					if (GUILayout.Button("", skin3, GUILayout.Width(30), GUILayout.Height(30))){
						
					}
						
						if(GUILayout.Button("Custom", skin5, GUILayout.Width(140), GUILayout.Height(30))){
							
						}
						
					if (GUILayout.Button("", skin4, GUILayout.Width(30), GUILayout.Height(30))){
						
					}
					
				
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal("", skin6, GUILayout.Width(430));
					
					GUILayout.Label("Other Options ", skin2, GUILayout.Width(180), GUILayout.Height(40));
					GUILayout.Space(20);
					if (GUILayout.Button("", skin3, GUILayout.Width(30), GUILayout.Height(30))){
						
					}
						
						if(GUILayout.Button("Custom", skin5, GUILayout.Width(140), GUILayout.Height(30))){
							
						}
						
					if (GUILayout.Button("", skin4, GUILayout.Width(30), GUILayout.Height(30))){
						
					}
					
				
				GUILayout.EndHorizontal();
			
				GUILayout.BeginHorizontal("", skin6, GUILayout.Width(430));
					
					GUILayout.Label("Other Options ", skin2, GUILayout.Width(180), GUILayout.Height(40));
					GUILayout.Space(20);
					if (GUILayout.Button("", skin3, GUILayout.Width(30), GUILayout.Height(30))){
						
					}
						
						if(GUILayout.Button("Custom", skin5, GUILayout.Width(140), GUILayout.Height(30))){
							
						}
						
					if (GUILayout.Button("", skin4, GUILayout.Width(30), GUILayout.Height(30))){
						
					}
					
				
				GUILayout.EndHorizontal();
			
				GUILayout.BeginHorizontal("", skin6, GUILayout.Width(430));
					
					GUILayout.Label("Other Options ", skin2, GUILayout.Width(180), GUILayout.Height(40));
					GUILayout.Space(20);
					if (GUILayout.Button("", skin3, GUILayout.Width(30), GUILayout.Height(30))){
						
					}
						
						if(GUILayout.Button("Custom", skin5, GUILayout.Width(140), GUILayout.Height(30))){
							
						}
						
					if (GUILayout.Button("", skin4, GUILayout.Width(30), GUILayout.Height(30))){
						
					}
					
				
				GUILayout.EndHorizontal();
			
			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUILayout.EndArea();
			//CONSOLE		
		}
		else if (window == 3)
		{
			showTip = false;
			GUILayout.BeginArea(new Rect(20, 30, 468, 265), "");
			GUILayout.BeginVertical("", GUILayout.Height(265));
			scrollPosition = GUILayout.BeginScrollView(scrollPosition);
			foreach (ConsoleEntryFps entry in entries)
			{
				GUILayout.BeginHorizontal();
				GUI.color = Color.green;
				GUILayout.Label("" + entry.text + "");
				GUI.color = Color.yellow;
				GUILayout.Label(entry.note + " ");
				GUI.color = Color.cyan;
				GUILayout.Label(entry.text2);
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.Space(3);
			}

			GUILayout.EndScrollView();
			GUILayout.EndVertical();
			GUI.color = Color.white;
			GUILayout.EndArea();
			GUILayout.FlexibleSpace();

			if (Event.current.type == EventType.KeyDown && Event.current.character == char.Parse("\n") && inputField.Length > 0)
			{
				StartCoroutine(ApplyVariable(inputField, value));
				inputField = "";
				value = "";
			}

			GUILayout.BeginHorizontal("", skin7);
			inputField = GUILayout.TextField(inputField, GUILayout.Width(320));
			GUILayout.FlexibleSpace();
			GUILayout.Label(" >> ");
			GUILayout.FlexibleSpace();
			value = GUILayout.TextField(value, GUILayout.Width(90));
			GUILayout.EndHorizontal();

		}
		else if (window == 4)
		{
			showTip = false;
			//CREDITS
			GUI.DrawTexture(new Rect(10, 29, 480, 299), tex4);

			//GUILayout.BeginArea (new Rect (40,30,468,265), "");
			//Text here
			//GUILayout.EndArea ();

		}
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
		scrollPosition.y = 100000;

		if (str == variable[0])
		{ //bullet speed
			entries.Add(entry);
			if (float.TryParse(value, out newValue))
			{
				playerweapons.weaponsInUse[playerweapons.currentWeapon].weapon.bulletSpeed = newValue;
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
				playerweapons.weaponsInUse[playerweapons.currentWeapon].weapon.numberOfClips = (int)newValue;
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
			grenades.explType = Exposives.GRENADES;
			grenades.grenadeCount = 3;
			entry.note = " - Explosives changed to ";
			entry.text2 = "Grenades";
			entries.Add(entry);

		}
		else if (str == variable[8])
		{ //claymore	
			grenades.explType = Exposives.CLAYMORE;
			grenades.grenadeCount = 3;
			entry.note = " - Explosives changed to ";
			entry.text2 = "Claymore";
			entries.Add(entry);

		}
		else if (str == variable[9])
		{ //sticky
			grenades.explType = Exposives.STICKY;
			grenades.grenadeCount = 3;
			entry.note = " - Explosives changed to Sticky ";
			entry.text2 = "Sticky";
			entries.Add(entry);

		}
		else if (str == variable[10])
		{ //cash
			if (float.TryParse(value, out newValue))
			{
				scoreManager.addScore((int)newValue);
				entry.note = " Cash + ";
				entry.text2 = newValue.ToString();
				entries.Add(entry);
			}
		}
		else if (str == variable[11])
		{ //cash
			codControllerHealth.ApplyDamage(500);
		}
		else if (str == variable[12])
		{ //commands
			entry = new ConsoleEntryFps();
			entries.Add(entry);
			entry.text = "List of available commands:";

			for (int c = 0; c < variable.Length - 1; c++){
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
				codControllerHealth.hitPoints = newValue;
				codControllerHealth.maximumHitPoints = newValue;
				entry.note = " - update health to ";
				entry.text2 = newValue.ToString();
			}
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

	////SKILLS

	public void UpdateSkills(int index)
	{

		if (index == 0)
		{
			codController.runSpeed += 1;
		}
		else if (index == 1)
		{
			codController.baseGravity -= 1;
		}
		else if (index == 2)
		{
			codControllerHealth.regenerationSpeed += 1;
		}
		else if (index == 3)
		{
			codController.jumpSpeed += 1;
		}
		else if (index == 4)
		{
			codControllerHealth.maximumHitPoints += 10;
		}
		else if (index == 5)
		{
			codControllerHealth.protectFromDamage += 1;
		}
		else if (index == 6)
		{
			codController.airControl = true;
		}
	}

	//Message from skull (when you pick up SP)
	public void AddPoints()
	{
		skillPoints += 20;
	}
	public void HealthPoints()
	{
		codControllerHealth.hitPoints += 25;
		codControllerHealth.maximumHitPoints += 25;
	}
	public void WeaponAmmo()
	{
		playerweapons.weaponsInUse[playerweapons.currentWeapon].weapon.numberOfClips += 30;
		//playerweapons.weaponsInUse[playerweapons.weaponName].weapon.numberOfClips += 30;
		//weaponscript.fireMode.[weaponscript.Launcher];
	}*/
}
