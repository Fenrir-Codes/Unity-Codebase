using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class DynamicCrosshairNew : MonoBehaviour {

	public bool showCrosshair = true;
	private HUDManager HUD;
    public Texture2D crosshairTexture;
	public Texture2D backTexture;
	public Color32 Color;
    
    //Size of boxes
    public float cLength = 10f;
    public float cWidth = 3f;
	public float borderSize = 0.4f;
	public float SizeRoot;
 
    //Rotation
    //private float rotAngle = 0.0f;
	[HideInInspector]
	public float spread;
	public int crosshairType = 0;

	private void Start()
	{
		if (HUD == null && FindObjectOfType<HUDManager>() != null)
			HUD = FindObjectOfType<HUDManager>();
	}

	public void Update()
	{
		if (showCrosshair)
		{
			var crossSize = spread * (Screen.height - spread) / 600f - borderSize;

			HUD.RootCross.gameObject.SetActive(true);

			HUD.CrossUp.localPosition = new Vector3(0f, crossSize, 0f);
			HUD.CrossDown.localPosition = new Vector3(0f, -crossSize, 0f);
			HUD.CrossRight.localPosition = new Vector3(crossSize, 0f, 0f);
			HUD.CrossLeft.localPosition = new Vector3(-crossSize, 0f, 0f);

			HUD.CrossUp.sizeDelta = new Vector2(cWidth, cLength);
			HUD.CrossDown.sizeDelta = new Vector2(cWidth, cLength);
			HUD.CrossRight.sizeDelta = new Vector2(cLength, cWidth);
			HUD.CrossLeft.sizeDelta = new Vector2(cLength, cWidth);

			HUD.CrossUp.GetComponent<Image>().color = Color;
			HUD.CrossDown.GetComponent<Image>().color = Color;
			HUD.CrossRight.GetComponent<Image>().color = Color;
			HUD.CrossLeft.GetComponent<Image>().color = Color;

			HUD.RootCross.localScale = new Vector3(SizeRoot, SizeRoot, SizeRoot);
		}
		else
			HUD.RootCross.gameObject.SetActive(false);
	}

	/*public void OnGUI()
	{

		var crossGUI = new GUIStyle();
		var backgGUI = new GUIStyle();
		crossGUI.normal.background = crosshairTexture;
		backgGUI.normal.background = backTexture;
		Vector2 pivot = new Vector2(Screen.width / 2, Screen.height / 2);

		if (showCrosshair)
		{
			if (crosshairType == 0)
			{
				//Draw border
				GUI.Box(new Rect((Screen.width - cWidth - borderSize) / 2, (Screen.height - spread - borderSize) / 2 - cLength, cWidth + borderSize, cLength + borderSize), "", backgGUI);
				GUI.Box(new Rect((Screen.width - cWidth - borderSize) / 2, (Screen.height + spread - borderSize) / 2, cWidth + borderSize, cLength + borderSize), "", backgGUI);
				GUI.Box(new Rect((Screen.width - spread - borderSize) / 2 - cLength, (Screen.height - cWidth - borderSize) / 2, cLength + borderSize, cWidth + borderSize), "", backgGUI);
				GUI.Box(new Rect((Screen.width + spread - borderSize) / 2, (Screen.height - cWidth - borderSize) / 2, cLength + borderSize, cWidth + borderSize), "", backgGUI);

				//Horizontal
				GUI.Box(new Rect((Screen.width - cWidth) / 2, (Screen.height - spread) / 2 - cLength, cWidth, cLength), "", crossGUI);
				GUI.Box(new Rect((Screen.width - cWidth) / 2, (Screen.height + spread) / 2, cWidth, cLength), "", crossGUI);
				//Vertical
				GUI.Box(new Rect((Screen.width - spread) / 2 - cLength, (Screen.height - cWidth) / 2, cLength, cWidth), "", crossGUI);
				GUI.Box(new Rect((Screen.width + spread) / 2, (Screen.height - cWidth) / 2, cLength, cWidth), "", crossGUI);
			}
			else if (crosshairType == 1)
			{

				GUI.Box(new Rect((Screen.width - 2 - borderSize) / 2, (Screen.height - spread - borderSize) / 2 - 14, 2 + borderSize, 14 + borderSize), "", backgGUI);
				GUI.Box(new Rect((Screen.width - 2) / 2, (Screen.height - spread) / 2 - 14, 2, 14), "", crossGUI);
				GUIUtility.RotateAroundPivot(45, pivot);
				GUI.Box(new Rect((Screen.width + spread - borderSize) / 2, (Screen.height - borderSize) / 2, 14 + borderSize, 2 + borderSize), "", backgGUI);
				GUI.Box(new Rect((Screen.width + spread) / 2, (Screen.height - 2) / 2, 14, 2), "", crossGUI);
				GUIUtility.RotateAroundPivot(0, pivot);
				GUI.Box(new Rect((Screen.width - 2 - borderSize) / 2, (Screen.height + spread - borderSize) / 2, 2 + borderSize, 14 + borderSize), "", backgGUI);

				GUI.Box(new Rect((Screen.width - 2) / 2, (Screen.height + spread) / 2, 2, 14), "", crossGUI);
			}
		}
	}*/
}
