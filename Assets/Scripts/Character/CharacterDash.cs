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
    private bool _isDashRequired;
    
    private PlayerInputActions _playerInput;
    private Character _character;

    private bool _canDash = true;

    public event Action Dashed;
    public event Action PreparingDash;
    public event Action<bool> DashingChanged;//TODO: Remove?

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _character = GetComponent<Character>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        _playerInput.Character.Enable();
        _playerInput.Character.Dash.performed += OnDash;
    }

    private void OnDisable()
    {
        _playerInput.Character.Disable();
        _playerInput.Character.Dash.performed -= OnDash;
    }

    private void Update()
    {
        if (_canDash == false && _character.IsGrounded)
        {
            _canDash = true;
        }
    }

    private void FixedUpdate()
    {
        if (_isDashRequired && _canDash)
        {
            Dash();
            _rigidbody.velocity = _velocity;
            _isDashRequired = false;
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (_canDash)
        {
            _isDashRequired = true;
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
        DashingChanged?.Invoke(true);

        _rigidbody.gravityScale = 0f;

        _velocity = direction * _speed;
        
        yield return new WaitForSeconds(_duration);
        
        _rigidbody.velocity = Vector2.zero;

        yield return new WaitForSeconds(_waitAfterDash);
        
        DashingChanged?.Invoke(false);

        if (_character.IsGrounded)
        {
            _canDash = true;
        }
    }

    private Vector2 GetDashDirection()
    {
        Vector2 direction = _playerInput.Character.Move.ReadValue<Vector2>().normalized;

        if (direction == Vector2.zero)
        {
            direction.x = _character.FacingDirectionX;
        }

        return direction;
    }
}