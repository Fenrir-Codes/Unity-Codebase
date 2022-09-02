using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingBar;
    public ParticleSystem mist;
    //public ParticleSystem mistOnLoading;
    public Text loadingText;


    private void Start()
    {
        mist.Play();
    }

    public void PlayGame(int levelIndex)
    {
        // get the first map (demo map)
        StartCoroutine(LoadLevel(levelIndex));
        Time.timeScale = 1f; // time set back to normal
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(levelIndex);
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingText.text = (int)(progress * 100f) + "%";
            loadingBar.value = operation.progress;
            yield return null;
        }
    }


}
