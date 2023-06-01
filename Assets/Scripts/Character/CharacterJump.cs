using System;
using UnityEngine;
using UnityEngine.InputSystem;


public class CharacterJump : MonoBehaviour
{
    [Header("Components")] [HideInInspector]
    public Rigidbody2D body;

    private Grounder ground;
    private CharacterGround _ground2;

    [HideInInspector] public Vector2 velocity;
    //private characterJuice juice;


    [Header("Jumping Stats")] [SerializeField, Range(2f, 5.5f)] [Tooltip("Maximum jump height")]
    public float jumpHeight = 7.3f;

    [SerializeField, Range(0.2f, 1.25f)] [Tooltip("How long it takes to reach that height before coming back down")]
    public float timeToJumpApex;

    [SerializeField, Range(0f, 5f)] [Tooltip("Gravity multiplier to apply when going up")]
    public float upwardMovementMultiplier = 1f;

    [SerializeField, Range(1f, 10f)] [Tooltip("Gravity multiplier to apply when coming down")]
    public float downwardMovementMultiplier = 6.17f;

    [SerializeField, Range(0, 1)] [Tooltip("How many times can you jump in the air?")]
    public int maxAirJumps = 0;

    [Header("Options")] [Tooltip("Should the character drop when you let go of jump?")]
    public bool variablejumpHeight;

    [SerializeField, Range(1f, 10f)] [Tooltip("Gravity multiplier when you let go of jump")]
    public float jumpCutOff;

    [SerializeField] [Tooltip("The fastest speed the character can fall")]
    public float speedLimit;

    [SerializeField, Range(0f, 0.3f)] [Tooltip("How long should coyote time last?")]
    public float coyoteTime = 0.15f;

    [SerializeField, Range(0f, 0.3f)] [Tooltip("How far from ground should we cache your jump?")]
    public float jumpBuffer = 0.15f;

    [Header("Calculations")] public float jumpSpeed;
    private float defaultGravityScale = 1f;
    public float gravMultiplier = 1f;

    [Header("Current State")] private bool desiredJump;
    private bool pressingJump;
    public bool onGround;
    private bool currentlyJumping;
    
    private PlayerInputActions _playerInput;

    public event Action Jumped;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        ground = GetComponent<Grounder>();
        _ground2 = GetComponent<CharacterGround>();
        _playerInput = new PlayerInputActions();
        defaultGravityScale = 1f;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Character.Jump.started += OnJumpStarted;
        _playerInput.Character.Jump.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Character.Jump.started -= OnJumpStarted;
        _playerInput.Character.Jump.canceled -= OnJumpCanceled;
    }

    void Update()
    {
        if (CharacterMovementBlocker.Instance.CanMove)
        {
            setPhysics();
        }

        if (ground.enabled)
        {
            onGround = ground.IsGrounded;
        }
        else if (_ground2.enabled)
        {
            onGround = _ground2.GetOnGround();
        }
    }

    private void setPhysics()
    {
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
        body.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
    }

    private void FixedUpdate()
    {
        velocity = body.velocity;

        if (desiredJump)
        {
            DoAJump();
            Jumped?.Invoke();
            body.velocity = velocity;
            return;
        }

        calculateGravity();
    }

    private void OnJumpStarted(InputAction.CallbackContext context)
    {
        desiredJump = true;
        pressingJump = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        pressingJump = false;
    }

    private void calculateGravity()
    {
        if (body.velocity.y > 0.01f)
        {
            if (onGround)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                if (pressingJump && currentlyJumping)
                {
                    gravMultiplier = upwardMovementMultiplier;
                }
                else
                {
                    gravMultiplier = jumpCutOff;
                }
            }
        }
        else if (body.velocity.y < -0.01f)
        {
            if (onGround)
            {
                gravMultiplier = defaultGravityScale;
            }
            else
            {
                gravMultiplier = downwardMovementMultiplier;
            }
        }
        else
        {
            if (onGround)
            {
                currentlyJumping = false;
            }

            gravMultiplier = defaultGravityScale;
        }

        body.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }

    private void DoAJump()
    {
        if (onGround)
        {
            desiredJump = false;

            var rem = jumpSpeed;

            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * body.gravityScale * jumpHeight);
        
            if (velocity.y > 0f) {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f) {
                jumpSpeed += Mathf.Abs(body.velocity.y);
            }
            
            if (jumpSpeed > rem * 1.5 && rem != 0)
            {
                jumpSpeed = rem;
            }

            velocity.y += jumpSpeed;
            currentlyJumping = true;
        }
    }
}