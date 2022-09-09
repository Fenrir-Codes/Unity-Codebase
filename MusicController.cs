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
        int i = 0;
        if (!pauseMenu.isPaused && !musicPlayer.isPlaying)
        {
            i++;
            var music = Random.Range(0, audioClips.Length);
            musicPlayer.clip = audioClips[music];
            musicPlayer.Play();

            if (i >= audioClips.Length)
            {
                musicPlayer.clip = audioClips[i];
                musicPlayer.Play();
            }

            if (pauseMenu.isPaused)
            {
                musicPlayer.Pause();
            }
            else
            {
                musicPlayer.UnPause();
            }

            //if (inputController.activeWeapon == 4)
            //{
            //    musicPlayer.Stop();
            //    musicPlayer.clip = BFGforMinigun;
            //    musicPlayer.Play();
            //}

        }



    }
}
