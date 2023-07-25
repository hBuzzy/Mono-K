using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDash : MonoBehaviour
{
    [SerializeField] private float _speed = 32;
    [SerializeField, Range(0f, 0.5f)] private float _duration;
    [SerializeField] private float _waitAfterDash;//TODO: rename
    [SerializeField] private float _dashPreparationTime; //TODO: Rename?
    
    private Rigidbody2D _rigidbody;

    private Vector2 _velocity;
    private bool _dashNeed;
    
    private PlayerInputActions _playerInput;
    private Character _character;

    private bool _canDash = true;
    private bool _isGrounded;

    public event Action Dashed;
    public event Action PreparingDash;
    public event Action<bool> DashStatusChanged;//TODO: Remove?

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _character = GetComponent<Character>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Character.Dash.performed += OnDash;
        _character.GroundedChanged += isGrounded =>
        {
            _isGrounded = isGrounded;
            if (_isGrounded == true)
            {
                _canDash = true;
            }
        };
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Character.Dash.performed -= OnDash;
    }

    private void FixedUpdate()
    {
        //_velocity = _rigidbody.velocity;//TODO: Count only when dashing.

        if (_dashNeed && _canDash)
        {
            Dash();
            _rigidbody.velocity = _velocity;
            _dashNeed = false;
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (_canDash)
        {
            _dashNeed = true;
        }
    }
    
    private void Dash()
    {
        _rigidbody.velocity = Vector2.zero;
        _canDash = false;

        StartCoroutine(WaitDash(GetDashDirection()));
    }

    private IEnumerator WaitDash(Vector2 direction)//TODO: rename ?
    {
        Dashed?.Invoke();
        DashStatusChanged?.Invoke(true);

        _rigidbody.gravityScale = 0f;

        _velocity = direction * _speed;
        
        yield return new WaitForSeconds(_duration);
        
        _rigidbody.velocity = Vector2.zero;

        yield return new WaitForSeconds(_waitAfterDash);
        
        DashStatusChanged?.Invoke(false);

        if (_isGrounded)
            _canDash = true;
    }

    private Vector2 GetDashDirection()
    {
        int left = -1;
        int right = 1;
        
        Vector2 direction = _playerInput.Character.Move.ReadValue<Vector2>();

        if (direction == Vector2.zero)
        {
            direction.x = _character.IsFacingLeft ? left : right;
        }

        return direction;
    }
}