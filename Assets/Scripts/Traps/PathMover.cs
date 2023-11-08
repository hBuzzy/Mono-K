using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PathMover : MonoBehaviour
{
    [Header("Path settings")]
    [SerializeField] private LoopType _loopType;
    [SerializeField] private float _waitBeforeStart;

    [Header("Moving between points")]
    [SerializeField] private float _duration;
    [SerializeField] private bool _isSpeedBased;
    [SerializeField] private float _waitAtPoint;
    [SerializeField] private Ease _ease;
    
    [Header("Path")]
    [SerializeField] private Transform[] _wayPoints;

    private int _currentPointIndex;
    private int _movingDirection = 1;
    
    public event Action Moved;
    
    private void OnValidate()
    {
        if (_loopType == LoopType.Incremental)
            _loopType = LoopType.Restart;
    }

    private void Start()
    {
        transform.position = _wayPoints[0].position;
        StartCoroutine(Move());
    }
    
    private void OnDrawGizmos()
    {
        int minWayPointsNumber = 2;
        
        if (_wayPoints == null || _wayPoints.Length < minWayPointsNumber)                          
            return; 
        
        for (int i = 0; i < _wayPoints.Length - 1; i++)                      
        {                                                               
            Gizmos.DrawLine(_wayPoints[i].position, _wayPoints[i + 1].position);  
        }                                                           
    }                                                                   
    
    private IEnumerator Move()
    {
        yield return new WaitForSeconds(_waitBeforeStart);

        while (enabled)
        {
            int nextPointIndex = _currentPointIndex + _movingDirection;
            
            yield return GetTween(GetNextPoint(nextPointIndex)).WaitForCompletion();
            
            yield return new WaitForSeconds(_waitAtPoint);
        }
    }

    private Vector3 GetNextPoint(int nextIndex)
    {
        if (_loopType == LoopType.Restart)
        {
            if (nextIndex >= _wayPoints.Length)
            {
                transform.position = _wayPoints[0].position;
                _currentPointIndex = 0;
            }
        }
        else if (_loopType == LoopType.Yoyo)
        {
            if (nextIndex >= _wayPoints.Length)
            {
                _movingDirection = -1;
            }
            else if (nextIndex < 0)
            {
                _movingDirection = 1;
            }
        }

        _currentPointIndex += _movingDirection;

        return _wayPoints[_currentPointIndex].position;
    }

    private Tween GetTween(Vector3 target)
    {
        return transform.DOMove(target, _duration)
            .SetEase(_ease)
            .SetSpeedBased(_isSpeedBased)
            .OnPlay(OnMove);
    }

    private void OnMove()
    {
        Moved?.Invoke();
    }
}
