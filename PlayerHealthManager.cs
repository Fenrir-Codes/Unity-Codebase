using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    [Header("Player health amount")]
    public float PlayerHealth = 100;
    private float MaxHealth = 100f;
    public bool isGameOver = false;
    public Text PlayerHealthText;
    // Start is called before the first frame update
    void Start()
    {
        isGameOver = false;
        PlayerHealth = MaxHealth;
    }

    private void Update()
    {
        PlayerHealthText.text = PlayerHealth.ToString();
    }

    public void TakeDamage(float damagetaken)
    {
        PlayerHealth -= damagetaken;
        Debug.Log("Damage -> " + damagetaken);
        if (PlayerHealth <= 0f)
        {
            isGameOver = true;
        }
    }

}
