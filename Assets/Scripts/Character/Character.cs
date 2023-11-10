using UnityEngine;
using States = CharacterData.States;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CharacterDeath))]
[RequireComponent(typeof(CharacterMovement))]

public class Character : MonoBehaviour
{
    [SerializeField] private Cutscenes _cutscenes;
    [SerializeField] private CharacterGroundDetector _groundDetector;
    [SerializeField] private CharacterWallDetector _wallDetector;
    
    private CharacterStateHandler _states;
    private CharacterMovement _movement;
    private Rigidbody2D _rigidbody;
    private CharacterDeath _death;
    private CharacterOutline _characterOutline;

    public Vector2 Velocity => _rigidbody.velocity;
    private States _currentState;

    private bool _canMove = true;
    private bool _isGamePaused;
    private bool _isCutsceneActive;
    
    public bool IsFacingLeft => _movement.IsFacingLeft;
    public float InputX => _movement.MoveInput.x;
    public float GravityScale => _rigidbody.gravityScale;
    public bool CanMove => _canMove;
    public bool IsTouchingWall => _wallDetector.IsTouchingWall();
    public bool IsGrounded => _groundDetector.IsGrounded;

    private void Awake()
    {
        _states = GetComponent<CharacterStateHandler>();
       
        _rigidbody = GetComponent<Rigidbody2D>();
        _death = GetComponent<CharacterDeath>();
        _movement = GetComponent<CharacterMovement>();
    }
    
    private void OnEnable()
    {
        _states.StateChanged += OnStateChanged;
        _death.DeathChanged += OnDeathChanged;
        _cutscenes.ActiveChanged += OnCutsceneActiveChanged;

        GamePause.Instance.PauseChanged += OnGamePauseChanged;
    }

    private void OnDisable()
    {
        _states.StateChanged -= OnStateChanged;
        _death.DeathChanged -= OnDeathChanged;
        _cutscenes.ActiveChanged -= OnCutsceneActiveChanged;  
 
        GamePause.Instance.PauseChanged -= OnGamePauseChanged;
    }

    public void SetVelocity(Vector2 velocity)
    {
        if (_canMove || _currentState == States.Dash || _currentState == States.DashPreparation)
        {
            _rigidbody.velocity = velocity;
        }
    }

    public void SetGravity(float gravityScale)
    {
        if (_canMove)
            _rigidbody.gravityScale = gravityScale;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public bool IsSlideOnWall(out bool isOnLeft)
    {
        if (_wallDetector.IsTouchingWall(out isOnLeft) && IsGrounded == false)
        {
            if (_movement.IsFacingLeft && isOnLeft)
                return true;
            
            if (_movement.IsFacingLeft == false && isOnLeft == false)
                return true; 

            return false;
        }
        
        return false;
    }

    private void UpdateMoveAbility()
    { 
        if (_isGamePaused || IsUnmovableState() || _isCutsceneActive)
        {
            _rigidbody.gravityScale = 0f;
            
            if (_states.CurrentState != States.Dash)
                _rigidbody.velocity = Vector2.zero;
            
            _canMove = false;
        }
        else
        {
            _rigidbody.simulated = true;
            _canMove = true;
        }
    }

    private bool IsUnmovableState()
    {
        return _currentState is States.Dash or States.Grab or States.DashPreparation or States.Die;
    }

    private void OnStateChanged(States state)
    {
        _currentState = state;
        UpdateMoveAbility();
    }

    private void OnGamePauseChanged(bool isPaused)
    {
        _isGamePaused = isPaused;
        UpdateMoveAbility();
    }

    private void OnCutsceneActiveChanged(bool isActive)
    {
        _isCutsceneActive = isActive;
    }

    private void OnDeathChanged(bool isDead)
    {
        _rigidbody.simulated = !isDead;
    }
}