using UnityEngine;
using UnityEngine.Rendering.Universal;
using States = CharacterStates.States;

[RequireComponent(typeof(CharacterStates))]
[RequireComponent(typeof(Rigidbody2D))]

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterGroundDetector _groundDetector;
    [SerializeField] private CharacterWallDetector _wallDetector;
    [SerializeField] private Transform _respawnPoint; //TODO Make it
    [SerializeField] private NightSwitcher _nightSwitcher;
    [SerializeField] private Light2D _light;
    [SerializeField] private TrailRenderer _trail;
    
    private CharacterStates _states;
    private PlayerInputActions _playerInput;
    private Rigidbody2D _rigidbody;

    private States _currentState;
    
    private float _inputX;
    private float _facingDirectionX = 1f;
    
    private bool _canMove;

    public Vector2 Velocity => _rigidbody.velocity;
    
    public float InputX => _inputX;
    public float FacingDirectionX => _facingDirectionX;
    public float GravityScale => _rigidbody.gravityScale;
    
    public bool CanMove => _canMove;
    public bool IsTouchingWall => _wallDetector.IsTouchingWall;
    public bool IsGrounded => _groundDetector.IsGrounded;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _states = GetComponent<CharacterStates>();
        _rigidbody = GetComponent<Rigidbody2D>();
    }
    
    private void OnEnable()
    {
        _playerInput.Character.Enable();
        _nightSwitcher.NightStarted += NightStarted;
        _states.StateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        _playerInput.Character.Disable();
        _nightSwitcher.NightStarted -= NightStarted;
        _states.StateChanged -= OnStateChanged;
    }

    private void Update()
    {
        _inputX = _playerInput.Character.Move.ReadValue<Vector2>().x;

        if (_inputX != 0f)
        {
            _facingDirectionX = _inputX;
        }
    }

    public void SetVelocity(Vector2 velocity)
    {
        if (_canMove || _currentState == States.Dash)
        {
            _rigidbody.velocity = velocity;
        }
    }

    public void SetGravity(float gravityScale)
    {
        if (_canMove)
        {
            _rigidbody.gravityScale = gravityScale;
        }
    }

    private void UpdateMoveAbility()
    {
        if (_currentState == CharacterStates.States.Dash ||
            _currentState == CharacterStates.States.Grab ||
            _currentState == CharacterStates.States.DashPreparation)
        {
            _rigidbody.gravityScale = 0f;
            _rigidbody.velocity = Vector2.zero;
            _canMove = false;
        }
        else
        {
            _canMove = true;
        }
    }
    
    private void NightStarted()
    {
        Debug.Log("night");

        _light.enabled = true;
        _trail.enabled = true;
    }

    private void OnStateChanged(States state)//TODO:Rename
    {
        _currentState = state;
        UpdateMoveAbility();
    }
}
