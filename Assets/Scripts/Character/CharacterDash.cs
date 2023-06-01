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
    [SerializeField, Range(0f, 1f)] private float _dragChangeTime;
    
    
    private Rigidbody2D _rigidbody;

    private Vector2 _velocity;
    private bool _dashNeed;
    
    private PlayerInputActions _playerInput;

    public event Action Dashed;
    
    private void Awake()
    {
        _playerInput = new PlayerInputActions();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _playerInput.Enable();
        _playerInput.Character.Dash.performed += OnDash;
    }

    private void OnDisable()
    {
        _playerInput.Disable();
        _playerInput.Character.Dash.performed -= OnDash;
    }

    private void FixedUpdate()
    {
        _velocity = _rigidbody.velocity;

        if (_dashNeed)
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, 0f);
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
        
        Dashed?.Invoke();

        StartCoroutine(WaitDash(direction));
    }

    private IEnumerator WaitDash(Vector2 direction)
    {
        CharacterMovementBlocker.Instance.DisableMovement();
        
        var orGravity = _rigidbody.gravityScale;

        DOVirtual.Float(_maxDrag, _minDrag, _dragChangeTime, RigidbodyDrag);
        
        _rigidbody.gravityScale = 0f;
        
        _velocity = direction * new Vector2(_speed, _speed);
        yield return new WaitForSeconds(.1f);
        
        CharacterMovementBlocker.Instance.EnableMovement();
        _rigidbody.gravityScale = 1f;
    }

    private void RigidbodyDrag(float x)
    {
        _rigidbody.drag = x;
    }
}