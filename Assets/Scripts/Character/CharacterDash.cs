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
    
    private PlayerInputActions _playerInput;
    private Character _character;
    private Coroutine _dashCoroutine;

    private bool _isDashRequired;
    private bool _canDash = true;
    
    public event Action<bool> PreparingDashChanged;
    public event Action<bool> DashingChanged;//TODO: Remove?

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _character = GetComponent<Character>();
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
            _dashCoroutine ??= StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        _isDashRequired = false;
        _canDash = false;
        
        Vector2 direction = GetDashDirection();
        
        yield return PrepareDash();

        yield return PerformDash(direction);
        
        if (_character.IsGrounded)
        {
            _canDash = true;
        }

        _dashCoroutine = null;
    }

    private IEnumerator PrepareDash()
    {
        PreparingDashChanged?.Invoke(true);
        
        yield return new WaitForSeconds(_dashPreparationTime);
        
        PreparingDashChanged?.Invoke(false);
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        DashingChanged?.Invoke(true);

        yield return new WaitForFixedUpdate();
        
        _character.SetVelocity(direction * _speed);

        yield return new WaitForSeconds(_duration);

        _character.SetVelocity(Vector2.zero);

        yield return new WaitForSeconds(_waitAfterDash);

        DashingChanged?.Invoke(false);   
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

    private void OnDash(InputAction.CallbackContext context)
    {
        if (_canDash)
        {
            _isDashRequired = true;
        }
    }
}