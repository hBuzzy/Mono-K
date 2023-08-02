using System;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(CharacterJump))]
[RequireComponent(typeof(CharacterDash))]
[RequireComponent(typeof(CharacterWallMovement))]
[RequireComponent(typeof(CharacterHurt))]

public class CharacterStates : MonoBehaviour
{
    [SerializeField] private CharacterGroundDetector _grounder;
    [SerializeField] private TMP_Text _text;

    private Character _character;
    private Rigidbody2D _rigidbody;
    private CharacterMovement _movementScript;
    private CharacterJump _jumpScript;
    private CharacterDash _dashScript;
    private CharacterWallMovement _wallMovementScript;
    private CharacterHurt _hurtScript;
    
    private bool _isMoving;
    private bool _isJumping;
    private bool _isDashPreparation;
    private bool _isDashing;
    private bool _isFalling;
    private bool _isSliding;
    private bool _isHurting;
    private bool _isGrabbing;

    private Vector2 _velocity;
    private States _currentState;

    public event Action<States> StateChanged;
    
    private void Awake()
    {
        _movementScript = GetComponent<CharacterMovement>();
        _jumpScript = GetComponent<CharacterJump>();
        _dashScript = GetComponent<CharacterDash>();
        _wallMovementScript = GetComponent<CharacterWallMovement>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
        _hurtScript = GetComponent<CharacterHurt>();
    }

    private void OnEnable()
    {
        _jumpScript.Jumped += OnJumped;
        _dashScript.DashingChanged += OnDashingChanged;
        _dashScript.PreparingDashChanged += OnPreparingDashChanged;
        _hurtScript.HurtingChanged += OnHurtingChanged;
        _wallMovementScript.SlidingChanged += OnSlidingChanged;
        _wallMovementScript.GrabbingChanged += OnGrabbingChanged;
    }

    private void OnDisable()
    {
        _jumpScript.Jumped -= OnJumped;
        _dashScript.DashingChanged -= OnDashingChanged;
        _hurtScript.HurtingChanged -= OnHurtingChanged;
        _wallMovementScript.SlidingChanged -= OnSlidingChanged;
        _wallMovementScript.GrabbingChanged -= OnGrabbingChanged;
    }

    private void Update()
    {
        _velocity = _rigidbody.velocity;

        _isMoving = _movementScript.MovementDirectionX != 0f;

        _isFalling = (_velocity.y < 0f && _character.IsGrounded == false);
    }

    private void FixedUpdate()
    {
        var newState = UpdateState();
        
        if (_currentState != newState)
        {
            if (_currentState == States.Jump)//TODO: Now need cuz base script isn't reset that value. Fix base script logic
                _isJumping = false;
            
            _currentState = newState;
            StateChanged?.Invoke(_currentState);
            _text.text = _currentState.ToString();
        }
    }

    public States GetCurrentState()
    {
        return _currentState;
    }

    private States UpdateState()
    {
        if (_isHurting)
        {
            return States.Hurt;
        }

        if (_isDashing)
        {
            return States.Dash;
        }
        
        if (_isDashPreparation)
        {
            return States.DashPreparation;
        }

        if (_isGrabbing)
        {
            return States.Grab;
        }

        if (_isSliding)
        {
            return States.Slide;
        }

        if (_isJumping && _rigidbody.velocity.y > 0)
        {
            return States.Jump;
        }

        if (_character.IsGrounded)
        {
            return _isMoving ? States.Move : States.Idle;
        }

        return _isFalling ? States.Fall : States.Idle;
    }

    private void OnJumped()
    {
        _isJumping = true;
    }

    private void OnDashingChanged(bool isDashing)
    {
        _isDashing = isDashing;

        if (_isDashing == true)
        {
            _isGrabbing = false;
        }
    }

    private void OnHurtingChanged(bool isHurting)
    {
        _isHurting = isHurting;
    }

    private void OnSlidingChanged(bool isSliding)
    {
        _isSliding = isSliding;
    }

    private void OnGrabbingChanged(bool isGrabbing)
    {
        _isGrabbing = isGrabbing;
    }

    private void OnPreparingDashChanged(bool isDashPreparation)
    {
        _isDashPreparation = isDashPreparation;
    }

    public enum States
    {
        Idle = 1,
        Move = 2,
        Jump = 3,
        JumpStart = 4,
        Land = 5,
        Fall = 6,
        Slide =7,
        DashPreparation = 8,
        Dash = 9,
        Hurt = 10,
        Grab = 11
    }
}