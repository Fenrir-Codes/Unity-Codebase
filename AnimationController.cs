using System;
using System.Collections;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    BuyAssaultRifle buyAssault;
    BuyLMG buyLMG;
    BuyShotgun buyShotgun;
    BuyMinigun buyMinigun;

    PlayerMovementController player;
    InputController InputController;

    private Animator animator;
    private string currentAnimaton;
    const string Begin = "Start";
    const string Idle = "Idle";
    const string Walk = "Walk";
    const string Fire = "Shoot";
    const string Reload = "Reload";
    //weapon tag's
    const string Pistol = "Pistol";
    const string AssaultRifle = "Assault Rifle";
    const string LMG = "LMG";
    const string Shotgun = "Shotgun";
    const string Minigun = "Minigun";

    [Header("The activated weapon in the input controller array")]
    public int activeGun = 0;

    //[Header("_____ ANIMATOR WALK SETTINGS _____")]
    //[Header("The walk animation speed")]
    //[SerializeField, Range(0, 2)] float walkAnimationSpeed = 1.0f;
    [Header("_____ ANIMATOR SHOOTING SPEED SETTINGS _____")]
    [Header("The default shooting animation speed")]
    [SerializeField, Range(0, 2)] float DefaultAnimationSpeed = 1.0f;
    [Header("Pistol shoot animation")]
    [SerializeField, Range(0, 2)] float PistolAnimationSpeed = 1.0f;
    [Header("Assault Rifle shoot animation")]
    [SerializeField, Range(0, 2)] float ARAnimationSpeed = 0.8f;
    [Header("LMG shoot animation")]
    [SerializeField, Range(0, 2)] float LMGAnimationSpeed = 2.0f;
    [Header("Minigun shoot animation")]
    [SerializeField, Range(0, 2)] float MinigunAnimationSpeed = 2.0f;
    [Header("Shotgun shoot animation")]
    [SerializeField, Range(0, 2)] float ShotgunAnimationSpeed = 1.0f;
    [Header("_____ ANIMATOR RELOAD SPEED SETTINGS _____")]
    [Header("Pistol reload animation")]
    [SerializeField, Range(0, 2)] float PistolReloadAnimationSpeed = 1.5f;
    [Header("Assault Rifle reload animation")]
    [SerializeField, Range(0, 2)] float AssaultRifleReloadAnimationSpeed = 1.0f;
    [Header("LMG reload animation")]
    [SerializeField, Range(0, 2)] float LMGReloadAnimationSpeed = 1.0f;
    [Header("Minigun reload animation")]
    [SerializeField, Range(0, 2)] float MinigunReloadAnimationSpeed = 1.0f;
    [Header("Shotgun reload animation")]
    [SerializeField, Range(0, 2)] float ShotgunReloadAnimationSpeed = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        getPrerequisites();
        StartCoroutine(startAnimation());
    }

    #region Update
    // Update is called once per frame
    void Update()
    {
        activeGun = InputController.activeWeapon;

        if (activeGun > -1)
        {
            animator = GetComponentInChildren<Animator>();
            if (animator != null)
            {
                //if (player.isWalking && !InputController.isShooting && !InputController.isAiming && !InputController.isReloading)
                //{

                //    ChangeAnimationState(Walk);
                //    animator.speed = walkAnimationSpeed;
                //}
                if (player.isGrounded && InputController.isShooting)
                {
                    if (InputController.weapons[activeGun].CompareTag(Pistol))
                    {
                        animator.speed = PistolAnimationSpeed;
                    }
                    if (InputController.weapons[activeGun].CompareTag(AssaultRifle))
                    {
                        animator.speed = ARAnimationSpeed;
                    }
                    if (InputController.weapons[activeGun].CompareTag(LMG))
                    {
                        animator.speed = LMGAnimationSpeed;
                    }
                    if (InputController.weapons[activeGun].CompareTag(Shotgun))
                    {
                        animator.speed = ShotgunAnimationSpeed;
                    }
                    if (InputController.weapons[activeGun].CompareTag(Minigun))
                    {
                        animator.speed = MinigunAnimationSpeed;
                    }

                    ChangeAnimationState(Fire);
                }
                else if (InputController.isReloading)
                {
                    if (InputController.weapons[activeGun].CompareTag(Pistol))
                    {
                        animator.speed = PistolReloadAnimationSpeed;
                    }
                    if (InputController.weapons[activeGun].CompareTag(AssaultRifle))
                    {
                        animator.speed = AssaultRifleReloadAnimationSpeed;
                    }
                    if (InputController.weapons[activeGun].CompareTag(LMG))
                    {
                        animator.speed = LMGReloadAnimationSpeed;
                    }
                    if (InputController.weapons[activeGun].CompareTag(Shotgun))
                    {
                        animator.speed = ShotgunReloadAnimationSpeed;
                    }
                    if (InputController.weapons[activeGun].CompareTag(Minigun))
                    {
                        animator.speed = MinigunReloadAnimationSpeed;
                    }

                    ChangeAnimationState(Reload);
                }
                else if (buyLMG.paid || buyAssault.paid || buyShotgun.paid || buyMinigun.paid)
                {
                    StartCoroutine(startAnimation());
                }
                else
                {
                    ChangeAnimationState(Idle);
                    animator.speed = DefaultAnimationSpeed;
                }
            }
            //else
            //{
            //    Debug.Log("Animator component not found! Check if animator components exists on the prefabs.");
            //}

        }

    }
    #endregion

    #region Function for change the animation state with strings
    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
    #endregion

    #region getting the prerequisites as player controller, weapon shops, animator, etc...
    void getPrerequisites()
    {
        buyShotgun = GameObject.FindGameObjectWithTag("ShopShotgun").GetComponent<BuyShotgun>();
        buyAssault = GameObject.FindGameObjectWithTag("ShopAR").GetComponent<BuyAssaultRifle>();
        buyLMG = GameObject.FindGameObjectWithTag("ShopLMG").GetComponent<BuyLMG>();
        buyMinigun = GameObject.FindGameObjectWithTag("ShopMinigun").GetComponent<BuyMinigun>();

        InputController = GetComponent<InputController>();

        player = GetComponentInParent<PlayerMovementController>();

        animator = GetComponentInChildren<Animator>();
    }
    #endregion

    #region startup animation plays once
    IEnumerator startAnimation()
    {
        yield return new WaitForSeconds(.01f);
        animator.Play(Begin);
        yield return new WaitForSeconds(.5f);
    }
    #endregion


}
