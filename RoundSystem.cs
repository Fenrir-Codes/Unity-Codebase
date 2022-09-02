using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RoundSystem : MonoBehaviour
{
    [HideInInspector] public int roundNo = 0;
    private float waitForRoundEndSFX = 9f;  // 9 seconds for wait the ensing round sound effect
    private int rewardOnCompletedRound = 550; //giving 300 dollars on a completed round

    private float slowDownFactor = 0.05f;
    private float slowDowndLength = 5f;

    AudioSource SFX;
    [Header("Audio settings")]
    [Tooltip("The start and end effet audio clips")]
    public AudioClip roundStartClip;
    public AudioClip roundEndClip;

    [Header("Booleans")]
    [Tooltip("Showing canSpawn and CanStartWave values")]
    public bool canSpawn;
    public bool canStartWave;

    [Header("GUI setting")]
    [Tooltip("Text for the current round")]
    public Text currentRoundUI; //GUI text (showing number of the current round)
    public Text RemainingZUI;
    public Text killedZombiesEndgameUI;

    //SpawnController reference
    SpawnController spawnCtrl;
    PauseMenu pausemenu;
    EndGameMenu endGameMenu;

    public int killCounter = 0;

    // Start is called before the first frame update
    void Start()
    {

        pausemenu = GameObject.Find("PauseGameCanvas").GetComponent<PauseMenu>();
        endGameMenu = GameObject.Find("EndGameCanvas").GetComponent<EndGameMenu>();
        spawnCtrl = GetComponent<SpawnController>();
        SFX = GetComponent<AudioSource>();

        SFX.clip = roundStartClip;
        SFX.Play();

        roundNo = 1;
        canSpawn = true;

        StartCoroutine(spawnCtrl.Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        currentRoundUI.text = roundNo.ToString();

        GameObject[] zombiesInScene = GameObject.FindGameObjectsWithTag("Enemy");
        RemainingZUI.text = spawnCtrl.staringSpawnAmount.ToString();

        if (spawnCtrl.zombiesToSpawn == 0 && !canSpawn && canStartWave && zombiesInScene.Length == 0)
        {
            //extra 100 money on a completed round
            MoneyDisplayController money = GameObject.Find("MoneyDisplay").GetComponent<MoneyDisplayController>();
            money.GiveMoney(rewardOnCompletedRound);

            spawnCtrl.staringSpawnAmount++;
            spawnCtrl.staringSpawnAmount += Random.Range(2, 6);
            rewardOnCompletedRound += 50;
            StartCoroutine(NextRound());

        }

        if (!pausemenu.isPaused)
        {
            Time.timeScale += (1f / slowDowndLength) * Time.unscaledDeltaTime;
            Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        }
        if (endGameMenu.isGameOver)
        {
            Time.timeScale = 0f;
            endGameMenu.survivedRounds.text = roundNo.ToString();
        }


    }

    public IEnumerator NextRound()
    {
        SFX.clip = roundEndClip;
        SFX.Play();

        canStartWave = false;
        BulletTimeEffectOnEndingRound();
        yield return new WaitForSeconds(waitForRoundEndSFX);

        roundNo++;
        spawnCtrl.zombiesToSpawn = spawnCtrl.staringSpawnAmount;
        //Debug.Log("amount of Z: "+spawnCtrl.zombiesToSpawn);

        SFX.clip = roundStartClip;
        SFX.Play();

        canSpawn = true;
        //canStartWave = true;
        StartCoroutine(spawnCtrl.Spawn());
    }

    void BulletTimeEffectOnEndingRound()
    {
        Time.timeScale = slowDownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }
}
