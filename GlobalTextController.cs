using System.Collections;
using TMPro;
using UnityEngine;


public class GlobalTextController : MonoBehaviour
{
    AmmoBoxController ammoController;
    public GameObject globalTextUI;
    public TextMeshProUGUI textToShow;
    public string setText = "";
    public bool showText;

    // Start is called before the first frame update
    void Start()
    {
        globalTextUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (showText)
        {
            StartCoroutine(showPopUpText());
        }
    }

    IEnumerator showPopUpText()
    {
        globalTextUI.SetActive(true);
        textToShow.text = setText;
        yield return new WaitForSeconds(2.5f);
        globalTextUI.SetActive(false);
        showText = false;
        setText = "";
    }
}
