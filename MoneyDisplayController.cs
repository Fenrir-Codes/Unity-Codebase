using UnityEngine;
using UnityEngine.UI;

public class MoneyDisplayController : MonoBehaviour
{
    //[SerializeField] private MoneyManager moneyManager;
    // [HideInInspector]
    public int MyMoney = 0;

    public Text moneyDisplay;

    // Start is called before the first frame update
    void Start()
    {
        MyMoney = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (MyMoney < 0)
        {
            MyMoney = 0;
        }
        moneyDisplay.text = $"{MyMoney}";
    }

    public void GiveMoney(int money)
    {
        MyMoney += money;
        //Debug.Log("added : "+MyMoney);
    }

    public void TakeMoney(int money)
    {
        MyMoney -= money;
    }
}
