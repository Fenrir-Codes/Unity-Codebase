using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode()]
public class MiniMap : MonoBehaviour
{
	public enum posOnScreen { TopLeft, BottomLeft }
	public posOnScreen placement = posOnScreen.TopLeft;

	public float minimapSize = 2.0f;
	public float offsetX = 10.0f;
	public float offsetY = 10.0f;
	private float adjustSize = 0.0f;


	public Texture borderTexture;
	public Texture effectTexture;
	public Camera minimapCamera;

	void Update()
	{
		if (minimapCamera == null) return;

		adjustSize = Mathf.RoundToInt(Screen.width / 10);
		if (placement == posOnScreen.TopLeft)
		{
			minimapCamera.pixelRect = new Rect(offsetX, (Screen.height - (minimapSize * adjustSize)) - offsetY, minimapSize * adjustSize, minimapSize * adjustSize);
		}
		else if (placement == posOnScreen.BottomLeft)
		{
			minimapCamera.pixelRect = new Rect(offsetX, offsetY, minimapSize * adjustSize, minimapSize * adjustSize);
		}
	}


	void OnGUI()
	{
		if (borderTexture != null)
		{
			minimapCamera.Render();
			if (placement == posOnScreen.TopLeft)
			{
				GUI.DrawTexture(new Rect(offsetX, offsetY, minimapSize * adjustSize, minimapSize * adjustSize), effectTexture);
				GUI.DrawTexture(new Rect(offsetX, offsetY, minimapSize * adjustSize, minimapSize * adjustSize), borderTexture);
			}

			if (placement == posOnScreen.BottomLeft)
			{
				GUI.DrawTexture(new Rect(offsetX, (Screen.height - (minimapSize * adjustSize)) - offsetY, minimapSize * adjustSize, minimapSize * adjustSize), effectTexture);
				GUI.DrawTexture(new Rect(offsetX, (Screen.height - (minimapSize * adjustSize)) - offsetY, minimapSize * adjustSize, minimapSize * adjustSize), borderTexture);
			}
		}
	}
}
