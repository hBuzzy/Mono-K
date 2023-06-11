using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDash : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField, Range(0f, 20f)] private float _maxDrag;
    [SerializeField, Range(0f, 20f)] private float _minDrag;
    [SerializeField, Range(0f, 3f)] private float _dragChangeTime;
    [SerializeField, Range(0f, 0.5f)] private float _duration;
    
    private Rigidbody2D _rigidbody;

    private Vector2 _velocity;
    private bool _dashNeed;
    
    private PlayerInputActions _playerInput;
    private Character _character;

    private bool _canDash = true;
    private bool _isGrounded;

    public event Action Dashed;
    public event Action<bool> DashStatusChanged;
    
    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _character = GetComponent<Character>();
    }

    private void Start()
    {
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
        _velocity = _rigidbody.velocity;

        if (_dashNeed && _canDash)
        {
            //_rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
            Dash();
            _rigidbody.velocity = _velocity;
            _dashNeed = false;
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        _dashNeed = true;
    }
    
    private void Dash()
    {
        _rigidbody.velocity = Vector2.zero;
        Vector2 direction = _playerInput.Character.Move.ReadValue<Vector2>().normalized;

        _canDash = false;

        Dashed?.Invoke();
        Camera.main.transform.DOShakePosition(0.2f, 0.5f, 14, 90, false, true);
        StartCoroutine(WaitDash(direction));
    }

    private IEnumerator WaitDash(Vector2 direction)
    {
        //CharacterMovementBlocker.Instance.DisableMovement();
        
        DashStatusChanged.Invoke(true);
        
        var orGravity = _rigidbody.gravityScale;

        DOVirtual.Float(_maxDrag, _minDrag, _dragChangeTime, RigidbodyDrag);
        
        _rigidbody.gravityScale = 0f;
        
        _velocity = direction * new Vector2(_speed, _speed);
        yield return new WaitForSeconds(_duration);
        
        //CharacterMovementBlocker.Instance.EnableMovement();
        
        DashStatusChanged.Invoke(false);
        
        if (_isGrounded)
            _canDash = true;
        
        //_rigidbody.gravityScale = 0.8f;
    }

    private void RigidbodyDrag(float x)
    {
        _rigidbody.drag = x;
    }
}