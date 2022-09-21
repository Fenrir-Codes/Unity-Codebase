using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsOption : MonoBehaviour
{
	public Text OptionName;
	public Text SettingCurrent;
	public Button Submit;
	public enum Settings { Quality, Resolution, AntiAliasing, AnisotropicFiltering, Fullsreen, ShowFrameRate }
	public Settings _Settings;
	private HUDManager HUD;

	void Start()
	{
		HUD = FindObjectOfType<HUDManager>();
	}

	void Update()
	{
		if(HUD != null)
		{
			switch(_Settings)
			{
				case Settings.Quality:
					OptionName.text = "Quality";
					HUD.Qualnames = QualitySettings.names;
					SettingCurrent.text = HUD.Qualnames[HUD.qual];
					Submit.onClick.AddListener(() => QualitySettings.SetQualityLevel(HUD.qual, false));
					break;

				case Settings.Resolution:
					OptionName.text = "Resolution ";
					HUD.resolutions = Screen.resolutions;
					SettingCurrent.text = string.Format("{0}x{1}", HUD.resolutions[HUD.res].width, HUD.resolutions[HUD.res].height);
					Submit.onClick.AddListener(() => Screen.SetResolution(HUD.resolutions[HUD.res].width, HUD.resolutions[HUD.res].height, false));
					break;

				case Settings.AntiAliasing:
					OptionName.text = "AntiAliasing ";
					SettingCurrent.text = HUD.Aliasing[HUD.Antistatus];
					break;

				case Settings.AnisotropicFiltering:
					OptionName.text = "Anisotropic Filtering ";
					SettingCurrent.text = HUD.Anisotropic[HUD.Filtering];
					break;

				case Settings.Fullsreen:
					OptionName.text = "Fullsreen ";
					SettingCurrent.text = HUD.Fullscreen[HUD.Fullscreenstatus];
					break;

				case Settings.ShowFrameRate:
					OptionName.text = "Show Frame Rate ";
					SettingCurrent.text = HUD.FPS[HUD.FPSstatus];
					break;
			}
		}
	}

	public void ChangeAntiAliasing()
	{
		if (HUD.Antistatus == 0)
		{
			QualitySettings.antiAliasing = 0;
		}
		else if (HUD.Antistatus == 1)
		{
			QualitySettings.antiAliasing = 2;
		}
		else if (HUD.Antistatus == 2)
		{
			QualitySettings.antiAliasing = 4;
		}
		else if (HUD.Antistatus == 3)
		{
			QualitySettings.antiAliasing = 8;
		}
	}

	public void ChangeAnisotropicFiltering()
	{
		if (HUD.Filtering == 0)
		{
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
		}
		else if (HUD.Filtering == 1)
		{
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
		}
		else if (HUD.Filtering == 2)
		{
			QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
		}
	}

	public void ChangeFullScreen()
	{
		if (HUD.Fullscreenstatus == 1)
		{
			Screen.fullScreen = true;
			HUD.Fullscreenstatus = 0;
		}
		else
		{
			Screen.fullScreen = false;
			HUD.Fullscreenstatus = 1;
		}
	}

	public void ChangeFrameRate()
	{
		if (HUD.FPSstatus == 1)
		{
			HUD.showFrameRate = 1;
			HUD.FPSstatus = 0;
		}
		else
		{
			HUD.showFrameRate = 0;
			HUD.FPSstatus = 1;
		}
	}

	public void _Left()
	{
		switch (_Settings)
		{
			case Settings.Quality:
				if (HUD.qual < HUD.Qualnames.Length)
				{
					HUD.qual--;
					if (HUD.qual < 0) HUD.qual = HUD.Qualnames.Length - 1;
				}
				break;

			case Settings.Resolution:
				if (HUD.res < HUD.resolutions.Length)
				{
					HUD.res--;
					if (HUD.res < 0) HUD.res = HUD.resolutions.Length - 1;
				}
				break;

			case Settings.AntiAliasing:
				HUD.Antistatus--;
				if (HUD.Antistatus < 0) HUD.Antistatus = 4 - 1;
				break;

			case Settings.AnisotropicFiltering:
				HUD.Filtering--;
				if (HUD.Filtering < 0) HUD.Filtering = 3 - 1;
				break;
		}
	}

	public void _Right()
	{
		switch (_Settings)
		{
			case Settings.Quality:
				if (HUD.qual < HUD.Qualnames.Length)
				{
					HUD.qual++;
					if (HUD.qual > (HUD.Qualnames.Length - 1)) HUD.qual = 0;
				}
				break;

			case Settings.Resolution:
				if (HUD.res < HUD.resolutions.Length)
				{
					HUD.res++;
					if (HUD.res > (HUD.resolutions.Length - 1)) HUD.res = 0;
				}
				break;

			case Settings.AntiAliasing:
				HUD.Antistatus++;
				if (HUD.Antistatus > (4 - 1)) HUD.Antistatus = 0;
				break;

			case Settings.AnisotropicFiltering:
				HUD.Filtering++;
				if (HUD.Filtering > (3 - 1)) HUD.Filtering = 0;
				break;
		}
	}
}
