using System;
using System.Collections;
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

    private bool _isGrounded;
    private bool _isCurrentlyGrounded;
    private bool _isOnWall;
    private bool _isCurrentlyOnWall;
    private float _directionX;
    private bool _canMove;
    
    private CharacterStates _states;
    private PlayerInputActions _playerInput;
    public event Action<bool> GroundedChanged; //TODO: Rename
    public event Action<bool> WalledChanged;
    public event Action<bool> Hurted;

    public float DirectionX => _directionX;
    public bool CanMove => _canMove;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _playerInput.Character.Enable();
        _canMove = true;
    }

    private void Start()
    {
        _isGrounded = false;
        _isCurrentlyGrounded = false;
        _isOnWall = false;
        _isCurrentlyOnWall = false;
        _states = GetComponent<CharacterStates>();
    }

    private void OnEnable()
    {
        _playerInput.Character.Enable();
        _nightSwitcher.NightStarted += NightStarted;
    }

    private void NightStarted()
    {
        Debug.Log("night");

        _light.enabled = true;
        _trail.enabled = true;
    }

    private void OnDisable()
    {
        _playerInput.Character.Disable();
    }

    private void Update()
    {
        _directionX = _playerInput.Character.Move.ReadValue<Vector2>().x;
        //directionX = Mathf.MoveTowards(input, directionX, 5f * Time.deltaTime);

        if (_directionX != 0)
        {
            _directionX = _directionX > 0 ? 1 : -1;
        }
        else
        {
            _directionX = 0;
        }
        GetGroundState();
        GetWallState();
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
            WalledChanged?.Invoke(_isOnWall);   
        }
    }
}
