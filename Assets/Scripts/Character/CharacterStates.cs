using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterStates : MonoBehaviour
{
    [SerializeField] private CharacterGround _grounder;
    [SerializeField] private TMP_Text _text;
    
    private CharacterMovement _movementScript;
    private Character _character;
    private CharacterJump _jumpScript;
    private CharacterDash _dashScript;
    private Rigidbody2D _rigidbody;
    private CharacterSlide _slideScript;
    private CharacterHurt _hurtScript;

    private bool _isGrounded;
    private bool _isMoving;
    private bool _isJumping;
    private bool _isDashing;
    private bool _isFalling;
    private bool _isJump;
    private bool _isSliding;
    private bool _isHurt;
    private Vector2 _velocity;

    private States _currentState;

    public event Action<States> StateChanged;
    private void Awake()
    {
        _movementScript = GetComponent<CharacterMovement>();
        _jumpScript = GetComponent<CharacterJump>();
        _dashScript = GetComponent<CharacterDash>();
        _slideScript = GetComponent<CharacterSlide>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _character = GetComponent<Character>();
        _hurtScript = GetComponent<CharacterHurt>();
        
        _jumpScript.Jumped += () => { _isJumping = true; };
        _dashScript.DashStatusChanged += isDashing =>
        {
            _isDashing = isDashing;
            //_isJumping = false;
        };
        _character.GroundedChanged += isGrounded =>
        {
            _isGrounded = isGrounded;
            //_isJumping = false;
        };

        _character.WalledChanged += b =>
        {
        };
        
        _hurtScript.Hurting += isHurt =>
        {
            _isHurt = isHurt;
        };
        _slideScript.SlidingStatusChanged += isSliding =>
        {
            _isSliding = isSliding;
        };
    }

    private void Update()
    {
        _velocity = _rigidbody.velocity;
        _isGrounded = _grounder.GetOnGround();
    
        _isMoving = _movementScript.directionX != 0;

        if (_isGrounded == false)
        {
            _isFalling = _rigidbody.velocity.y < 0f;
        }
        else
        {
            _isFalling = false;
        }

        
    }

    private void FixedUpdate()
    {
        var newState = UpdateState();
        
        if (_currentState != newState)
        {
            if (_currentState == States.Jump)
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
        if (_isHurt)
        {
            return States.Hurt;
        }
        
        if (_isDashing)
        {
            return States.Dash;
        }

        if (_isSliding)
        {
            return States.Slide;
        }

        if (_isJumping && _rigidbody.velocity.y > 0)
        {
            return States.Jump;
        }

        if (_isGrounded)
        {
            return _isMoving ? States.Move : States.Idle;
        }

        return _isFalling ? States.Fall : States.Idle;
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
        Dash = 8,
        Hurt = 9
    }
}