using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{[Header("Movement")]
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _deceleration;

    [Header("Jump")]
    [SerializeField] private float _jumpForce;

    [SerializeField] private LayerMask _groundMask;

    private PlayerInputActions _playerInput;
    private Rigidbody2D _rigidbody;

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
        Move(moveDirection);
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

    private void Move(Vector2 direction)
    {
        _rigidbody.velocity = new Vector2(direction.x * _moveSpeed, _rigidbody.velocity.y);
        //transform.position += new Vector3(direction.x, 0, direction.y) * (_moveSpeed * Time.deltaTime);
    }

    private bool IsGrounded()
    {
        return false;
    }
}
