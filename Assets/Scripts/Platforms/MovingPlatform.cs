using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _waitAtPoint;

    [Header("Path")]
    [SerializeField] private Transform[] _wayPoints;

    private const int LeftDirection = -1;
    private const int RightDirection = 1;
    private const float DistanceError = 0.2f;

    private Vector3 _targetPosition;
    private Vector2 _velocity;
    private Vector3 _moveDirection;
    private Rigidbody2D _rigidbody;
    private Coroutine _waitCoroutine;

    private int _indexDirection;
    private int _wayPointIndex;
    private bool _isOnPlatform;

    public Vector3 Velocity => _rigidbody.velocity;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        _indexDirection = RightDirection;
        
        _wayPointIndex = 1;
        _targetPosition = _wayPoints[_wayPointIndex].position;

        CalculateMoveDirection();
    }

    private void Update()
    {
        if (_waitCoroutine != null || IsTargetPointReached() == false)
            return;

        transform.position = _targetPosition;
        _rigidbody.velocity = Vector2.zero;
        
        SelectNextWayPoint();
        
        if (_waitCoroutine != null)
            StopCoroutine(_waitCoroutine);

        _waitCoroutine = StartCoroutine(Wait());
    }

    private void FixedUpdate()
    {
        if (_waitCoroutine != null)
            return;

        _rigidbody.velocity = (_moveDirection) * _moveSpeed;
    }
    
    private void OnDrawGizmos()
    {                                                                   
        if (_wayPoints == null || _wayPoints.Length < 2)                          
            return;                                                               
                                                                       
        for (int i = 0; i < _wayPoints.Length - 1; i++)                      
        {                                                               
            Gizmos.DrawLine(_wayPoints[i].position, _wayPoints[i + 1].position);  
        }                                                           
    }

    private bool IsTargetPointReached()
    {
        return Vector2.Distance(transform.position, _targetPosition) < DistanceError;
    }

    private void SelectNextWayPoint()
    {
        if (_wayPointIndex == _wayPoints.Length - 1)
            _indexDirection = LeftDirection;

        if (_wayPointIndex == 0)
            _indexDirection = RightDirection;

        _wayPointIndex += _indexDirection;
        _targetPosition = _wayPoints[_wayPointIndex].transform.position;
        
        CalculateMoveDirection();
    }

    private void CalculateMoveDirection()
    {
        _moveDirection = (_targetPosition - transform.position).normalized;
    }

    private IEnumerator Wait()
    {
        yield return new WaitForSeconds(_waitAtPoint);
        
        _waitCoroutine = null;
    }
}
