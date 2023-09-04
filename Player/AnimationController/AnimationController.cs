
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class AnimationController : MonoBehaviour
{
    Gun Gun;
    [SerializeField] InputController playerInput;
    [SerializeField] PlayerMovementController playerMovement;
    private Animator animator;
    AnimatorStateInfo info;
    private string currentAnimaton;
    private int WeaponIndex = 0;

    //int UnHolster = Animator.StringToHash("UnHolster");
    //int Idle = Animator.StringToHash("Idle");
    //int Shoot = Animator.StringToHash("Shoot");
    const string Idle = "Idle";
    const string LongTimeIdle = "LongIdle";
    const string Walk = "Walk";
    const string Run = "Run";
    const string Shoot = "Shoot";
    const string Reload = "Reload";
    const string ReloadEmpty = "EmptyReload";
    const string Steady = "Steady";
    const string Recoiling = "Recoil";


    private bool IDLE => !playerInput.isShooting && !playerInput.isAiming && !playerMovement.isWalking && !playerMovement.isRunning && !playerInput.isReloading;
    private bool Walking => playerMovement.isGrounded && playerMovement.isWalking && !playerInput.isShooting && !playerInput.isReloading && !playerInput.isAiming;
    private bool Running => playerMovement.isGrounded && playerMovement.isRunning && !playerMovement.isWalking && !playerInput.isShooting && !playerInput.isAiming && !playerInput.isReloading;
    private bool Shooting => !playerInput.isAiming && playerInput.isShooting;
    private bool EmptyReload => playerInput.isReloading && Gun.CurrentAmmo <= 0;
    private bool ReloadMagazine => playerInput.isReloading && Gun.CurrentAmmo > 0;
    private bool Aiming => playerInput.isAiming;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        WeaponIndex = playerInput.weaponIndex;
        if (!playerInput.isEmptyHands)
        {
            Gun = GetComponentInChildren<Gun>();
            animator = GetComponentInChildren<Animator>();
            if (animator != null)
            {
                if (Walking)
                {
                    ChangeAnimationState(Walk);
                }
                else if (Running)
                {
                    ChangeAnimationState(Run);
                }
                else if (Shooting)
                {
                    ChangeAnimationState(Recoiling);
                }
                else if (EmptyReload)
                {
                    ChangeAnimationState(ReloadEmpty);
                }
                else if (ReloadMagazine)
                {
                    ChangeAnimationState(Reload);
                }
                else if (Aiming)
                {
                    ChangeAnimationState(Steady);
                }
                else
                {
                    ChangeAnimationState(Idle);
                }
            }
            else
            {
                Debug.Log("Animator not found on the child object! Check components");
            }


        }
    }

    #region Function for change the animation state with strings
    private void ChangeAnimationState(string state)
    {
        if (currentAnimaton == state) return;

        animator.Play(state);
        currentAnimaton = state;
    }
    #endregion
}
