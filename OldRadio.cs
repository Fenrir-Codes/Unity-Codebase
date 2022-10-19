using UnityEngine;

public class OldRadio : MonoBehaviour
{
    PauseMenu pauseMenu;
    public AudioSource Radio;
    [Header("Here goes the conversation canvases objects")]
    public Canvas on_Canvas;
    public Canvas off_Canvas;
    [Header("Music audio clips")]
    [SerializeField] private AudioClip[] audioClips;

    public bool isTurnedOn = false;
    public bool canTurnOn = false;

    // Start is called before the first frame update
    void Start()
    {
        InitializeRadio();
    }

    private void InitializeRadio()
    {
        Radio = GetComponent<AudioSource>();
        pauseMenu = GameObject.Find("PauseGameCanvas").GetComponent<PauseMenu>();
        on_Canvas.enabled = false;
        off_Canvas.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canTurnOn)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isTurnedOn = isTurnedOn ? false : true;
            }
        }

        checkOnOffState();
    }

    #region On Off state
    private void checkOnOffState()
    {
        if (isTurnedOn)
        {
            on_Canvas.enabled = true;
            off_Canvas.enabled = false;
            PlayMusic();
        }
        else
        {
            on_Canvas.enabled = false;
            off_Canvas.enabled = true;
            Radio.Stop();
        }
    }
    #endregion

    #region play the music
    private void PlayMusic()
    {
        int i = 0;
        if (pauseMenu.isPaused)
        {
            Radio.Pause();
        }
        else if (isTurnedOn && !Radio.isPlaying)
        {
            Radio.UnPause();

            i++;
            var music = Random.Range(0, audioClips.Length);
            Radio.clip = audioClips[music];
            Radio.Play();

            if (i >= audioClips.Length)
            {
                Radio.clip = audioClips[i];
                Radio.Play();
            }

        }
    }
    #endregion

    #region ontriggerenter and exit
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canTurnOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {

        canTurnOn = false;
    }
    #endregion

}
