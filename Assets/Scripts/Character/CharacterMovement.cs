using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private SpriteRenderer _spriteRenderer;
    
    [Header("Speed settings")]
    [SerializeField, Range(0f, 20f)] private float _maxMoveSpeed;
    [SerializeField, Range(0f, 100f)] private float _turnSpeed;
    [SerializeField, Range(0f, 100f)] private float _airTurnSpeed;
    [SerializeField, Range(0f, 15f)] private float _friction;
    
    [Header("Acceleration settings")]
    [SerializeField, Range(0f, 100f)] private float _acceleration;
    [SerializeField, Range(0f, 100f)] private float _deceleration;
    [SerializeField, Range(0f, 100f)] private float _airAcceleration;
    [SerializeField, Range(0f, 100f)] private float _airDeceleration;
    
    private PlayerInputActions _playerInput;
    private CharacterStateHandler _states;
    private Character _character;
    
    private Vector2 _desiredVelocity;
    private Vector2 _platformVelocity;
    private Vector2 _moveInput;
    
    private bool _isOnPlatform;
    private bool _isFacingLeft;

    public Vector2 MoveInput => _moveInput;
    public bool IsFacingfLeft => _isFacingLeft;
    
    private void Awake()
    {
        _playerInput = new PlayerInputActions();
    }

    private void Start()
    {
        _character = GetComponent<Character>();
        _states = GetComponent<CharacterStateHandler>();

        _isFacingLeft = false;
    }

    private void OnEnable()
    {
        _playerInput.Character.Enable();
    }
    
    private void OnDisable()
    {
        _playerInput.Character.Disable();
    }

    private void Update()
    {
        _moveInput = _playerInput.Character.Move.ReadValue<Vector2>();
        
        if (CanFlip())
            Flip();
        
        _desiredVelocity = new Vector2(_moveInput.x, 0f) * Mathf.Max(_maxMoveSpeed - _friction, 0f);
    }

    private void FixedUpdate()
    {
        if (_character.CanMove == false) 
            return;
        
        Move(_character.Velocity);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out MovingPlatform movingPlatform))
        {
            _isOnPlatform = true;
            _platformVelocity = movingPlatform.Velocity;
        }
    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out MovingPlatform movingPlatform))
        {
            _platformVelocity = movingPlatform.Velocity;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out MovingPlatform movingPlatform))
        {
            _isOnPlatform = false;
            _platformVelocity = Vector3.zero;
        }
    }

    private void Move(Vector2 velocity)
    {
        if (_isOnPlatform)
            _desiredVelocity += _platformVelocity;
        
        velocity.x = Mathf.MoveTowards(velocity.x, _desiredVelocity.x, GetSpeedChange(velocity));
        
        _character.SetVelocity(velocity);
    }

    private float GetSpeedChange(Vector2 velocity)
    {
        bool isGrounded = _character.IsGrounded;
        float acceleration = isGrounded ? _acceleration : _airAcceleration;
        float deceleration = isGrounded ? _deceleration : _airDeceleration;
        float turnSpeed = isGrounded ? _turnSpeed : _airTurnSpeed;
        
        if (_moveInput.x == 0)
            return deceleration * Time.deltaTime;

        if (Mathf.Sign(_moveInput.x) != Mathf.Sign(velocity.x))
            return turnSpeed * Time.deltaTime;

        return acceleration * Time.deltaTime;
    }

    private void Flip()
    {
        if (_moveInput.x > 0 && _isFacingLeft)
        {
            _isFacingLeft = false;
            _spriteRenderer.flipX = false;
        }
        else if (_moveInput.x < 0 && _isFacingLeft == false)
        {
            _isFacingLeft = true;
            _spriteRenderer.flipX = true;
        }
    }

    private bool CanFlip()
    {
        return _moveInput.x != 0 && _states.CurrentState is not
            (CharacterData.States.Slide or CharacterData.States.Grab);
    }
}