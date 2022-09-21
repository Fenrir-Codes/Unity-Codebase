using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillItem : MonoBehaviour {

	/*public class SkillConfig
	{
		public Texture skillTexture;
		public string skillName;
		public string skillDscription;
		//@HideInInspector
		public int currentSkillLevel = 0;
		public float maxSkillLevel = 50f;
		public int priceForOneLevel = 1;
	}*/
	
	public Image SkillTexture;
	public Button SkillInfo;
	public Text SkillName;
	public Text LevelSkill;
	public Slider SkillProgress;
	public Button UpgradeSkill;
	public int SkillIndex;
	private HUDManager HUD;

	void Start () {
		HUD = FindObjectOfType<HUDManager>();
	}
	
	void Update () {
		if (HUD != null)
		{
			SkillInfo.onClick.AddListener(() => SkillInfoUpdate());
			SkillTexture.sprite = HUD.Skills[SkillIndex].skillTexture;
			SkillName.text = HUD.Skills[SkillIndex].skillName;
			LevelSkill.text = string.Format("LVL  {0} / {1}", HUD.Skills[SkillIndex].currentSkillLevel, HUD.Skills[SkillIndex].maxSkillLevel);
			SkillProgress.value = HUD.Skills[SkillIndex].currentSkillLevel;
			SkillProgress.maxValue = HUD.Skills[SkillIndex].maxSkillLevel;
			//UpgradeSkill.onClick.AddListener(() => UpgradeSkills(SkillIndex));
		}
	}

	void SkillInfoUpdate()
	{
		HUD.SkillInfo.gameObject.SetActive(true);
		HUD.SkillInfoTexture.sprite = HUD.Skills[SkillIndex].skillTexture;
		HUD.SkillDescription.text = HUD.Skills[SkillIndex].skillDscription;
	}

	public void UpgradeSkills()
	{
		if (HUD.Skills[SkillIndex].currentSkillLevel < HUD.Skills[SkillIndex].maxSkillLevel && HUD.skillPoints >= 0)
		{
			if (HUD.Skills[SkillIndex].priceForOneLevel <= HUD.skillPoints)
			{
				HUD.Skills[SkillIndex].currentSkillLevel += 1;
				HUD.skillPoints -= HUD.Skills[SkillIndex].priceForOneLevel;
				HUD.UpdateSkills(SkillIndex);
			}
			else
			{
				HUD.showNotEnoughPoints = true;

				if (HUD.showNotEnoughPoints)
					HUD.PointsWarning.text = string.Format("Points required: {0}  |  Points available: {1}", HUD.Skills[SkillIndex].priceForOneLevel, HUD.skillPoints);
			}
		}
	}
}
