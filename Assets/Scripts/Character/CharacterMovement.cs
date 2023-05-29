using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;
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
    private Grounder _grounder;
    private Rigidbody2D _rigidbody;

    private float _speedChange;
    private Vector2 _velocity;
    private Vector2 _desiredVelocity;
    private float directionX;
    private bool isGrounded;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
    }

    private void Start()
    {
        _grounder = GetComponent<Grounder>();
        _rigidbody = GetComponent<Rigidbody2D>();
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
        directionX = _playerInput.Character.Move.ReadValue<Vector2>().x;
        float friction = 0;
        _desiredVelocity = new Vector2(directionX, 0f) * Mathf.Max(_maxMoveSpeed - friction, 0f);
    }
    
    private void FixedUpdate()
    {
        isGrounded = _grounder.IsGrounded;
        _velocity = _rigidbody.velocity;

        Move();
    }

    private void Move()
    {
        //_rigidbody.velocity = new Vector2(direction.x * _moveSpeed, _rigidbody.velocity.y);
        //transform.position += new Vector3(direction.x, 0, direction.y) * (_moveSpeed * Time.deltaTime);

        float acceleration = isGrounded ? _acceleration : _airAcceleration;
        float deceleration = isGrounded ? _deceleration : _airDeceleration;
        float turnSpeed = isGrounded ? _turnSpeed : _airTurnSpeed;

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

        _velocity.x = Mathf.MoveTowards(_velocity.x, _desiredVelocity.x, _speedChange);
        _rigidbody.velocity = _velocity;
    }
}
