using UnityEngine;
using UnityEngine.UI;

public class AmmoHudController : MonoBehaviour
{
    InputController inputController;
    public Text CurrentAmmoDisplay;
    private int AmmoToDisplay = 0;

    // Start is called before the first frame update
    void Start()
    {
        inputController = GameObject.Find("Player").GetComponentInChildren<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        AmmoDisplay();
    }

    #region AmmoDisplay
    void AmmoDisplay()
    {
        AmmoToDisplay = inputController.currentAmmo;
        for (int i = 0; i <= AmmoToDisplay; i++)
        {
            CurrentAmmoDisplay.text = new string('|', i);
        }
    }
    #endregion

}
