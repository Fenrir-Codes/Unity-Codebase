using System.Text.RegularExpressions;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    // This script have to be attached on tha player object.
    //character controller
    [Header("Character controller (First person player)")]
    [Tooltip("The caharacter controller")]
    [SerializeField] private CharacterController controller;
    [Space]
    //Variables
    [Header("Gravity setting")]
    [Tooltip("Gravity value")]
    [SerializeField] private float Gravity = -20f;
    [Space]
    [Header("Player settings")]
    [Tooltip("Walking speed value")]
    [SerializeField] private float walkSpeed = 2.5f;
    [Space]
    [Tooltip("Running speed value")]
    [SerializeField] private float runSpeed = 3f;
    [Space]
    [Tooltip("Jump force value")]
    [SerializeField] private float jumpForce = 0.5f;
    [HideInInspector] public float moveSpeed = 0f;
    [Space]
    //Vectors
    [HideInInspector] public Vector3 moveDirection;
    public Vector3 Velocity;

    //Booleans
    public bool isGrounded;
    [HideInInspector] public bool isWalking = false;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool isJumping = false;
    [HideInInspector] public bool isAiming = false;

    //Keycodes
    private KeyCode jumpButton = KeyCode.Space;
    private KeyCode runButton = KeyCode.LeftShift;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    //Update
    #region Update
    // Update is called once per frame
    private void Update()
    {
        ResetVelocity();
        Initializing();
        processActions();
        ApplyGravity();
    }
    #endregion

    #region process Actions
    public void processActions()
    {
        //Move
        if (moveDirection != Vector3.zero)
        {
            isWalking = true;
            Move();
        }
        else
        {
            isWalking = false;
            moveSpeed = 0.0f;
        }
        //Run
        if (moveDirection != Vector3.zero && Input.GetKey(runButton))
        {
            Run();
        }
        else
        {
            moveSpeed = 0.0f;
            isRunning = false;
        }
        if (Input.GetMouseButton(1))
        {
            isAiming = true;
        }
        else
        {
            isAiming = false;
        }
        //Jump
        if (Input.GetKeyDown(jumpButton) && isGrounded)
        {
            Jump();
        }

    }
    #endregion

    #region Initalize
    private void Initializing()
    {
        isGrounded = controller.isGrounded;
        moveDirection.Normalize();
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = Input.GetAxis("Vertical");
    }
    #endregion

    #region Move the character
    void Move()
    {
        if (isWalking)
        {
            isRunning = false;
            moveSpeed = walkSpeed;
            moveDirection = transform.right * moveDirection.x + transform.forward * moveDirection.y;
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        }
        else
        {
            moveSpeed = 0.0f;
        }


    }
    #endregion

    #region Jump
    void Jump()
    {
        isJumping = true;
        Velocity.y += Mathf.Sqrt(jumpForce * -3.0f * Gravity);
        isJumping = false;
    }
    #endregion

    #region Running
    void Run()
    {
        isWalking = false;
        isRunning = true;
        moveSpeed = runSpeed;
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
        //Debug.Log("running? : "+isRunning);
    }
    #endregion

    #region Crouching
    //void Crouch()
    //{
    //    if (isCrouching)
    //    {
    //        controller.height = crouchHeight;
    //        playerBody.position = new Vector3(transform.position.x, transform.position.y - 0.687f, transform.position.z);

    //        if (moveSpeed > 0f)
    //        {
    //            isCrouchWalk = true;
    //            controller.height = crouchHeight;
    //            playerBody.position = new Vector3(transform.position.x, transform.position.y - 0.687f, transform.position.z);
    //        }
    //        else
    //        {
    //            isCrouchWalk = false;
    //        }
    //    }
    //    else
    //    {
    //        isCrouchWalk = false;
    //        playerBody.position = new Vector3(transform.position.x, transform.position.y - 0.968f, transform.position.z);
    //        controller.height = originalHeight;

    //    }

    //}
    #endregion

    #region Apply gravitation
    void ApplyGravity()
    {
        Velocity.y += Gravity * Time.deltaTime;
        controller.Move(Velocity * Time.deltaTime);
    }
    #endregion

    #region Resetting velocity
    void ResetVelocity()
    {
        if (Velocity.y < 0 && isGrounded)
        {
            Velocity.y = -1f;
            //Debug.Log("Velocity : " + Velocity.y);
        }
        if (Velocity.y < -15)
        {
           
        }
    }
    #endregion

}


