using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(CharacterStates))]

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterGround _grounder; //TODO: Rename
    [SerializeField] private CharacterWallChecker _waller;//TODO: Rename
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private NightSwitcher _nightSwitcher;
    [SerializeField] private Light2D _light;
    [SerializeField] private TrailRenderer _trail;
    
    private float _inputX;
    private float _facingDirectionX = 1f;

    private bool _isGrounded;
    private bool _isCurrentlyGrounded;
    private bool _isOnWall;
    private bool _isCurrentlyOnWall;
    private bool _canMove;
    
    private CharacterStates _states;
    private PlayerInputActions _playerInput;
    public event Action<bool> GroundedChanged;
    public event Action<bool> WallTouchingChanged;

    public float InputX => _inputX;
    public float FacingDirectionX => _facingDirectionX;
    public bool CanMove => _canMove;

    private void Awake()
    {
        _states = GetComponent<CharacterStates>();
        _playerInput = new PlayerInputActions();
        _playerInput.Character.Enable();
    }

    private void Start()
    {
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
        
        GetGroundState();
        GetWallState();
    }
    
    private void NightStarted()
    {
        Debug.Log("night");

        _light.enabled = true;
        _trail.enabled = true;
    }

    private void UpdateMoveState(CharacterStates.States state)
    {
        if (state == CharacterStates.States.Dash || state == CharacterStates.States.Grab)
        {
            _canMove = false;
        }
        else
        {
            _canMove = true;
        }
    }

    private void GetGroundState() //TODO: Rename
    {
        _isCurrentlyGrounded = _grounder.GetOnGround();

        if (_isGrounded != _isCurrentlyGrounded)
        {
            _isGrounded = _isCurrentlyGrounded;
            GroundedChanged?.Invoke(_isGrounded);   
        }
    }
    
    private void GetWallState() //TODO: Rename
    {
        _isCurrentlyOnWall = _waller.GetOnWall();

        if (_isOnWall != _isCurrentlyOnWall)
        {
            _isOnWall = _isCurrentlyOnWall;
            WallTouchingChanged?.Invoke(_isOnWall);   
        }
    }
}
