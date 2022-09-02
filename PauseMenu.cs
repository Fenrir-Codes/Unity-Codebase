using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool isPaused = false;  // bool for check the game is paused or not
    public GameObject pauseMenuUI; // gameobjet for set the pauseMenu Ui here

    // Update is called once per frame
    void Update()
    {
        //if we hit ESC
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) // check paused state
            {
                Cursor.visible = false;
                Resume(); // if paused  resume
            }
            else
            {
                Pause(); // else pause
            }
        }
    }

    public void Resume()
    {
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudio)
        {
            audio.UnPause();
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenuUI.SetActive(false); // deactivate pause UI
        Time.timeScale = 1f; // time set back to normal
        isPaused = false; // bool to false, game is not paused
    }

    public void Pause()
    {
        AudioSource[] allAudio = FindObjectsOfType<AudioSource>();
        foreach (AudioSource audio in allAudio)
        {
            audio.Pause();
        }

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        pauseMenuUI.SetActive(true); // activate pause UI
        Time.timeScale = 0f; // freeze the time
        isPaused = true; // pase boolean to tru, game is paused
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}

