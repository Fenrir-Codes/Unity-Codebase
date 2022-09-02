using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    //This script should be attached to player character (Swat)

    WeaponPickUp weaponPickUp;
    InputController gun;
    PlayerMovementController player;
    private Animator animator;
    private string currentAnimaton;
    [SerializeField, Range(0, 1)] float IdleAnimationSpeed = 1f;
    [SerializeField, Range(0, 1)] float WalkAnimationSpeed = 0.55f;
    [SerializeField, Range(0, 1)] float RunAnimationSpeed = 0.8f;

    //Animation states
    const string Player_Idle = "fps_idle";
    const string Player_Walking = "fps_walk";
    const string Player_Fire = "Shoot";
    const string Player_Running = "fps_run_fast";
    const string Player_ReloadMagazine = "Reload";


    // Start is called before the first frame update
    void Awake()
    {
        player = GetComponentInParent<PlayerMovementController>();
        weaponPickUp = GetComponentInChildren<WeaponPickUp>();
        animator = GetComponentInChildren<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded)
        {
            if (player.isWalking && !player.isRunning)
            {
                ChangeAnimationState(Player_Walking);
                animator.speed = WalkAnimationSpeed;
            }
            else if (player.isRunning && !player.isWalking)
            {
                ChangeAnimationState(Player_Running);
                animator.speed = RunAnimationSpeed;
            }
            else if (gun != null)
            {

            }
            else
            {
                ChangeAnimationState(Player_Idle);
                animator.speed = IdleAnimationSpeed;
            }

        }

    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }
}
