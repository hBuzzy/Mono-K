using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class CharacterMovement : MonoBehaviour
{
    [FormerlySerializedAs("_moveSpeed")]
    [Header("Movement")]
    [SerializeField, Range(0f, 20f)] private float _maxMoveSpeed;
    [SerializeField, Range(0f, 100f)] private float _acceleration;
    [SerializeField, Range(0f, 100f)] private float _deceleration;
    [SerializeField, Range(0f, 100f)] private float _turnSpeed;

    [Header("Jump")]
    [SerializeField] private float _jumpForce;

    [SerializeField, Range(0f, 20f)] private float _maxJumpHeight;
    [SerializeField, Range(0f, 100f)] private float _airAcceleration;
    [SerializeField, Range(0f, 100f)] private float _airDeceleration;
    [SerializeField, Range(0f, 100f)] private float _airTurnSpeed;
    
    [SerializeField] private LayerMask _groundMask;

    private PlayerInputActions _playerInput;
    private Rigidbody2D _rigidbody;

    private float _speedChange;
    private Vector2 _velocity;
    private Vector2 _desiredVelocity;

    private void Awake()
    {
        _playerInput = new PlayerInputActions();
        _playerInput.Character.Jump.performed += context => Jump();
    }

    private void Start()
    {
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
        Vector2 moveDirection = _playerInput.Character.Move.ReadValue<Vector2>();
        float friction = 0;
        _desiredVelocity = new Vector2(moveDirection.x, 0f) * Mathf.Max(_maxMoveSpeed - friction, 0f);
        
        Run(moveDirection);
    }
    
    private void FixedUpdate()
    {
        if (_rigidbody.velocity.y < 0)
        {
            _rigidbody.velocity += Vector2.up * (Physics2D.gravity.y * 0.7f * Time.deltaTime);
        }
    }

    private void Jump()
    {
        _rigidbody.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void Run(Vector2 direction)
    {
        //_rigidbody.velocity = new Vector2(direction.x * _moveSpeed, _rigidbody.velocity.y);
        //transform.position += new Vector3(direction.x, 0, direction.y) * (_moveSpeed * Time.deltaTime);

        float acceleration = IsGrounded() ? _acceleration : _airAcceleration;
        float deceleration = IsGrounded() ? _deceleration : _airDeceleration;
        float turnSpeed = IsGrounded() ? _turnSpeed : _airTurnSpeed;

        if (direction.x == 0) //TODO: Refactoring 
        {
            _speedChange = deceleration * Time.deltaTime;
        }
        else
        {
            if (Mathf.Sign(direction.x) != Mathf.Sign(_velocity.x))
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

    private bool IsGrounded()
    {
        return true;
    }
}
