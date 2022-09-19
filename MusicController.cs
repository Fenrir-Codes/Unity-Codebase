using Unity.VisualScripting;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    InputController inputController;
    PauseMenu pauseMenu;
    public AudioSource musicPlayer;
    [Header("Music audio clips")]
    [SerializeField] private AudioClip[] audioClips;
    [Header("Big Fucking Guns")]
    public AudioClip BFGforLMG;
    public AudioClip BFGforMinigun;

    private int LMG = 2;
    private int Minigun = 4;
    private int i;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        pauseMenu = GameObject.Find("PauseGameCanvas").GetComponent<PauseMenu>();
        inputController = GameObject.FindGameObjectWithTag("WeaponHolder").GetComponent<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayMusic();
    }

    #region play the music
    private void PlayMusic()
    {
        if (pauseMenu.isPaused)
        {
            musicPlayer.Pause();
        }
        else
        {
            musicPlayer.UnPause();
            i = 0;

            if (inputController.activeWeapon != LMG | inputController.activeWeapon != Minigun)
            {
                PlayRandomFromList();
            }
            if (inputController.activeWeapon == LMG)
            {
                PlayLMGClip();
            }
            if (inputController.activeWeapon == Minigun)
            {
                PlayMinigunClip();
            }

        }
    }
    #endregion

    #region play random from array
    private void PlayRandomFromList()
    {
        i++;
        if (!musicPlayer.isPlaying)
        {
            var music = Random.Range(i, audioClips.Length);
            musicPlayer.clip = audioClips[music];
            musicPlayer.volume = 0.3f;
            musicPlayer.Play();
        }
        if (i >= audioClips.Length)
        {
            musicPlayer.clip = audioClips[i];
            musicPlayer.Play();
        }
    }
    #endregion

    #region Play big fucking gun clip for LMG
    private void PlayLMGClip()
    {
        musicPlayer.clip = BFGforLMG;
        musicPlayer.volume = 1f;
        if (!musicPlayer.isPlaying)
        {
            musicPlayer.Play();
        }
    }
    #endregion

    #region Play big fucking gun clip for Minigun
    private void PlayMinigunClip()
    {
        musicPlayer.clip = BFGforMinigun;
        musicPlayer.volume = 1f;
        if (!musicPlayer.isPlaying)
        {
             musicPlayer.Play();
        }
    }
    #endregion
}
