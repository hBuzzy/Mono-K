using System;
using UnityEngine;
using UnityEngine.UIElements;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _waitAtPoint;
    [SerializeField] private Transform _path;
    
    private Vector3[] _wayPoints;
    private Vector3 target;
    private Vector3 _velocity;
    private Rigidbody2D _rigidbody;
    private Vector3 _moveDirection;

    public Vector3 Velocity => _rigidbody.velocity; 
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        FillPoints();
        target = _wayPoints[0]; 
        
        CalculateDirection();
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, _wayPoints[0]) < 0.05f)
        {
            target = _wayPoints[1];
            CalculateDirection();
        }
        if (Vector2.Distance(transform.position, _wayPoints[1]) < 0.05f)
        {
            target = _wayPoints[0];
            CalculateDirection();
        }

        //transform.position = Vector3.MoveTowards(transform.position, target, _moveSpeed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = _moveDirection * _moveSpeed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out Character character))
        {
            character.transform.SetParent(transform);
        }
    }
    
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.TryGetComponent(out Character character))
        {
            character.transform.SetParent(null);
        }
    }

    private void CalculateDirection()
    {
        _moveDirection = (target - transform.position).normalized;
    }
    
    private void FillPoints()
    {
        _wayPoints = new Vector3[_path.childCount];

        for (int i = 0; i < _path.childCount; i++)
        {
            _wayPoints[i] = _path.GetChild(i).position;
        }
    }
}
