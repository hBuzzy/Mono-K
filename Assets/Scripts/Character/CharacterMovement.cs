using System;
using UnityEngine;
using UnityEngine.Serialization;

public class CharacterMovement : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField, Range(0f, 20f)] private float _maxMoveSpeed;
    [SerializeField, Range(0f, 100f)] private float _turnSpeed;
    [SerializeField, Range(0f, 100f)] private float _airTurnSpeed;
    
    [Header("Accelerations")]
    [SerializeField, Range(0f, 100f)] private float _acceleration;
    [SerializeField, Range(0f, 100f)] private float _deceleration;
    [SerializeField, Range(0f, 100f)] private float _airAcceleration;
    [SerializeField, Range(0f, 100f)] private float _airDeceleration;
    
    private PlayerInputActions _playerInput;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    private Character _character;
    private CharacterStates _characterStates;

    private Vector2 _velocity; //TODO: Make it private after tests
    private Vector2 _desiredVelocity; // TODO: Make it private after tests
    private Vector2 _platformVelocity;
    
    private float _speedChange; //TODO: Make it private after tests
    private float previousDirectionX;
    private float _movementDirectionX; //TODO: Make it private after tests
    
    private bool _isOnPlatform;

    public float MovementDirectionX => _movementDirectionX;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _character = GetComponent<Character>();
        _characterStates = GetComponent<CharacterStates>();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        _movementDirectionX = _playerInput.Character.Move.ReadValue<Vector2>().x;

        if (IsDirectionChanged() && _character.IsTouchingWall == false)
        {
            FlipDirection();
            previousDirectionX = _movementDirectionX;
        }

        float friction = 0;
        _desiredVelocity = new Vector2(_movementDirectionX, 0f) * Mathf.Max(_maxMoveSpeed - friction, 0f);
    }

    private void FixedUpdate()
    {
        _velocity = _rigidbody.velocity;

        if (_character.CanMove)
        {
            Move();
        }
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

    private void Move()
    {
        bool isGrounded = _character.IsGrounded;
        
        float acceleration = isGrounded ? _acceleration : _airAcceleration;
        float deceleration = isGrounded ? _deceleration : _airDeceleration;
        float turnSpeed = isGrounded ? _turnSpeed : _airTurnSpeed;

        if (_movementDirectionX == 0) //TODO: Refactoring 
        {
            _speedChange = deceleration * Time.deltaTime;
        }
        else
        {
            if (Mathf.Sign(_movementDirectionX) != Mathf.Sign(_velocity.x))
            {
                _speedChange = turnSpeed * Time.deltaTime;
            }
            else
            {
                _speedChange = acceleration * Time.deltaTime;
            }
        }

        if (_isOnPlatform && _movementDirectionX == 0)
        {
            _desiredVelocity.x += _platformVelocity.x;
        }
        
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _speedChange);

        _rigidbody.velocity = _velocity;
    }

    private void FlipDirection()//TODO: Change it with facing and velocity?
    {
        _spriteRenderer.flipX = _movementDirectionX == (float)MoveDirectionX.Left;
    }

    private bool IsDirectionChanged()
    {
        return _movementDirectionX != previousDirectionX && _movementDirectionX != (float)MoveDirectionX.Idle;
    }

    private enum MoveDirectionX
    {
        Left = -1,
        Idle = 0,
        Right = 1
    }
}
