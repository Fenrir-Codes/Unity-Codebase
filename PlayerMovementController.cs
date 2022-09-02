using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    // This script have to be attached on tha player object.

    //character controller
    WeaponPickUp pickupController;
    GunController gun;
    [Header("Character controller (First person player)")]
    [Tooltip("The caharacter controller")]
    [SerializeField] private CharacterController controller;

    //Variables
    [Header("Gravity setting")]
    [Tooltip("Gravity value")]
    [SerializeField] private float Gravity = -15f;

    [Header("Player settings")]
    [Tooltip("Walking speed value")]
    [SerializeField] private float walkSpeed = 2.5f;
    [Tooltip("Running speed value")]
    [SerializeField] private float runSpeed = 5f;
    [Tooltip("Jump force value")]
    [SerializeField] private float jumpForce = 4f;
    [HideInInspector] public float moveSpeed = 0f;

    //Vectors
    public Vector3 moveDirection;
    private Vector3 Velocity;

    //Booleans
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public bool isWalking = false;
    [HideInInspector] public bool isRunning = false;
    [HideInInspector] public bool isJumping = false;
    [HideInInspector] public bool isAiming = false;

    //Keycodes
    private KeyCode jumpButton = KeyCode.Space;
    private KeyCode runButton = KeyCode.LeftShift;


    private void Awake()
    {
        pickupController = GetComponentInChildren<WeaponPickUp>();
        controller = GetComponent<CharacterController>();
    }

    //Update
    #region Update
    // Update is called once per frame
    void Update()
    {
        ResetVelocity();
        ApplyGravity();
        isGrounded = controller.isGrounded;

        moveDirection.Normalize();
        moveDirection.x = Input.GetAxis("Horizontal");
        moveDirection.y = Input.GetAxis("Vertical");

        //Move
        if (moveDirection != Vector3.zero)
        {
            isWalking = true;
            Move();
        }
        else
        {
            isWalking = false;
        }
        //Run
        if (moveDirection != Vector3.zero && Input.GetKey(runButton))
        {
            Run();
        }
        else
        {
            moveSpeed = 0f;
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
        //if (pickupController.isEmptyHands == false)
        //{
        //    gun = GetComponentInChildren<GunController>();
        //    if (gun.isShooting == true)
        //    {
        //        isRunning = false;
        //        isWalking = false;
        //    }
        //    if (gun.isReloading)
        //    {
        //        isRunning = false;
        //        isWalking = false;
        //    }
        //}

        ApplyGravity();


    }
    #endregion

    //Functions
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
            moveSpeed = 0f;
        }


    }
    #endregion

    #region Jump
    void Jump()
    {
        isJumping = true;
        Velocity.y = jumpForce;
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
        Velocity.y -= Gravity * Time.deltaTime;
        controller.Move(Velocity * Time.deltaTime);
    }
    #endregion

    #region Resetting velocity
    void ResetVelocity()
    {
        if (Velocity.y < 0)
        {
            Velocity.y = -2f;
            //Debug.Log("Velocity : " + Velocity.y);
        }
    }
    #endregion

}


