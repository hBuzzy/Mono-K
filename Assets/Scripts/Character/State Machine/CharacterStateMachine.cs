using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterStateMachine : MonoBehaviour
{
    private CharacterBaseState _currentState;
    private CharacterStateFactory _states;
    
    private PlayerInputActions _playerInput;

    private bool _isJumpPressed;

    public Rigidbody2D Rigidbody;

    [SerializeField] private CharacterGround _grounder;
    
    public bool IsJumpPressed => _isJumpPressed;
    public bool IsGrounded => _grounder.GetOnGround();
    public float DirectionX;

    public CharacterBaseState CurrentState => _currentState;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _states = new CharacterStateFactory(this);
        _currentState = _states.Grounded();
        _currentState.Enter();
    }

    private void OnEnable()
    {
        _playerInput.Character.Enable();
        _playerInput.Character.Jump.started += OnJumpStarted;
        _playerInput.Character.Jump.canceled += OnJumpCanceled;
    }

    private void OnDisable()
    {
        _playerInput.Character.Enable();
        _playerInput.Character.Jump.started -= OnJumpStarted;
        _playerInput.Character.Jump.canceled -= OnJumpCanceled;
    }

    private void Update()
    {
        DirectionX = _playerInput.Character.Move.ReadValue<Vector2>().x;

        if (DirectionX != 0)
        {
            DirectionX = DirectionX > 0 ? 1 : -1;
        }
        
        _currentState.UpdateState();
    }

    public void SetCurrentState(CharacterBaseState state)
    {
        _currentState = state;
    }
    
    private void OnJumpCanceled(InputAction.CallbackContext obj)
    {
        _isJumpPressed = false;
    }

    private void OnJumpStarted(InputAction.CallbackContext obj)
    {
        _isJumpPressed = true;
    }
}