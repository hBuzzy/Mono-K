using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterDash : MonoBehaviour
{
    [Header("Dash stats")]
    [SerializeField, Range(40f, 55f)] private float _speed;
    [SerializeField, Range(0f, 10f)] private float _endSpeed;
    [SerializeField, Range(0f, 0.3f)] private float _duration;
    [SerializeField, Range(0f, 0.3f)] private float _cooldown;
    
    [Header("Assistance")]
    [SerializeField, Range(0f, 0.15f)] private float _preparationTime;
    [SerializeField, Range(0f, 0.3f)] private float _dashBuffer;

    private const float LeftDirection = -1f;
    private const float RightDirection = 1f;
    
    private PlayerInputActions _playerInput;
    private Character _character;
    private Coroutine _dashCoroutine;

    private bool _isDashRequired;
    private bool _canDash = true;

    private float _dashBufferCounter;

    private bool CanDash
    {
        get => _canDash;

        set
        {
            if (_canDash == value)
                return;

            _canDash = value;
            CanDashChanged?.Invoke(_canDash);
        }
    }

    public event Action<bool> PreparingDashChanged;
    public event Action<bool> DashingChanged;
    public event Action<bool> CanDashChanged;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _character = GetComponent<Character>();
    }

    private void OnEnable()
    {
        _playerInput.Character.Enable();
        _playerInput.Character.Dash.performed += OnDashPerformed;
    }

    private void OnDisable()
    {
        _playerInput.Character.Disable();
        _playerInput.Character.Dash.performed -= OnDashPerformed;
    }

    private void Update()
    {
        if (CanDash == false && _dashCoroutine == null && _character.IsGrounded)
            CanDash = true;
        
        if (_isDashRequired && CanDash)
            _dashCoroutine ??= StartCoroutine(Dash());

        CalculateBuffer();
    }
    
    private IEnumerator Dash()
    {
        _dashBufferCounter = 0f;
        _isDashRequired = false;
        
        yield return PrepareDash();

        yield return PerformDash(GetDirection());
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

        CanDash = false;
        _character.SetVelocity(direction * _speed);
        
        float startTime = Time.time;
        
        while (Time.time - startTime <= _duration)
        {
            yield return new WaitForFixedUpdate();
        }
        
        _character.SetVelocity(direction * _endSpeed);
        
        yield return new WaitForSeconds(_cooldown);
        
        _dashCoroutine = null;
        DashingChanged?.Invoke(false);
    }

    private Vector2 GetDirection()
    {
        Vector2 direction = _playerInput.Character.Move.ReadValue<Vector2>().normalized;

        if (direction != Vector2.zero) 
            return direction;

        direction.x = _character.IsFacingLeft ? LeftDirection : RightDirection;

        return direction;
    }

    private void CalculateBuffer()
    {
        if (_dashBuffer == 0f || _isDashRequired == false)
            return;

        _dashBufferCounter += Time.deltaTime;

        if (_dashBufferCounter >= _dashBuffer)
        {
            _isDashRequired = false;
            _dashBufferCounter = 0f;
        }
    }

    private void OnDashPerformed(InputAction.CallbackContext context)
    {
        _isDashRequired = true;
    }
}