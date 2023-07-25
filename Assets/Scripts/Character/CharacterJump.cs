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
    
    private bool _desiredJump;
    private bool _pressingJump;
    private bool _isCurrentlyJumping;
    private bool _isGrounded;
    private bool _isTouchingWall;
    
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
        if (_character.CanMove)
        {
            SetPhysics();
        }

        CalculateBuffer();
    }

    private void SetPhysics()
    {
        Vector2 newGravity = new Vector2(0, (-2 * _jumpHeight) / (_timeToJumpApex * _timeToJumpApex));

        float gravityY = (-2 * _jumpHeight) / (_timeToJumpApex * _timeToJumpApex);//TODO: use it instead of Vector2?
        
        _rigidbody.gravityScale = (newGravity.y / Physics2D.gravity.y) * _gravityMultiplier;
    }

    private void FixedUpdate()
    {
        _velocity = _rigidbody.velocity;

        if (_desiredJump)
        {
            Jump();
            return;
        }

        CalculateGravity();
    }

    private void OnJumpStarted(InputAction.CallbackContext context)
    {
        _desiredJump = true;
        _pressingJump = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        _pressingJump = false;
    }

    private void CalculateGravity()
    {
        if (_rigidbody.velocity.y > 0.01f)
        {
            if (_isGrounded)
            {
                _gravityMultiplier = _defaultGravityScale;
            }
            else
            {
                if (_pressingJump && _isCurrentlyJumping)
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
            if (_isGrounded)
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
            if (_isGrounded || _isTouchingWall)//TODO: Добавить на стене
            {
                _isCurrentlyJumping = false;
            }

            _gravityMultiplier = _defaultGravityScale;
        }

        _rigidbody.velocity = new Vector3(_velocity.x, Mathf.Clamp(_velocity.y, -_speedLimit, 100));
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

        if (_jumpBuffer == 0)
        {
            _desiredJump = false;
        }
        _rigidbody.velocity = _velocity;
    }

    private void JumpFromGround()
    {
        Jumped?.Invoke();
        _jumpBufferCounter = 0;
        _desiredJump = false;

        var rem = _jumpSpeed;

        _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * _rigidbody.gravityScale * _jumpHeight);
        
        if (_velocity.y > 0f) {
            _jumpSpeed = Mathf.Max(_jumpSpeed - _velocity.y, 0f);
        }
        else if (_velocity.y < 0f) {
            _jumpSpeed += Mathf.Abs(_rigidbody.velocity.y);
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
        //_rigidbody.velocity = Vector2.zero;
        
        Jumped?.Invoke();
        _jumpBufferCounter = 0;
        _desiredJump = false;
        
        if (_character.IsFacingLeft)
        {
            _velocity += new Vector2(_wallJumpForce, _wallJumpForce);
        }
        else
        {
            _velocity += new Vector2(-_wallJumpForce, _wallJumpForce);
        }
        
        _isCurrentlyJumping = true;
    }

    private void CalculateBuffer()
    {
        if (_jumpBuffer <= 0 || _desiredJump == false)
        {
            return;
        }

        _jumpBufferCounter += Time.deltaTime;

        if (_jumpBufferCounter > _jumpBuffer)
        {
            _desiredJump = false;
            _jumpBufferCounter = 0;
        }
    }
}