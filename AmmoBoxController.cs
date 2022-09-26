using UnityEngine;

public class AmmoBoxController : MonoBehaviour
{

    PauseMenu pauseMenu;
    AudioSource audioSource;
    [Header("Colliders here. For disable them")]
    public Collider[] Colliders;
    [Space]
    [Header("Ammo box settings")]
    public int AmmoInBox = 100;
    [Tooltip("Price of the ammo")]
    public int AmmoPrice = 150;
    [Space]
    [SerializeField] private AudioClip pickupEffect;
    [Space]
    [Header("Are you in the buy zone?")]
    public bool canPressTheButton;


    private void Start()
    {
        canPressTheButton = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (canPressTheButton)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                MoneyDisplayController money = GameObject.Find("MoneyDisplay").GetComponent<MoneyDisplayController>();
                GlobalTextController textController = GameObject.Find("GlobalTextController").GetComponent<GlobalTextController>();
                InputController input = GameObject.FindGameObjectWithTag("WeaponHolder").GetComponent<InputController>();

                textController.showText = true;
                if (money.MyMoney >= AmmoPrice)
                {
                    audioSource.clip = pickupEffect;
                    if (!audioSource.isPlaying)
                    {
                        audioSource.Play();
                        money.TakeMoney(AmmoPrice);
                        input.refillAmmoReserves(AmmoInBox);
                        textController.showText = true;
                        textController.setText = $"Ammo replenished.";
                    }
                }
                else if (money.MyMoney < AmmoPrice)
                {
                    textController.showText = true;
                    textController.setText = $"Not enough money!";
                }
                else if (input.ammoReserves >= input.maxReserves)
                {
                    textController.showText = true;
                    textController.setText = $"Ammo reserves are full!";
                }
                else
                {
                    textController.showText = false;
                    textController.setText = "";
                }

            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canPressTheButton = true;
        }
    }


    private void OnTriggerExit()
    {
        canPressTheButton = false;
    }

}
