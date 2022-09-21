using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ManagerScore : MonoBehaviour {

	private float baseSize = 40.0f;
	[HideInInspector]
	public float scoreSize;
	[HideInInspector]
	public float secondsToShow = 2f;
	[HideInInspector]
	public float tempTimeToShow;
	[HideInInspector]
	public bool show = false;
	//[HideInInspector]
	public float currentScore = 0f;
	[HideInInspector]
	public float tempScore;
	[HideInInspector]
	public int pointToAdd;
	private float tempSpeed;
	[HideInInspector]
	public float alpha;
	//public GUISkin mySkin;
	public AudioClip rewardSound;
	public AudioClip hitSound;
	public AudioClip buySound;
	public AudioSource aSource;

	//HIT CROSSHAIR VARIABLE
	private float time = 0.0f;
	[HideInInspector]
	public int crosshairSize = 40;
	public Texture hitCrosshairTexture;
	internal float alphaHit;
	[HideInInspector]
	public bool updatingScore;
	
	private float waitTime = 0.0f;
	private float finalScore;
	private int killedEnemies = 0;
	public AudioClip[] UTKillSounds;
	public AudioSource quakeASource;
	public int roundNR = 0;

	void Start()
	{
		scoreSize = baseSize;
	}

	void Update()
	{
		if (currentScore < tempScore)
		{
			updatingScore = true;
			float multiplier = tempSpeed / 3;
			currentScore += multiplier * Time.deltaTime;
			if (currentScore >= tempScore)
			{
				currentScore = tempScore;
				tempSpeed = 0.0f;
			}
		}
		else
		{
			updatingScore = false;
		}

		if (show)
		{
			if (tempTimeToShow > 0)
			{
				tempTimeToShow -= Time.deltaTime;
				if (tempTimeToShow > secondsToShow - 0.3f)
				{       //how many seconds texture will grow (change number)
					scoreSize += Time.deltaTime * 200;          //texture grow speed
				}
				else if (scoreSize > baseSize)
				{
					scoreSize -= Time.deltaTime * 100;
				}
			}
			else
			{
				show = false;
				scoreSize = baseSize; //Reset texture size back to normal
			}

			if (scoreSize > baseSize + 20)
			{
				scoreSize = baseSize + 20;
			}
			//Fading out texture
			alpha = tempTimeToShow;
		}

		if (waitTime > 0.0f)
		{
			CalcFinalScore();
		}

		//HIT CROSSHAIR
		if (time > 0)
		{
			time -= Time.deltaTime;
		}
		alphaHit = time;
	}

	public void DecreaseScore(int amount)
	{
		tempScore -= amount;
		currentScore -= amount;
		GetComponent<AudioSource>().clip = buySound;
		GetComponent<AudioSource>().Play();
	}

	public void DrawCrosshair()
	{
		GetComponent<AudioSource>().clip = hitSound;
		GetComponent<AudioSource> ().volume = 0.3f;
		GetComponent<AudioSource> ().Play();
		time = 1.0f;
	}

	public void addScore(int value)
	{
		waitTime = 1.0f;
		finalScore += value;
		killedEnemies++;
	}

	public void CalcFinalScore()
	{
		waitTime -= Time.deltaTime;

		if (waitTime <= 0.0f)
		{
			AddFinalScore();
		}
	}

	public void AddFinalScore()
	{
		int bonus = 0;
		if (killedEnemies > 1) bonus = killedEnemies * 100; //if you kill more then 1 enemy, you will get bonus(100$) for every killed enemy.

		show = true;
		aSource.clip = rewardSound;
		aSource.volume = 1.0f;
		aSource.Play();
		tempSpeed += finalScore + bonus;
		tempTimeToShow = secondsToShow;
		tempScore += finalScore + bonus;
		pointToAdd = (int)(finalScore) + bonus;
		finalScore = 0;
		if (killedEnemies >= 2)
		{
			quakeASource.clip = UTKillSounds[killedEnemies - 2];
			quakeASource.volume = 0.7f;
			quakeASource.Play();
		}
		killedEnemies = 0;
	}



	/*public void OnGUI()
	{
		GUI.skin = mySkin;
		var style1 = mySkin.customStyles[0];
		var style2 = mySkin.customStyles[1];
		//var style3 = mySkin.customStyles[2];
		var style4 = mySkin.customStyles[3];
		var style5 = mySkin.customStyles[5];


		GUI.Label(new Rect(110, Screen.height - 155, 100, 60), roundNR.ToString("F0"), style2);
		GUI.Label(new Rect(0, Screen.height - 155, 100, 60), "Round: ", style4);

		GUI.Label(new Rect(110, Screen.height - 115, 100, 60), "" + currentScore.ToString("F0"), style2);
		GUI.Label(new Rect(0, Screen.height - 113, 100, 60), "Score: ", style4);

		if (show)
		{
			style1.fontSize = (int)scoreSize;
			style5.fontSize = (int)scoreSize;

			GUI.Label(new Rect(Screen.width / 2 - 85, Screen.height / 2 - Screen.height / 3, 160, 60), "+" + pointToAdd, style5);
			GUI.color = new Color(1.0f, 1.0f, 1.0f, alpha); //Color (r,g,b,a)
			GUI.Label(new Rect(Screen.width / 2 - 85, Screen.height / 2 - Screen.height / 3, 160, 60), "+" + pointToAdd, style1);

		}
		GUI.color = new Color(1.0f, 1.0f, 1.0f, alphaHit);
		GUI.DrawTexture(new Rect(Screen.width / 2 - crosshairSize / 2, Screen.height / 2 - crosshairSize / 2, crosshairSize, crosshairSize), hitCrosshairTexture);

	}*/
}
