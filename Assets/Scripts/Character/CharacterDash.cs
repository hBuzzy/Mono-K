using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDash : MonoBehaviour
{
    [Header("Speed")]
    [SerializeField, Range(30f, 60f)] private float _speed = 50f;
    [SerializeField, Range(0f, 40f)] private float _endSpeed = 25f;
    
    [Header("Time")]
    [SerializeField, Range(0f, 0.3f)] private float _duration;
    [SerializeField, Range(0, 0.3f)] private float _endDuration;
    
    [Header("Assistance")]
    [SerializeField, Range(0f, 0.15f)] private float _preparationTime; //TODO: Rename?
    [SerializeField, Range(0f, 0.3f)] private float _dashBuffer;
    
    private PlayerInputActions _playerInput;
    private Character _character;
    private Coroutine _dashCoroutine;

    private bool _isDashRequired;
    private bool _canDash = true;

    private float _dashBufferCounter; //TODO: Rename?
    
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
        if (_canDash == false && _dashCoroutine == null && _character.IsGrounded)
        {
            _canDash = true;
        }
        
        CalculateBuffer();
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
        _dashBufferCounter = 0f;
        _isDashRequired = false;
        _canDash = false;
        
        
        yield return PrepareDash();

        yield return PerformDash(GetDashDirection());
        
        if (_character.IsGrounded)
        {
            _canDash = true;
        }
    }

    private IEnumerator PrepareDash()
    {
        PreparingDashChanged?.Invoke(true);
        
        yield return new WaitForSeconds(_preparationTime);
        
        PreparingDashChanged?.Invoke(false);
    }

    private IEnumerator PerformDash(Vector2 direction)
    {
        DashingChanged?.Invoke(true);

        yield return new WaitForFixedUpdate();
        
        _character.SetVelocity(direction * _speed);

        yield return new WaitForSeconds(_duration);
        
        _character.SetVelocity(direction * _endSpeed);
        
        yield return new WaitForSeconds(_endDuration);

        _character.SetVelocity(Vector2.zero);

        _dashCoroutine = null;
        
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

    private void CalculateBuffer()
    {
        if (_dashBuffer == 0f || _isDashRequired == false)
        {
            return;
        }

        _dashBufferCounter += Time.deltaTime;

        if (_dashBufferCounter >= _dashBuffer)
        {
            _isDashRequired = false;
            _dashBufferCounter = 0f;
        }
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        _isDashRequired = true;
    }
}