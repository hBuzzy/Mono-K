using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CharacterJump : MonoBehaviour
{
    [Header("Jumping Stats")]
    [SerializeField, Range(2f, 5.5f)] private float _jumpHeight = 2.25f;
    [SerializeField, Range(0.2f, 1.25f)] private float _timeToJumpApex = 0.3f;
    [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 0.65f;
    [SerializeField, Range(1f, 10f)] private float _downwardMovementMultiplier = 2.25f;
    [SerializeField, Range(10f, 25f)] private float _wallJumpForce = 20f;
    
    [Header("Options")]
    [SerializeField, Range(10f, 20f)] private float _speedLimit = 15f;
    [SerializeField, Range(0f, 10f)] private float _jumpCutOff = 2.25f; //TODO: Rename
    [SerializeField, Range(0f, 0.3f)] private float _jumpBuffer = 0.15f;
    [SerializeField, Range(0f, 0.3f)] private float _coyoteTime = 0.15f;
    
    private float _defaultGravityScale = 1f; //TODO: Make const and reorder?

    private Rigidbody2D _rigidbody;
    private PlayerInputActions _playerInput;
    private Character _character;
    private CharacterStates _states;
    
    #region Calculations
    
    private Vector2 _velocity;
    private float _gravityMultiplier;
    private float _jumpSpeed;
    private float _jumpBufferCounter;

    #endregion

    #region Current states
    
    private bool _isJumpRequired;
    private bool _isJumpButtonPressing;
    private bool _isCurrentlyJumping;

    #endregion

    public event Action Jumped;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
        _states = GetComponent<CharacterStates>();
        _playerInput = new PlayerInputActions();
        _defaultGravityScale = 1f;
    }

    private void OnEnable()
    {
        _playerInput.Character.Enable();
        _playerInput.Character.Jump.started += OnJumpStarted;
        _playerInput.Character.Jump.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        _playerInput.Character.Disable();
        _playerInput.Character.Jump.started -= OnJumpStarted;
        _playerInput.Character.Jump.canceled -= OnJumpCanceled;
    }

    void Update()
    {
        if (_character.CanMove)
        {
            SetGravity();
        }

        CalculateBuffer();
    }

    private void SetGravity()
    {
        float gravityY = (-2 * _jumpHeight) / (_timeToJumpApex * _timeToJumpApex);
        
        _rigidbody.gravityScale = (gravityY / Physics2D.gravity.y) * _gravityMultiplier;
    }

    private void FixedUpdate()
    {
        _velocity = _rigidbody.velocity;

        if (_isJumpRequired)
        {
            Jump();
            return;
        }

        CalculateGravity();
    }

    private void CalculateGravity()
    {
        if (_rigidbody.velocity.y > 0.01f)
        {
            if (_character.IsGrounded || _character.IsTouchingWall)
            {
                _gravityMultiplier = _defaultGravityScale;
            }
            else
            {
                if (_isJumpButtonPressing && _isCurrentlyJumping)
                {
                    _gravityMultiplier = _upwardMovementMultiplier;
                }
                else
                {
                    _gravityMultiplier = _jumpCutOff;
                }
            }
        }
        else if (_rigidbody.velocity.y < -0.01f)
        {
            if (_character.IsGrounded)
            {
                _gravityMultiplier = _defaultGravityScale;
            }
            else
            {
                _gravityMultiplier = _downwardMovementMultiplier;
            }
        }
        else
        {
            if (_character.IsGrounded || _character.IsTouchingWall)//TODO: Добавить на стене
            {
                _isCurrentlyJumping = false;
            }

            _gravityMultiplier = _defaultGravityScale;
        }

        _rigidbody.velocity = new Vector3(_velocity.x, Mathf.Clamp(_velocity.y, -_speedLimit, 100));
    }

    private void Jump()
    {
        if (_character.IsGrounded)
        {
            JumpFromGround();
        }

        if (_states.GetCurrentState() == CharacterStates.States.Slide) //TODO:  Add Grabbing?
        {
            JumpFromWall();
        }

        if (_jumpBuffer == 0)
        {
            _isJumpRequired = false;
        }
        _rigidbody.velocity = _velocity;
    }

    private void JumpFromGround()
    {
        Jumped?.Invoke();
        
        _jumpBufferCounter = 0;
        _isJumpRequired = false;

        var rem = _jumpSpeed;

        _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _rigidbody.gravityScale * _jumpHeight);
        
        if (_velocity.y > 0f) {
            _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
        }
        else if (_velocity.y < 0f) {
            _jumpSpeed += Mathf.Abs(_rigidbody.velocity.y); //TODO: Exchange it with = instead of += ?
        }
            
        if (_jumpSpeed > rem * 1.3 && rem != 0) //TODO: Create Methhod or fix this bug
        {
            _jumpSpeed = rem;
        }

        _velocity.y += _jumpSpeed;
        _isCurrentlyJumping = true;
    }

    private void JumpFromWall()
    {
        Jumped?.Invoke();
        
        _jumpBufferCounter = 0;
        _isJumpRequired = false;
        
        _velocity += new Vector2(-_character.FacingDirectionX * _wallJumpForce, _wallJumpForce);
        
        _isCurrentlyJumping = true;
    }

    private void CalculateBuffer()
    {
        if (_jumpBuffer <= 0 || _isJumpRequired == false)
        {
            return;
        }

        _jumpBufferCounter += Time.deltaTime;

        if (_jumpBufferCounter > _jumpBuffer)
        {
            _isJumpRequired = false;
            _jumpBufferCounter = 0;
        }
    }

    private void OnJumpStarted(InputAction.CallbackContext context)
    {
        _isJumpRequired = true;
        _isJumpButtonPressing = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        _isJumpButtonPressing = false;
    }
}