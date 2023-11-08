using System;
using TMPro;
using UnityEngine;
using States = CharacterData.States;

[RequireComponent(typeof(Character))]

public class CharacterStateHandler : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private TMP_Text _text;
    [SerializeField] private CharacterEffects _effects;
    [SerializeField] private Animator _animator;

    [Header("Animations")]
    [SerializeField] private float _transitionDuration; 
    
    private Character _character;
    private CharacterMovement _movement;
    private CharacterJump _jump;
    private CharacterDash _dash;
    private CharacterWallMovement _wallMovement;
    private CharacterDeath _death;
    
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
        _character = GetComponent<Character>();
        _movement = GetComponent<CharacterMovement>();
        _jump = GetComponent<CharacterJump>();
        _dash = GetComponent<CharacterDash>();
        _wallMovement = GetComponent<CharacterWallMovement>();
        _death = GetComponent<CharacterDeath>();
    }

    private void OnEnable()
    {
        _jump.Jumped += OnJumped;
        _dash.DashingChanged += OnDashingChanged;
        _dash.PreparingDashChanged += OnPreparingDashChanged;
        _death.DeathChanged += OnDeathChanged;
        _wallMovement.SlidingChanged += OnSlidingChanged;
        _wallMovement.GrabbingChanged += OnGrabbingChanged;
    }

    private void OnDisable()
    {
        _jump.Jumped -= OnJumped;
        _dash.DashingChanged -= OnDashingChanged;
        _death.DeathChanged -= OnDeathChanged;
        _wallMovement.SlidingChanged -= OnSlidingChanged;
        _wallMovement.GrabbingChanged -= OnGrabbingChanged;
    }
    
    private void Update()
    {
        _velocity = _character.Velocity;

        _isMoving = _movement.MoveInput.x != 0f;

        _isFalling = (_velocity.y < 0f && _character.IsGrounded == false);
    }

    private void FixedUpdate()
    {
        CurrentState = GetCurrentState();
    }

    public States CurrentState
    {
        get => _currentState;

        private set
        {
            if (_currentState == value)
                return;
            
            if (_currentState == States.Jump)
                _isJumping = false;

            _currentState = value;
            StateChanged?.Invoke(_currentState);
            HandleState(_currentState);
        }
    }
    
    private States GetCurrentState()
    {
        if (_isHurting)
            return States.Die;

        if (_isDashing)
            return States.Dash;
        
        if (_isDashPreparation)
            return States.DashPreparation;

        if (_isGrabbing)
            return States.Grab;

        if (_isJumping && _character.Velocity.y > 0)
            return States.Jump;
        
        if (_isSliding)
            return States.Slide;

        if (_character.IsGrounded)
            return _isMoving ? States.Walk : States.Idle;

        return _isFalling ? States.Fall : States.Idle;
    }

    private void HandleState(States state)
    {
        AnimateState(CharacterData.Animations.GetAnimationHash(state));
        _effects.Play(state);
    }
    
    private void AnimateState(int animationState)
    {
        _animator.CrossFade(animationState, _transitionDuration);
    }

    private void ResetState()
    {
        _currentState = States.Default;
    }

    private void OnJumped()
    {
        _isJumping = true;
        ResetState();
    }

    private void OnDashingChanged(bool isDashing)
    {
        _isDashing = isDashing;

        if (_isDashing)
            _isGrabbing = false;
    }

    private void OnDeathChanged(bool isHurting)
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
}
