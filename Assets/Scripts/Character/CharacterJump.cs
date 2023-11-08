using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterJump : MonoBehaviour
{
    [Header("Components")] 
    [SerializeField] private CharacterMovement _movement;
    
    [Header("Jump stats")]
    [SerializeField, Range(2f, 4f)] private float _jumpHeight;
    [SerializeField, Range(0.2f, 0.8f)] private float _timeToReachApex;
    [SerializeField, Range(10f, 25f)] private float _wallJumpForce;

    [Header("Gravity multipliers")] 
    [SerializeField, Range(0f, 5f)] private float _jumpUpwardMultiplier;
    [SerializeField, Range(0f, 10f)] private float _jumpDownwardMultiplier;
    [SerializeField, Range(1f, 10f)] private float _fallMultiplier;
    
    [Header("Assists")]
    [SerializeField, Range(13f, 22f)] private float _maxFallSpeed;
    [SerializeField, Range(0f, 0.2f)] private float _jumpBuffer;
    [SerializeField, Range(0f, 0.2f)] private float _coyoteTime;
    
    private const float LeftDirection = -1f;
    private const float RightDirection = 1f;
    private const float DefaultGravityMultiplier = 1f;
    private const float ComparisonError = 0.03f;
    private const float CoyoteError = 0.03f;
    
    private PlayerInputActions _playerInput;
    private Character _character;

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
    private bool _hasJumps; 
    
    public event Action Jumped;

    void Awake()
    {
        _playerInput = new PlayerInputActions();
        
        _gravityY = (-2 * _jumpHeight) / (_timeToReachApex * _timeToReachApex);
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
        CalculateBuffer();
        CalculateCoyoteTime();
    }

    private void FixedUpdate()
    {
        if (_character.CanMove == false)
            return;
        
        _character.SetGravity(_currentGravity);
        
        if ((_character.IsGrounded || _character.IsTouchingWall))
            _hasJumps = true;

        if (_isJumpRequired && CanJump())
        {
            _character.SetGravity(_defaultGravity);
            Vector2 velocity = GetJumpVelocity();
                
            if (velocity != Vector2.zero)
                Jump(velocity);
        }

        if (_jumpBuffer == 0f)
            _isJumpRequired = false;

        _currentGravity = GetGravity();
        
        if (_currentGravity == _jumpUpwardGravity || _currentGravity == _defaultGravity)
            return;

        _isCurrentlyJumping = false;
        
        LimitFallSpeed();
    }

    private float GetGravity()
    {
        if (_character.Velocity.y > ComparisonError)
        {
            if (_character.IsGrounded)
                return _defaultGravity;

            if (_isJumpButtonPressing && _isCurrentlyJumping)
                return _jumpUpwardGravity;

            return _jumpDownwardGravity;
        }

        if (_character.Velocity.y < -ComparisonError)
            return _character.IsGrounded ? _defaultGravity : _fallGravity;

        return _defaultGravity;
    }

    private bool CanJump()
    {
        return _hasJumps && (_character.IsGrounded || _character.IsTouchingWall || HasCoyoteTime());
    }

    private void Jump(Vector2 velocity)
    {
        _jumpBufferCounter = 0f;
        _coyoteTimeCounter = 0f;
        _hasJumps = false;
        _isJumpRequired = false;
        _isCurrentlyJumping = true;

        Jumped?.Invoke();
        
        _character.SetVelocity(velocity);
    }

    private Vector2 GetJumpVelocity()
    {
        if (_character.IsGrounded)
            return GetVelocityFromGround();

        if (_character.IsTouchingWall)
            return GetVelocityFromWall();
        
        return HasCoyoteTime() ? GetVelocityFromGround() : Vector2.zero;
    }

    private Vector2 GetVelocityFromGround()
    {
        float velocityY = Mathf.Sqrt(-2 * Physics2D.gravity.y * _character.GravityScale * _jumpHeight);
        
        return new Vector2(_character.Velocity.x, velocityY);
    }
 
    private Vector2 GetVelocityFromWall()
    {
        float direction;
        
        if (_character.IsSlideOnWall(out bool isOnLeft))
            direction = -(isOnLeft ? LeftDirection : RightDirection);
        else
            direction = _character.IsFacingLeft ? LeftDirection : RightDirection;

        return new Vector2(direction * _wallJumpForce, _wallJumpForce);
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
        if (_isCurrentlyJumping == false && _character.IsTouchingWall == false && _character.IsGrounded == false)
            _coyoteTimeCounter += Time.deltaTime;
        else
            _coyoteTimeCounter = 0;
    }

    private bool HasCoyoteTime()
    {
        return _coyoteTimeCounter > CoyoteError && _coyoteTimeCounter <= _coyoteTime;
    } 

    private float GetGravityScale(float gravityMultiplier)
    {
        return (_gravityY / Physics2D.gravity.y) * gravityMultiplier;
    }

    private void LimitFallSpeed()
    {
        Vector2 newVelocity = new Vector2(_character.Velocity.x, Mathf.Max(_character.Velocity.y, -_maxFallSpeed));
        
        _character.SetVelocity(newVelocity);
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