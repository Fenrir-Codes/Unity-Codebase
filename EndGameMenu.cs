using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndGameMenu : MonoBehaviour
{
    public GameObject EndGameMenuUI;
    public GameObject HUD;
    public GameObject Crosshair;
    PlayerHealthManager player;
    public Text survivedRounds;

    public bool isGameOver;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerHealthManager>();
        isGameOver = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGameOver)
        {
            isGameOver = true;
            EndScreen();
        }
    }

    public void EndScreen()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        Crosshair.SetActive(false);
        HUD.SetActive(false);
        EndGameMenuUI.SetActive(true); // activate endGame UI
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene("DemoMap");
    }
}
