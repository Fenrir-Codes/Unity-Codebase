using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SG;

public class ReloadCurrentLevelNew : MonoBehaviour {

	public float secBeforeFade = 3f;
	public float fadeTime = 5f;
	private bool fadeIn = false;
	private float tempTime;
	private float time = 0.0f;
	private HUDManager HUD;

	public IEnumerator Start(){
		if (HUD == null && FindObjectOfType<HUDManager>() != null)
			HUD = FindObjectOfType<HUDManager>();

		HUD.SwitchHUD(StateHUD.Disable);

		yield return new WaitForSeconds(secBeforeFade);
		fadeIn = true;
	}


	public void Update(){

		HUD.DamageReciver.color = new Color(1f, 1f, 1f, 1f);

		if (fadeIn){
			if(time < fadeTime) time += Time.deltaTime;
			tempTime = Mathf.InverseLerp(0.0f, fadeTime, time);
			AudioListener.volume = tempTime;
			HUD.FadeTexture.color = new Color(1f, 1f, 1f, tempTime);
		}
	
		if(tempTime >= 1.0)
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);//Application.LoadLevel(Application.loadedLevel);
	}

	/*public void OnGUI(){
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height), damageTexture);
		if(fadeIn){
			GUI.color.a = tempTime;
			GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), fadeTexture);
		}
	}*/
}
