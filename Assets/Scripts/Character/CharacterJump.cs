using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Grounder))]

public class CharacterJump : MonoBehaviour
{
    [Header("Main stats")]
    [SerializeField, Range(1.5f, 6f)] private float _maxJumpHeight;
    [SerializeField, Range(0.1f, 2f)] private float _timeToRichApex;
    [SerializeField, Range(0f, 10f)] private float _gravityDownMultiplier;
    [SerializeField, Range(0f, 10f)] private float _gravityUpMultiplier;
    [SerializeField, Range(0f, 30f)] private float _maxFallSpeed;

    [Header("Additional stats")]
    [SerializeField] private float _coyoteTime;
    [SerializeField] private float _bufferTime;

    private Rigidbody2D _rigidbody2D;
    private Grounder _grounder;
    private PlayerInputActions _playerInput;
    
    private float _jumpSpeed;
    private Vector2 _velocity;
    private float _gravityMultiplayer = 1f;
    private float _defaultGravityScale = 1f;

    private bool _isGrounded;
    private bool _pressingJumpButton;
    private bool _wantToJump;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        _grounder = GetComponent<Grounder>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Character.Jump.started += OnJumpStarted;
        _playerInput.Character.Jump.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Character.Jump.started -= OnJumpStarted;
        _playerInput.Character.Jump.canceled -= OnJumpCanceled;
    }

    private void Update()
    {
        CalculatePhysics();

        if (_wantToJump == true)
        {
            _wantToJump = false;
        }
    }

    private void FixedUpdate()
    {
        if (_wantToJump == true)
        {
            Jump();
            _rigidbody2D.velocity = _velocity;
        }
        
        CalculateGravity();
    }

    private void CalculatePhysics()
    {
        // float x = 0;
        // float y = (-2 * _maxJumpHeight) / (_timeToRichApex * _timeToRichApex);
        // Vector2 gravity = new Vector2(0, y);
        //
        // _rigidbody2D.gravityScale = (gravity.y / Physics2D.gravity.y) / _gravityMultiplayer;
    }

    private void CalculateGravity()
    {
        
    }
    
    private void OnJumpStarted(InputAction.CallbackContext context)
    {
        _wantToJump = true;
        _pressingJumpButton = true;
        Debug.Log("wanna jump");
    }
    
    private void OnJumpCanceled(InputAction.CallbackContext context)
    {
        _pressingJumpButton = false;
        Debug.Log("Don't wanna jump");
    }

    private void Jump()
    {
        
    }
}