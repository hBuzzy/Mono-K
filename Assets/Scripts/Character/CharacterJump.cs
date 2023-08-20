using System;
using UnityEngine;
using UnityEngine.InputSystem;
using States = CharacterStates.States;

public class CharacterJump : MonoBehaviour //TODO: Need separate awake and start?
{
    [Header("Jump stats")]
    [SerializeField, Range(2f, 5.5f)] private float _jumpHeight = 2.25f;
    [SerializeField, Range(0.2f, 1.25f)] private float _timeToJumpApex = 0.3f;
    [SerializeField, Range(10f, 25f)] private float _wallJumpForce = 20f;
    
    [Header("Gravity multipliers")]
    [SerializeField, Range(0f, 5f)] private float _jumpUpwardMultiplier = 0.65f;
    [SerializeField, Range(0f, 10f)] private float _jumpDownwardMultiplier = 2.25f;
    [SerializeField, Range(1f, 10f)] private float _fallMultiplier = 2.25f;

    [Header("Assists")]
    [SerializeField, Range(10f, 20f)] private float _speedLimit = 15f;
    [SerializeField, Range(0f, 0.3f)] private float _jumpBuffer = 0.15f;
    [SerializeField, Range(0f, 0.3f)] private float _coyoteTime = 0.15f; //TODO: NEed?;
    
    private const float DefaultGravityMultiplier = 1f;
    private const float ComparisonError = 0.01f;
    private const float CoyoteError = 0.03f;

    private Rigidbody2D _rigidbody;
    private PlayerInputActions _playerInput;
    private Character _character;
    private CharacterStates _states;

    private Vector2 _velocity;

    private float _gravityY;
    private float _jumpUpwardGravity;
    private float _jumpDownwardGravity;
    private float _fallGravity;
    private float _defaultGravity;
    private float _currentGravity;
    private float _jumpBufferCounter;
    private float _coyoteTimeCounter;
    
    private bool _isJumpRequired;
    private bool _isJumpButtonPressing;
    private bool _isCurrentlyJumping;
    
    public event Action Jumped;

    void Awake()
    {
        _playerInput = new PlayerInputActions();
        _states = GetComponent<CharacterStates>();
        
        _gravityY = (-2 * _jumpHeight) / (_timeToJumpApex * _timeToJumpApex);
    }

    private void Start()
    {
        _character = GetComponent<Character>();
        _defaultGravity = GetGravityScale(DefaultGravityMultiplier);
        _jumpUpwardGravity = GetGravityScale(_jumpUpwardMultiplier);
        _fallGravity = GetGravityScale(_fallMultiplier);
        _jumpDownwardGravity = GetGravityScale(_jumpDownwardMultiplier);
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
        //if (_character.IsGrounded && Math.Abs(_currentGravity - _defaultGravity) > 0.05f)//NEED ?
        //{
        //    _currentGravity = _defaultGravity;
        //}

        _character.SetGravity(_currentGravity);
        
        CalculateBuffer();
        CalculateCoyoteTime();
    }

    private void FixedUpdate()
    {
        _velocity = _character.Velocity;

        if (TryJump())
        {
            //return;
        }

        if (_jumpBuffer == 0f)
        {
            _isJumpRequired = false;
        }

        _currentGravity = GetGravity();
        LimitFallSpeed();
    }

    private float GetGravity()
    {
        if (_character.Velocity.y > ComparisonError)
        {
            if (_character.IsGrounded || _character.IsTouchingWall)
            {
                return _defaultGravity;
            }

            if (_isJumpButtonPressing && _isCurrentlyJumping)
            {
                return _jumpUpwardGravity;
            }

            return _jumpDownwardGravity;
        }

        if (_character.Velocity.y < -ComparisonError)
        {
            return _character.IsGrounded ? _defaultGravity : _fallGravity;
        }

        if (_character.IsGrounded || _character.IsTouchingWall)//TODO: Need here? Maybe need it out
        {
            _isCurrentlyJumping = false;
        }

        return _defaultGravity;
    }

    private bool CanJump(States state)
    {
        return _character.IsGrounded || state == States.Slide ||
               (_coyoteTimeCounter > CoyoteError && _coyoteTimeCounter < _coyoteTime);
    }

    private bool TryJump()//TODO: If tere's no need to ckip cur fixupdate then separate taht funciton
    {
        States state = _states.GetCurrentState();

        if (!_isJumpRequired || !CanJump(state))
            return false;
        
        _character.SetGravity(_defaultGravity); //TODO: Sweetch to upjump and fix wall juumping?
        Vector2 velocity = GetJumpVelocity(state);

        if (velocity == Vector2.zero) 
            return false;
            
        //_character.SetGravity(_defaultGravity);
        Jump(velocity);
            
        return true;
    }

    private void Jump(Vector2 velocity)
    {
        _jumpBufferCounter = 0f;
        _coyoteTimeCounter = 0f;
        _isJumpRequired = false;
        _isCurrentlyJumping = true;
        //_character.SetVelocity(Vector2.zero); // NEEED ?
        Jumped?.Invoke();
        
        _velocity = velocity;
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

    private Vector2 GetVelocityFromGround()//TODO: Insted of 0 for x make it character.velocity.x?
    {
       // _character.SetGravity(_defaultGravity);
        float velocityY = Mathf.Sqrt(-2 * Physics2D.gravity.y * _character.GravityScale * _jumpHeight);
        
        return new Vector2(_character.Velocity.x, velocityY);
    }

    private Vector2 GetVelocityFromWall()
    {
        return new Vector2(-_character.FacingDirectionX * _wallJumpForce, _wallJumpForce);
    }

    private void CalculateBuffer()
    {
        if (_jumpBuffer <= 0f || _isJumpRequired == false)
            return;

        _jumpBufferCounter += Time.deltaTime;

        if (_jumpBufferCounter >= _jumpBuffer)
        {
            _isJumpRequired = false;
            _jumpBufferCounter = 0f;
        }
    }

    private void CalculateCoyoteTime()
    {
        if (_isCurrentlyJumping == false && _character.IsGrounded == false)
        {
            _coyoteTimeCounter += Time.deltaTime;
        }
        else
        {
            _coyoteTimeCounter = 0;
        }
    }

    private float GetGravityScale(float gravityMultiplier)
    {
        return (_gravityY / Physics2D.gravity.y) * gravityMultiplier;
    }

    private void LimitFallSpeed()
    {
        float maxSpeed = 100f; //TODO: Raplace with positive speed limit?
        
        Vector2 velocity = new Vector2(_velocity.x, Mathf.Clamp(_velocity.y, -_speedLimit, maxSpeed));
        
        _character.SetVelocity(velocity);
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