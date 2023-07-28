using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterWallMovement : MonoBehaviour
{
    private Character _character;
    private Rigidbody2D _rigidbody;
    private PlayerInputActions _playerInput;
    private CharacterStates _states;
    
    
    private bool _isOnWall;
    private bool _isGrounded;
    private bool _isSliding;
    private bool _isSlidingRem;
    private bool _isGrabbing;
    private bool _desiredGrab;

    public event Action<bool> SlidingStatusChanged;
    public event Action<bool> GrabbingStatusChanged;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _rigidbody = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
        _states = GetComponent<CharacterStates>();
    }
    
    private void OnEnable()
    {
        _playerInput.Character.Enable();
        _playerInput.Character.Grab.started += OnGrabStarted;
        _playerInput.Character.Grab.canceled += OnGrabCanceled;
        
        _character.WalledChanged += isOnWall => { _isOnWall = isOnWall; };
        _character.GroundedChanged += isGrounded => { _isGrounded = isGrounded; };
    }


    private void OnDisable()
    {
        _playerInput.Character.Disable();
        _playerInput.Character.Grab.started -= OnGrabStarted;
        _playerInput.Character.Grab.canceled -= OnGrabCanceled;
    }

    private void Update()
    {
        var state = _states.GetCurrentState();

        if (state == CharacterStates.States.Dash)
        {
            _desiredGrab = false; 
            _isGrabbing = false;
            _isSliding = false;
        }

        if (_isOnWall == true && _isGrounded == false)
        {
            if (_desiredGrab == true)
            {
                _isGrabbing = true;
                _desiredGrab = false;
                GrabbingStatusChanged?.Invoke(_isGrabbing);
            }
            else
            {
                if (_character.DirectionX != 0 && _rigidbody.velocity.y <= 0)
                {
                    _isSliding = true;
                }
                else
                {
                    _isSliding = false;
                }
            }
        }
        else
        {
            _isSliding = false;
            _isGrabbing = false;
        }

        if (_isSliding != _isSlidingRem)
        {
            _isSlidingRem = _isSliding;
            SlidingStatusChanged?.Invoke(_isSliding);
        }

        if (_isGrabbing)
        {
            _rigidbody.gravityScale = 0f;
            _rigidbody.velocity = Vector2.zero;
        }
        else if (_isSliding)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, Mathf.Clamp(_rigidbody.velocity.y, -0.7f, float.MaxValue));
        }
    }

    private void OnGrabStarted(InputAction.CallbackContext obj)
    {
        _desiredGrab = true;
    }

    private void OnGrabCanceled(InputAction.CallbackContext obj)
    {
        _desiredGrab = false;
        _isGrabbing = false;
        GrabbingStatusChanged?.Invoke(_isGrabbing);
    }
}
