using System;
using UnityEngine;
using UnityEngine.InputSystem;
using States = CharacterStates.States;

public class CharacterWallMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 0.5f)] private float _slidingSpeed;
    
    private Character _character;
    private Rigidbody2D _rigidbody;
    private PlayerInputActions _playerInput;
    private CharacterStates _states;
    private CharacterMovement _characterMovement;

    private bool _isTouchingWall;
    private bool _isGrounded;
    private bool _isSliding;
    private bool _isGrabbing;
    private bool _isGrabRequired;
    
    #region Properties

    private bool IsSliding
    {
        get => _isSliding;
        
        set
        {
            if (_isSliding == value)
                return;

            _isSliding = value;
            SlidingChanged?.Invoke(_isSliding);
        }
    }

    private bool IsGrabbing
    {
        get => _isGrabbing;
        
        set
        {
            if (_isGrabbing == value)
                return;

            _isGrabbing = value;
            GrabbingChanged?.Invoke(_isGrabbing);
        }
    }

    #endregion

    public event Action<bool> SlidingChanged;
    public event Action<bool> GrabbingChanged;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        
        _rigidbody = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
        _states = GetComponent<CharacterStates>();
        _characterMovement = GetComponent<CharacterMovement>();
    }
    
    private void OnEnable()
    {
        _playerInput.Character.Enable();
        
        _playerInput.Character.Grab.started += OnGrabStarted;
        _playerInput.Character.Grab.canceled += OnGrabCanceled;
        _character.WallTouchingChanged += OnWallTouchingChanged;
        _character.GroundedChanged += OnGroundedChanged;
    }

    private void OnDisable()
    {
        _playerInput.Character.Disable();
        _playerInput.Character.Grab.started -= OnGrabStarted;
        _playerInput.Character.Grab.canceled -= OnGrabCanceled;
        _character.WallTouchingChanged -= OnWallTouchingChanged;
        _character.GroundedChanged -= OnGroundedChanged;
    }

    private void Update()
    {
        States characterState = _states.GetCurrentState();

        if (characterState == States.Dash)
        {
            _isGrabRequired = false; 
            IsGrabbing = false;
            IsSliding = false;
        }

        UpdateStates();

        if (IsGrabbing)
        {
            _rigidbody.gravityScale = 0f;
            _rigidbody.velocity = Vector2.zero;
        }
        else if (IsSliding)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x,
                Mathf.Clamp(_rigidbody.velocity.y, -_slidingSpeed, float.MaxValue));
        }
    }

    private void UpdateStates()
    {
        if (_isTouchingWall && _isGrounded == false)
        {
            if (_isGrabRequired && IsGrabbing == false)
            {
                _isGrabRequired = false;
                IsGrabbing = true;
            }
            else
            {
                if (_characterMovement.MovementDirectionX != 0 && _rigidbody.velocity.y <= 0)
                {
                    IsSliding = true;
                }
                else
                {
                    IsSliding = false;
                }
            }
        }
        else
        { 
            IsSliding = false;
            IsGrabbing = false;
        }
    }

    private void OnGrabStarted(InputAction.CallbackContext context)
    {
        _isGrabRequired = true;
    }

    private void OnGrabCanceled(InputAction.CallbackContext context)
    {
        _isGrabRequired = false;
        IsGrabbing = false;
    }

    private void OnWallTouchingChanged(bool isTouchingWall)
    {
        _isTouchingWall = isTouchingWall;
    }

    private void OnGroundedChanged(bool isGrounded)
    {
        _isGrounded = isGrounded;
    }
}
