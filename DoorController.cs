using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door colliders here. For disable them on opening")]
    public Collider[] Colliders;
    //[Header("The transform of the door. (Not the frame)")]
    //public Transform door;
    [Header("Here goes the buy conversation canvas object")]
    public Canvas showConversationCanvas;
    [Header("Price of opening the door")]
    [SerializeField] private int openingPrice = 20000;
    [Header("Are you in the buy zone?")]
    public bool canPressTheButton;
    [HideInInspector] public bool paid;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        canPressTheButton = false;
        showConversationCanvas.enabled = false;
    }

    #region update
    // Update is called once per frame
    void Update()
    {
        if (canPressTheButton)
        {

            if (Input.GetKeyDown(KeyCode.E))
            {
                MoneyDisplayController money = GameObject.Find("MoneyDisplay").GetComponent<MoneyDisplayController>();
                GlobalTextController textController = GameObject.Find("GlobalTextController").GetComponent<GlobalTextController>();

                if (money.MyMoney >= openingPrice)
                {
 
                    money.MyMoney -= openingPrice;
                    canPressTheButton = false;
                    paid = true;
                    openTheDoor();
                    
                }
                else
                {
                    textController.showText = true;
                    textController.setText = $"Opening price is ${ openingPrice }";
                    canPressTheButton = false;
                    paid = false;
                }

            }

        }
        else
        {
            paid = false;
        }
    }
    #endregion

    #region Ontriggerenter
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canPressTheButton = true;
            showConversationCanvas.enabled = true;
        }
    }
    #endregion

    #region ontriggerexit
    private void OnTriggerExit(Collider other)
    {
        canPressTheButton = false;
        showConversationCanvas.enabled = false;
    }
    #endregion

    #region opening the door function
    void openTheDoor()
    {
        paid = true;
        animator.Play("openDoor");

        foreach (Collider c in Colliders)
        {
            c.GetComponent<Collider>();
            c.enabled = false;
        }

        showConversationCanvas.enabled = false;
        //door.rotation = Quaternion.Euler(new Vector3(0f, -46f, 0f) * Time.deltaTime * smooth);
        //door.Rotate(0f, -85f, 0f * Time.deltaTime * smooth);
    }
    #endregion


}
