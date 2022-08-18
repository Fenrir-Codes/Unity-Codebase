using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    PlayerMovementController player;
    InputController InputController;
    private Animator animator;
    private string currentAnimaton;
    const string Idle = "Idle";
    const string Fire = "Shoot";
    const string Reload = "Reload";

    [Header("_____ ANIMATOR SHOOTING SPEED SETTINGS _____")]
    [Header("The default shooting animation speed")]
    [SerializeField, Range(0, 2)] float DefaultAnimationSpeed = 1.0f;
    [Header("Speed of the Pistol shoot animation")]
    [SerializeField, Range(0, 2)] float PistolAnimationSpeed = 1.0f;
    [Header("Speed of the Assault Rifle shoot animation")]
    [SerializeField, Range(0, 2)] float ARAnimationSpeed = 0.8f;
    [Header("Speed of the LMG shoot animation")]
    [SerializeField, Range(0, 2)] float LMGAnimationSpeed = 2.0f;
    [Header("Speed of the shotgun shoot animation")]
    [SerializeField, Range(0, 2)] float ShotgunAnimationSpeed = 1.0f;
    [Header("_____ ANIMATOR RELOAD SPEED SETTINGS _____")]
    [Header("Speed of the Pistol reload animation")]
    [SerializeField, Range(0, 2)] float PistolReloadAnimationSpeed = 1.0f;
    [Header("Speed of the Assault Rifle reload animation")]
    [SerializeField, Range(0, 2)] float AssaultRifleReloadAnimationSpeed = 1.0f;
    [Header("Speed of the LMG reload animation")]
    [SerializeField, Range(0, 2)] float LMGReloadAnimationSpeed = 1.0f;
    [Header("Speed of the shotgun reload animation")]
    [SerializeField, Range(0, 2)] float ShotgunReloadAnimationSpeed = 1.0f;


    // Start is called before the first frame update
    void Start()
    {
        InputController = GetComponent<InputController>();
        player = GetComponentInParent<PlayerMovementController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded && InputController.isShooting && animator != null)
        {
            ChangeAnimationState(Fire);

            if (InputController.weapons[0].CompareTag("Assault Rifle"))
            {
                animator.speed = ARAnimationSpeed;
            }
            if (InputController.weapons[0].CompareTag("LMG"))
            {
                animator.speed = LMGAnimationSpeed;
            }
            if (InputController.weapons[0].CompareTag("Shotgun"))
            {
                animator.speed = ShotgunAnimationSpeed;
            }
        }
        else if (InputController.isReloading)
        {
            ChangeAnimationState(Reload);

            if (InputController.weapons[0].CompareTag("Assault Rifle"))
            {
                animator.speed = AssaultRifleReloadAnimationSpeed;
            }
            if (InputController.weapons[0].CompareTag("LMG"))
            {
                animator.speed = LMGReloadAnimationSpeed;
            }
            if (InputController.weapons[0].CompareTag("Shotgun"))
            {
                animator.speed = ShotgunReloadAnimationSpeed;
            }
        }
        else
        {
            ChangeAnimationState(Idle);
            animator.speed = DefaultAnimationSpeed;
        }
    }

    private void ChangeAnimationState(string newAnimation)
    {
        if (currentAnimaton == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimaton = newAnimation;
    }

}
