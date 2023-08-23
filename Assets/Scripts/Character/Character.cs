using UnityEngine;
using UnityEngine.Rendering.Universal;
using States = CharacterStates.States;

[RequireComponent(typeof(Rigidbody2D))]

public class Character : MonoBehaviour
{
    [SerializeField] private CharacterGroundDetector _groundDetector;
    [SerializeField] private CharacterWallDetector _wallDetector;
    [SerializeField] private Transform _respawnPoint; //TODO Make it
    [SerializeField] private NightSwitcher _nightSwitcher;
    [SerializeField] private Light2D _light;

    private CharacterStates _states;
    private PlayerInputActions _playerInput;
    private Rigidbody2D _rigidbody;
    private CharacterDash _dashScript; //NeedBeHere?
    private Outline _outlineScript;

    private States _currentState;
    
    private float _inputX;
    private float _facingDirectionX = 1f;
    
    private bool _canMove = true;
    private bool _isGamePaused;

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
       // _states = GetComponent<CharacterStateHandler>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _dashScript = GetComponent<CharacterDash>();
    }
    
    private void OnEnable()
    {
        _playerInput.Character.Enable();
        _nightSwitcher.NightStarted += NightStarted;
        _states.StateChanged += OnStateChanged;

        GamePause.Instance.PauseChanged += OnGamePauseChanged;
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

    public void AddForce(Vector2 force)
    {
        _rigidbody.AddForce(force, ForceMode2D.Impulse);
    }


    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    private void UpdateMoveAbility()
    {
        if (_isGamePaused || _currentState is States.Dash or States.Grab or States.DashPreparation or States.Hurt)
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

    private void NightStarted()//TODO: Refactoring
    {
        Debug.Log("night");

        _light.enabled = true;
        _dashScript.enabled = true;
        //_outlineScript.enabled = true;
        //_trail.enabled = true;
    }

    private void OnStateChanged(States state)//TODO:Rename
    {
        _currentState = state;
        UpdateMoveAbility();
    }

    private void OnGamePauseChanged(bool isPaused)
    {
        _isGamePaused = isPaused;
        UpdateMoveAbility();
    }
}
