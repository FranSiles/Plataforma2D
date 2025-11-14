using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Move settings")]
    [Range(1, 10)]
    [SerializeField] private float moveSpeed;
    [Range(1, 3)]
    [SerializeField] private float jumpForce;
    [Range (0, 1f)]
    [SerializeField] private float crouch;
    //Update for jump 
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;


    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float radiusRaycast = 0.5f;
    private bool forceCrouch = false;

    //Inputs
    private Vector2 movementInput;
    private bool jumpInput;
    private bool attackInput;
    private float crouchInput;

    //Inputs Actions
    private InputSystem_Actions playerControls; //Control inputs actions 
    private Vector3 jumpVector = Vector3.zero;
    


    //Components and GameObjects
    private SpriteRenderer playerSprite;
    private Rigidbody2D rb;
    private GameObject playerFeet;
    private Animator animator;
    private CapsuleCollider2D capsuleCollider;

    //Ground check Layer
    private LayerMask groundLayer;

    //the player was in the ground the previous frame
    private bool wasGrounded = true;

    //Variables for the crouch action
    private Vector2 originalSize;
    private Vector2 crouchSize;
    private Vector2 originalOffset;
    private Vector2 crouchOffset;

    private void Awake()
    {
        //Initializations
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        groundLayer = LayerMask.GetMask("Ground");
        playerFeet = GameObject.Find("PlayerFeet");
        animator = GetComponentInChildren<Animator>();

        //Initialize and enable the input actions
        playerControls = new InputSystem_Actions();
        playerControls.Enable();

        // Original variable for capsuleCollider
        originalSize = capsuleCollider.size;
        originalOffset = capsuleCollider.offset;

        // Crouch variable for capsuleCollider
        crouchSize = new Vector2(originalSize.x, originalSize.y * crouch);
        //
        crouchOffset = new Vector2(originalOffset.x, originalOffset.y - (originalSize.y - crouchSize.y) / 2f);

    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        MovePlayer();
        FlipStated();

        CalculateJump();
        Crouch();
    }
    private void FixedUpdate()
    {
        Jump();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(playerFeet.transform.position, radiusRaycast);
    }
    /// <summary>
    /// Generate the Physic Jump Movement
    /// </summary>
    private void Jump()
    {
        rb.AddForce(jumpVector, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Read de vertical and horizontal Axis from Input
    /// </summary>
    private void GetInputs()
    {
        //Get movement Input
        movementInput = playerControls.Player.Move.ReadValue<Vector2>();

        //Read jump
        jumpInput = playerControls.Player.Jump.triggered;

        //ATTACK!!!!!!
        attackInput = playerControls.Player.Attack.triggered;

        //Crouch
        crouchInput = playerControls.Player.Crouch.ReadValue<float>();

    }

    /// <summary>
    /// Change Player Poisition with axis and speed 
    /// </summary>
    private void MovePlayer()
    {
        transform.position += Vector3.right * movementInput.x * moveSpeed * Time.deltaTime;

        //de/active animation
        animator.SetFloat("Speed", Mathf.Abs(movementInput.x));
    }
    private void FlipStated()
    {
        if (movementInput.x > 0)
        {
            playerSprite.flipX = false; //Right direction
        }
        else if (movementInput.x < 0)
        {
            playerSprite.flipX = true; //Left direction
        }
    }
    /// <summary>
    /// Check if the Player is on the Ground
    /// </summary>
    /// <returns> grounded state true or false</returns>
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(playerFeet.transform.position, radiusRaycast, groundLayer);
    }

    /// <summary>
    /// Calculate the vector3 for the jump
    /// </summary>
    private void CalculateJump()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if (jumpInput)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -=Time.deltaTime;
        }
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            jumpVector = Vector3.up * jumpForce;
            animator.SetBool("IsJumping", true);
            coyoteTimeCounter = 0f;
        }
        else if (!IsGrounded())
        {
            jumpVector = Vector3.zero;
        }
        //Landing detection
        //Am I on the ground now(this frame)?
        bool grounded = IsGrounded();
        //if the player was in the air before(previous fram), but now(this frame) is touching the ground
        if (grounded && !wasGrounded)
        {
            animator.SetBool("IsJumping", false);
        }
        wasGrounded = grounded;
    }

    /// <summary>
    /// Function for crouch the player when touch the ground
    /// </summary>
    private void Crouch()
    {
        if (crouchInput>0 && IsGrounded() || forceCrouch)
        {
            animator.SetBool("IsCrouching",true);
            capsuleCollider.size = crouchSize;
            capsuleCollider.offset = crouchOffset;
        }
        else
        {
            animator.SetBool("IsCrouching", false);
            capsuleCollider.size = originalSize;
            capsuleCollider.offset = originalOffset;
        }
    }
    public void ContinueCrouch()
    {
        forceCrouch = true;
        StartCoroutine(StayCrouchedUntilGrounded());
    }

    private IEnumerator StayCrouchedUntilGrounded()
    {
        yield return new WaitUntil(() => IsGrounded());
        forceCrouch = false;
    }

    private void OnDisable()
    {
        //disable the actions when the object is disabled
        playerControls.Disable();
    }
}
