using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnerWithVawesNew : MonoBehaviour {

	//@script ExecuteInEditMode()
	public GameObject[] enemyPrefab;
	public Transform[] spawnPoints;
	private int nextPos;
	private int enemiesInGame;
	//[HideInInspector]
	public bool countdown;
	//[HideInInspector]
	public bool waveInfo;
	public float cTimer;
	public float waveNrTimer;
	public int[] enemiesToInstantiate;
	public AudioClip coundownSound;
	//@HideInInspector
	public int waveNr;
	[HideInInspector]
	public int tempKilledEnemies;
	[HideInInspector]
	public bool allEnemiesInGame;
	public GUISkin mySkin;
	[HideInInspector]
	public bool showRoundCompleted;
	public ManagerScore score;
	public void Update()
	{
		if (countdown)
		{
			Countdown();
		}
		if (waveInfo)
		{
			ShowWaveStarts();
		}
		if (showRoundCompleted)
		{
			waveNrTimer = waveNrTimer - Time.deltaTime;
			if (waveNrTimer <= 3)
			{
				showRoundCompleted = false;
				waveNrTimer = 5.8f;
			}
		}
	}

	public IEnumerator InstantiateEnemy()
	{
		tempKilledEnemies = 0;
		while (enemiesInGame < enemiesToInstantiate[waveNr - 1])
		{
			nextPos = nextPos + 1;
			yield return new WaitForSeconds(1f);
			if (nextPos >= spawnPoints.Length)
			{
				nextPos = 0;
			}
			Transform pos = spawnPoints[nextPos];
			GameObject enemy = enemyPrefab[Random.Range(0, enemyPrefab.Length)];
			//var enemy : GameObject;
			//enemy = enemyPrefab[waveNr - 1];
			enemy.GetComponent<AStarBeastAINew>().backUpPoints = spawnPoints[Random.Range(0, spawnPoints.Length)];
			enemy.gameObject.name = "BeastPrefab";
			GameObject enPrefab = Object.Instantiate(enemy, pos.position, pos.rotation);
			enPrefab.GetComponent<BeastDamageNew>().hitPoints = enPrefab.GetComponent<BeastDamageNew>().hitPoints + (waveNr * 50);
			enemiesInGame++;
			if (enemiesInGame == enemiesToInstantiate[waveNr - 1])
			{
				allEnemiesInGame = true;
				enemiesInGame = enemiesInGame - tempKilledEnemies;
				yield break;
			}
		}
	}

	public void Countdown()
	{
		cTimer = cTimer - Time.deltaTime;
		if (cTimer <= waveNrTimer)
		{
			ShowWaveStarts();
			waveNr++;
			score.roundNR = waveNr;
			if (cTimer > 0f)
			{
				cTimer = 0f;
			}
			countdown = false;
			waveInfo = true;
			PlayAudioClip(coundownSound, transform.position, 1f);
		}
	}

	public void ShowWaveStarts()
	{
		waveNrTimer = waveNrTimer - Time.deltaTime;
		if (waveNrTimer <= 0.5f)
		{
			waveInfo = false;
			waveNrTimer = 5.8f;
			StartCoroutine(InstantiateEnemy());
		}
	}

	public void RecountEnemiesInGame()
	{
		if (allEnemiesInGame)
		{
			enemiesInGame--;
		}
		else
		{
			tempKilledEnemies++;
		}
		if (enemiesInGame == 0)
		{
			if (waveNr == enemiesToInstantiate.Length)
			{
				print("gameComplete");
				//Here you can load Stats or anything else.
				//Application.LoadLevel(Application.loadedLevel);
				SceneManager.LoadScene(SceneManager.GetActiveScene().name);
				return;
			}
			allEnemiesInGame = false;
			cTimer = 30f;
			countdown = true;
			showRoundCompleted = true;
		}
	}

	/*public void OnGUI()
	{
		GUI.skin = mySkin;
		GUIStyle style1 = mySkin.customStyles[0];
		GUIStyle style4 = mySkin.customStyles[3];
		GUIStyle style5 = mySkin.customStyles[4];
		if (countdown)
		{
			GUI.Label(new Rect(Screen.width - 160, 30, 100, 100), "Rest Time : ");
			GUI.Label(new Rect(Screen.width - 50, 30, 100, 100), cTimer.ToString("F2"), style1);
		}
		if (waveInfo)
		{
			GUI.Label(new Rect(Screen.width / 2, (Screen.height / 2) - 150, 100, 100), "" + waveNrTimer.ToString("F0"), style4);
		}
		if (showRoundCompleted)
		{
			GUI.Label(new Rect(Screen.width / 2, 30, 100, 100), (" Round " + waveNr) + " completed", style5);
		}
	}*/

	public AudioSource PlayAudioClip(AudioClip clip, Vector3 position, float volume)
	{
		GameObject go = new GameObject("One shot audio");
		go.transform.position = position;
		AudioSource source = (AudioSource)go.AddComponent(typeof(AudioSource));
		source.clip = clip;
		source.volume = volume;
		source.pitch = Time.timeScale;
		source.Play();
		Object.Destroy(go, clip.length);
		return source;
	}
}
