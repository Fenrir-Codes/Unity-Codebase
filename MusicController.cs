using UnityEngine;

public class MusicController : MonoBehaviour
{
    PauseMenu pauseMenu;
    AudioSource musicPlayer;
    [SerializeField] private AudioClip[] audioClips;

    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = GetComponent<AudioSource>();
        pauseMenu = GameObject.Find("PauseGameCanvas").GetComponent<PauseMenu>();
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

        }



    }
}
