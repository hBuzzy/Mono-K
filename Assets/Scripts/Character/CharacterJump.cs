using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterJump : MonoBehaviour
{
    [Header("Components")] [HideInInspector]
    public Rigidbody2D body;

    [HideInInspector] public Vector2 velocity;
    //private characterJuice juice;


    [Header("Jumping Stats")]
    [SerializeField, Range(2f, 5.5f)] public float jumpHeight = 7.3f;
    [SerializeField, Range(0.2f, 1.25f)] public float timeToJumpApex;
    [SerializeField, Range(0f, 5f)] public float upwardMovementMultiplier = 1f;
    [SerializeField, Range(1f, 10f)] public float downwardMovementMultiplier = 6.17f;

    [Header("Options")]
    [SerializeField, Range(0f, 10f)] public float jumpCutOff;
    [SerializeField] public float speedLimit;
    [SerializeField, Range(0f, 0.3f)] public float coyoteTime = 0.15f;

    [SerializeField, Range(0f, 0.3f)] public float jumpBuffer = 0.15f;

    [Header("Calculations")] public float jumpSpeed;
    private float defaultGravityScale = 1f;
    public float gravMultiplier = 1f;

    [Header("Current State")] 
    private bool desiredJump;
    private bool pressingJump;
    public bool _isGrounded;
    private bool currentlyJumping;
    
    private PlayerInputActions _playerInput;
    private Character _character;
    private CharacterStates _states;

    public event Action Jumped;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
        _states = GetComponent<CharacterStates>();
        _playerInput = new PlayerInputActions();
        defaultGravityScale = 1f;
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Character.Jump.started += OnJumpStarted;
        _playerInput.Character.Jump.canceled += OnJumpCanceled;
        _character.GroundedChanged += isGrounded => { _isGrounded = isGrounded; };
        
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Character.Jump.started -= OnJumpStarted;
        _playerInput.Character.Jump.canceled -= OnJumpCanceled;
    }

    void Update()
    {
        if (_states.GetCurrentState() != CharacterStates.States.Dash)
        {
            setPhysics();
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
            if (_isGrounded)
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
            if (_isGrounded)
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
            if (_isGrounded)
            {
                currentlyJumping = false;
            }

            gravMultiplier = defaultGravityScale;
        }

        body.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }

    private void DoAJump()
    {
        if (_isGrounded)
        {
            Jumped.Invoke();
            desiredJump = false;

            var rem = jumpSpeed;

            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * body.gravityScale * jumpHeight);
        
            if (velocity.y > 0f) {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f) {
                jumpSpeed += Mathf.Abs(body.velocity.y);
            }
            
            if (jumpSpeed > rem * 1.3 && rem != 0) //TODO: Create Methhod or fix this bug
            {
                jumpSpeed = rem;
            }

            velocity.y += jumpSpeed;
            currentlyJumping = true;
            //JumpStatusChanged?.Invoke(false);
        }

        if (_states.GetCurrentState() == CharacterStates.States.Slide)
        {
            Jumped.Invoke();
            desiredJump = false;
            
            var rem = jumpSpeed;

            jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * body.gravityScale * jumpHeight);
        
            if (velocity.y > 0f) {
                jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
            }
            else if (velocity.y < 0f) {
                jumpSpeed += Mathf.Abs(body.velocity.y);
            }
            
            if (jumpSpeed > rem * 1.3 && rem != 0) //TODO: Create Methhod or fix this bug
            {
                jumpSpeed = rem;
            }

            velocity.y += jumpSpeed;
            currentlyJumping = true;
        }
    }
}