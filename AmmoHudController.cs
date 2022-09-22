using UnityEngine;
using UnityEngine.UI;

public class AmmoHudController : MonoBehaviour
{
    InputController inputController;
    public Text CurrentAmmoDisplay;
    public Text AmmoReserveDisplay;
    private int AmmoToDisplay = 0;
    private int reservesToDisplay = 0;

    // Start is called before the first frame update
    void Start()
    {
        inputController = GameObject.Find("Player").GetComponentInChildren<InputController>();
    }

    // Update is called once per frame
    void Update()
    {
        AmmoDisplay();
        ReserveDisplay();
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

    #region Ammo reserves
    void ReserveDisplay()
    {
        reservesToDisplay = inputController.ammoReserves;
        AmmoReserveDisplay.text = reservesToDisplay.ToString();
    }
    #endregion

}

