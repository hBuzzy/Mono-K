using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CharacterJump : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    [HideInInspector] public Vector2 velocity;//TODO:
    //private characterJuice juice;


    [Header("Jumping Stats")]
    [SerializeField, Range(2f, 5.5f)] public float jumpHeight = 7.3f;
    [SerializeField, Range(0.2f, 1.25f)] public float timeToJumpApex;
    [SerializeField, Range(0f, 5f)] public float upwardMovementMultiplier = 1f;
    [SerializeField, Range(1f, 10f)] public float downwardMovementMultiplier = 6.17f;
    [SerializeField] private float _wallJumpForce;
    
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
    private bool _isTouchingWall;
    private float jumpBufferCounter;
    
    private PlayerInputActions _playerInput;
    private Character _character;
    private CharacterStates _states;

    public event Action Jumped;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
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
        _character.WalledChanged += isTouchingWall =>
        {
            _isTouchingWall = isTouchingWall;
        };
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
            SetPhysics();
        }

        if (jumpBuffer > 0)
        {
            if (desiredJump)
            {
                jumpBufferCounter += Time.deltaTime;

                if (jumpBufferCounter > jumpBuffer)
                {
                    desiredJump = false;
                    jumpBufferCounter = 0;
                }
            }
        }
    }

    private void SetPhysics()
    {
        Vector2 newGravity = new Vector2(0, (-2 * jumpHeight) / (timeToJumpApex * timeToJumpApex));
        _rigidbody.gravityScale = (newGravity.y / Physics2D.gravity.y) * gravMultiplier;
    }

    private void FixedUpdate()
    {
        velocity = _rigidbody.velocity;

        if (desiredJump)
        {
            Jump();
            return;
        }

        CalculateGravity();
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

    private void CalculateGravity()
    {
        if (_rigidbody.velocity.y > 0.01f)
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
        else if (_rigidbody.velocity.y < -0.01f)
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
            if (_isGrounded || _isTouchingWall)//TODO: Добавить на стене
            {
                currentlyJumping = false;
            }

            gravMultiplier = defaultGravityScale;
        }

        _rigidbody.velocity = new Vector3(velocity.x, Mathf.Clamp(velocity.y, -speedLimit, 100));
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            JumpFromGround();
        }

        if (_states.GetCurrentState() == CharacterStates.States.Slide)
        {
            JumpFromWall();
        }

        if (jumpBuffer == 0)
        {
            desiredJump = false;
        }
        _rigidbody.velocity = velocity;
    }

    private void JumpFromGround()
    {
        Jumped?.Invoke();
        jumpBufferCounter = 0;
        desiredJump = false;

        var rem = jumpSpeed;

        jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _rigidbody.gravityScale * jumpHeight);
        
        if (velocity.y > 0f) {
            jumpSpeed = Mathf.Max(jumpSpeed - velocity.y, 0f);
        }
        else if (velocity.y < 0f) {
            jumpSpeed += Mathf.Abs(_rigidbody.velocity.y);
        }
            
        if (jumpSpeed > rem * 1.3 && rem != 0) //TODO: Create Methhod or fix this bug
        {
            jumpSpeed = rem;
        }

        velocity.y += jumpSpeed;
        currentlyJumping = true;
    }

    private void JumpFromWall()
    {
        //_rigidbody.velocity = Vector2.zero;
        
        Jumped?.Invoke();
        jumpBufferCounter = 0;
        desiredJump = false;
        
        if (_character.IsFacingLeft)
        {
            velocity += new Vector2(_wallJumpForce, _wallJumpForce);
        }
        else
        {
            velocity += new Vector2(-_wallJumpForce, _wallJumpForce);
        }
        
        currentlyJumping = true;

        // if (_character.IsFacingLeft)
        // {
        //     _rigidbody.AddForce(new Vector2(1 * _wallJumpForce, _wallJumpForce), ForceMode2D.Impulse);
        // }
        // else
        // {
        //     _rigidbody.AddForce(new Vector2(-1 * _wallJumpForce, _wallJumpForce), ForceMode2D.Impulse);
        // }
    }
}