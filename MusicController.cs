using System.Runtime.CompilerServices;
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
    private int music;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        pauseMenu = GameObject.Find("PauseGameCanvas").GetComponent<PauseMenu>();
        inputController = GameObject.FindGameObjectWithTag("WeaponHolder").GetComponent<InputController>();
        RandomizeAudioClip();
    }

    // Update is called once per frame
    void Update()
    {
        PlayMusic();
    }

    #region play the music
    private void PlayMusic()
    {
        int i = 0;
        if (pauseMenu.isPaused)
        {
            musicPlayer.Pause();
        }
        else
        {
            musicPlayer.UnPause();

            if (inputController.activeWeapon != LMG && inputController.activeWeapon != Minigun)
            {
                i++;
                PlayRandomFromList(i);
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

    #region ranomize clip on start
    private void RandomizeAudioClip()
    {
        music = Random.Range(0, audioClips.Length);
        musicPlayer.clip = audioClips[music];
    }
    #endregion

    #region play random from array
    private void PlayRandomFromList(int i)
    {
        musicPlayer.volume = 0.3f;
        musicPlayer.clip = audioClips[music];
        if (!musicPlayer.isPlaying)
        {
            musicPlayer.Play();
        }

        if (i >= audioClips.Length && !musicPlayer.isPlaying)
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
