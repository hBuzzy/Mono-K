using System;
using UnityEngine;

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

    public float _speedChange; //TODO: Make it private after tests
    public Vector2 _velocity; //TODO: Make it private after tests
    public Vector2 _desiredVelocity; // TODO: Make it private after tests
    public float previousDirectionX;
    public float directionX; //TODO: Make it private after tests
    private bool _isGrounded;
    private bool _isWallTouch;

    private bool _isOnPlatform;
    private Vector2 _platformVelocity;

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
        _character.GroundedChanged += isGrounded => { _isGrounded = isGrounded; };
        _character.WalledChanged += isWallTouch => { _isWallTouch = isWallTouch; };
    }

    private void OnDisable()
    {
        _playerInput.Character.Disable();
    }
    
    private void Update()
    {
        directionX = _playerInput.Character.Move.ReadValue<Vector2>().x;
        //directionX = Mathf.MoveTowards(input, directionX, 5f * Time.deltaTime);
        
        Vector2 direction = _playerInput.Character.Move.ReadValue<Vector2>();

        if (directionX != 0)
        {
            directionX = directionX > 0 ? 1 : -1;
        }
        else
        {
            directionX = 0;
        }
        
        if (IsDirectionChanged() && _isWallTouch == false)
        {
            FlipDirection();
            previousDirectionX = directionX;
        }

        float friction = 0;
        _desiredVelocity = new Vector2(directionX, 0f) * Mathf.Max(_maxMoveSpeed - friction, 0f);
    }

    private void FixedUpdate()
    {
        _velocity = _rigidbody.velocity;

        if (_characterStates.GetCurrentState() != CharacterStates.States.Dash)//TODO: Exchange to charater.CanMove???
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
        float acceleration = _isGrounded ? _acceleration : _airAcceleration;
        float deceleration = _isGrounded ? _deceleration : _airDeceleration;
        float turnSpeed = _isGrounded ? _turnSpeed : _airTurnSpeed;

        if (directionX == 0) //TODO: Refactoring 
        {
            _speedChange = deceleration * Time.deltaTime;
        }
        else
        {
            if (Mathf.Sign(directionX) != Mathf.Sign(_velocity.x))
            {
                _speedChange = turnSpeed * Time.deltaTime;
            }
            else
            {
                _speedChange = acceleration * Time.deltaTime;
            }
        }


        if (_isOnPlatform && directionX == 0)
        {
            _desiredVelocity.x += _platformVelocity.x;
        }
        
        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _speedChange);

        _rigidbody.velocity = _velocity;
    }

    private void FlipDirection()
    {
        _spriteRenderer.flipX = directionX == (float)MoveDirectionX.Left;
    }

    private bool IsDirectionChanged()
    {
        return directionX != previousDirectionX && directionX != (float)MoveDirectionX.Idle;
    }
    
    private enum MoveDirectionX
    {
        Left = -1,
        Idle = 0,
        Right = 1
    }
}
