using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDash : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private Vector2 _velocity;
    private bool _dashNeed;
    
    private PlayerInputActions _playerInput;
    
    
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
        
        StartCoroutine(WaitDash(direction));
    }

    private IEnumerator WaitDash(Vector2 direction)
    {
        CharacterMovementBlocker.Instance.DisableMovement();
        
        var dashSpeed = 15f;
        var orGravity = _rigidbody.gravityScale;

        DOVirtual.Float(14, 0, 0.8f, RigidbodyDrag);
        
        _rigidbody.gravityScale = 0f;
        
        _velocity = direction * new Vector2(dashSpeed, dashSpeed);
        yield return new WaitForSeconds(.4f);
        
        CharacterMovementBlocker.Instance.EnableMovement();
        _rigidbody.gravityScale = orGravity;
    }

    private void RigidbodyDrag(float x)
    {
        _rigidbody.drag = x;
    }
}