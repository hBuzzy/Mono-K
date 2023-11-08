using System;
using UnityEngine;
using UnityEngine.InputSystem;
using States = CharacterData.States;

public class CharacterWallMovement : MonoBehaviour
{
    [SerializeField, Range(0f, 0.5f)] private float _slidingSpeed;
    
    private Character _character;
    private PlayerInputActions _playerInput;
    private CharacterStateHandler _states;
    
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
        
        _character = GetComponent<Character>();
        _states = GetComponent<CharacterStateHandler>();
    }
    
    private void OnEnable()
    {
        _playerInput.Character.Enable();
        
        _playerInput.Character.Grab.started += OnGrabStarted;
        _playerInput.Character.Grab.canceled += OnGrabCanceled;
    }

    private void OnDisable()
    {
        _playerInput.Character.Disable();
        _playerInput.Character.Grab.started -= OnGrabStarted;
        _playerInput.Character.Grab.canceled -= OnGrabCanceled;
    }

    private void Update()
    {
        States characterState = _states.CurrentState;

        if (characterState == States.Dash)
        {
            _isGrabRequired = false; 
            IsGrabbing = false;
            IsSliding = false;
        }

        UpdateWallStates();
        
        if (IsSliding)
            Slide();
    }

    private void UpdateWallStates()
    {
        if (_character.IsSlideOnWall(out bool isLeft) && _character.Velocity.y <= 0)
        {
            if (_isGrabRequired && IsGrabbing == false)
            {
                _isGrabRequired = false;
                IsGrabbing = true;
            }
            else
            {
                IsSliding = _character.InputX != 0f;
            }
        }
        else
        {
            IsSliding = false;
            IsGrabbing = false;
        }
    }

    private void Slide()
    {
        Vector2 velocity = _character.Velocity;
        velocity = new Vector2(velocity.x, Mathf.Clamp(velocity.y, -_slidingSpeed, float.MaxValue));
        
        _character.SetVelocity(velocity);
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
}