using System;
using UnityEngine;
using UnityEngine.InputSystem;
using States = CharacterStates.States;

public class CharacterJump : MonoBehaviour
{
    [Header("Jumping Stats")]
    [SerializeField, Range(2f, 5.5f)] private float _jumpHeight = 2.25f;
    [SerializeField, Range(0.2f, 1.25f)] private float _timeToJumpApex = 0.3f;
    [SerializeField, Range(0f, 5f)] private float _upwardMovementMultiplier = 0.65f;//TODO: rename
    [SerializeField, Range(1f, 10f)] private float _downwardMovementMultiplier = 2.25f;//TODO: rename
    [SerializeField, Range(10f, 25f)] private float _wallJumpForce = 20f;
    
    [Header("Options")]
    [SerializeField, Range(10f, 20f)] private float _speedLimit = 15f;
    [SerializeField, Range(0f, 10f)] private float _jumpCutOff = 2.25f; //TODO: Rename
    [SerializeField, Range(0f, 0.3f)] private float _jumpBuffer = 0.15f;
    [SerializeField, Range(0f, 0.3f)] private float _coyoteTime = 0.15f;
    
    private const float DefaultGravityScale = 1f;
    private const float ComparisonError = 0.01f;

    private Rigidbody2D _rigidbody;
    private PlayerInputActions _playerInput;
    private Character _character;
    private CharacterStates _states;
    
    private Vector2 _velocity;
    private float _gravityMultiplier;
    private float _jumpSpeed;
    private float _jumpBufferCounter;

    private bool _isJumpRequired;
    private bool _isJumpButtonPressing;
    private bool _isCurrentlyJumping;
    
    public event Action Jumped;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
        _states = GetComponent<CharacterStates>();
        _playerInput = new PlayerInputActions();
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

    private void Update()
    {
        SetGravity();

        CalculateBuffer();
    }

    private void SetGravity()
    {
        float gravityY = (-2 * _jumpHeight) / (_timeToJumpApex * _timeToJumpApex);
        float gravityScale = (gravityY / Physics2D.gravity.y) * _gravityMultiplier;
        
        _character.SetGravity(gravityScale);
    }

    private void FixedUpdate()
    {
        _velocity = _character.Velocity;

        if (TryJump())
        {
            return;
        }

        if (_jumpBuffer == 0f)
        {
            _isJumpRequired = false;
        }

        CalculateGravity();
    }

    private void CalculateGravity()
    {
        if (_rigidbody.velocity.y > ComparisonError)
        {
            if (_character.IsGrounded || _character.IsTouchingWall)
            {
                _gravityMultiplier = DefaultGravityScale;
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
        else if (_rigidbody.velocity.y < -ComparisonError)
        {
            if (_character.IsGrounded)
            {
                _gravityMultiplier = DefaultGravityScale;
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

            _gravityMultiplier = DefaultGravityScale;
        }
        
        LimitFallSpeed();//TODO: Move out?
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

    private void LimitFallSpeed()
    {
        float maxSpeed = 100f; //TODO: Raplace with positive speed limit?
        
        Vector2 velocity = new Vector2(_velocity.x, Mathf.Clamp(_velocity.y, -_speedLimit, maxSpeed));
        
        _character.SetVelocity(velocity);
    }

    private bool CanJump(States state)
    {
        return _character.IsGrounded || state == States.Slide;
    }

    private bool TryJump()
    {
        var state = _states.GetCurrentState();
        
        if (_isJumpRequired && CanJump(state))
        {
            var velocity = GetJumpVelocity(state);

            if (velocity != Vector2.zero)
            {
                Jump(velocity);

                return true;
            }
        }

        return false;
    }

    private void Jump(Vector2 velocity)
    {
        _jumpBufferCounter = 0;
        _isJumpRequired = false;
        _isCurrentlyJumping = true;
        
        Jumped?.Invoke();

        _velocity += velocity;
        _character.SetVelocity(_velocity);
    }

    private Vector2 GetJumpVelocity(States state)
    {
        if (_character.IsGrounded)
        {
            return GetVelocityFromGround();
        }

        if (state == States.Slide) //TODO:  Add Grabbing?
        {
            return GetVelocityFromWall();
        }

        return Vector2.zero;
    }

    private Vector2 GetVelocityFromGround()//TODO:Need refactoring for fox
    {
        var rem = _jumpSpeed;

        _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _character.GravityScale * _jumpHeight);
        
        /*if (_velocity.y > 0f)
        {
            _jumpSpeed += Mathf.Max(_jumpSpeed - _velocity.y, 0f);
        }
        else if (_velocity.y < 0f)
        {
            _jumpSpeed += Mathf.Abs(_velocity.y); //TODO: Exchange it with = instead of += ?
        }*/
            
        /*if (_jumpSpeed > rem * 1.3 && rem != 0) //TODO: Create Methhod or fix this bug
        {
            _jumpSpeed = rem;
        }*/

        return new Vector2(0f, _jumpSpeed);
    }

    private Vector2 GetVelocityFromWall()
    {
        return new Vector2(-_character.FacingDirectionX * _wallJumpForce, _wallJumpForce);
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