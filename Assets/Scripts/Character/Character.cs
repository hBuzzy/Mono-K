using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterStates))]

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterGround _groundDetector; //TODO: Rename
    [SerializeField] private CharacterWallChecker _wallDetector;//TODO: Rename
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private NightSwitcher _nightSwitcher;
    [SerializeField] private Light2D _light;
    [SerializeField] private TrailRenderer _trail;
    
    private CharacterStates _states;
    private PlayerInputActions _playerInput;
    
    private float _inputX;
    private float _facingDirectionX = 1f;
    
    private bool _canMove;

    public float InputX => _inputX;
    public float FacingDirectionX => _facingDirectionX;
    
    public bool CanMove => _canMove;
    public bool IsTouchingWall => _wallDetector.IsTouchingWall;//TODO: Refactor checkers
    public bool IsGrounded => _groundDetector.GetOnGround();

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _states = GetComponent<CharacterStates>();
    }
    
    private void OnEnable()
    {
        _playerInput.Character.Enable();
        _nightSwitcher.NightStarted += NightStarted;
        _states.StateChanged += UpdateMoveState;
    }

    private void OnDisable()
    {
        _playerInput.Character.Disable();
        _nightSwitcher.NightStarted -= NightStarted;
        _states.StateChanged -= UpdateMoveState;
    }

    private void Update()
    {
        _inputX = _playerInput.Character.Move.ReadValue<Vector2>().x;

        if (_inputX != 0f)
        {
            _facingDirectionX = _inputX;
        }
    }
    
    private void NightStarted()
    {
        Debug.Log("night");

        _light.enabled = true;
        _trail.enabled = true;
    }

    private void UpdateMoveState(CharacterStates.States state)//TODO:Rename
    {
        if (state == CharacterStates.States.Dash || state == CharacterStates.States.Grab || state == CharacterStates.States.DashPreparation)
        {
            _canMove = false;
        }
        else
        {
            _canMove = true;
        }
    }
}
