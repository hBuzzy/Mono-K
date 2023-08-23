using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class PathMover : MonoBehaviour//TODO: NEED refactoring?
{
    [Header("Path settings")]
    [SerializeField] private bool _isLooped;
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
        if (_wayPoints == null || _wayPoints.Length < 2)                          
        {                                                               
            return;                                                     
        }                                                               
                                                                       
        for (int i = 0; i < _wayPoints.Length - 1; i++)                      
        {                                                               
            Gizmos.DrawLine(_wayPoints[i].position, _wayPoints[i + 1].position);  
        }                                                           
    }                                                                   
    
    private IEnumerator Move()
    {
        yield return new WaitForSeconds(_waitBeforeStart);
        
        int nextPointIndex = 0;
        
        while (enabled)
        {
            nextPointIndex = _currentPointIndex + _movingDirection;
            
            yield return GetTween(GetNextPoint(nextPointIndex)).WaitForCompletion();
            
            yield return new WaitForSeconds(_waitAtPoint);
        }
    }

    private Vector3 GetNextPoint(int nextIndex)//TODO: Refactoring or make strategy
    {
        if (_loopType == LoopType.Restart)
        {
            if (nextIndex >= _wayPoints.Length)
            {
                transform.position = _wayPoints[0].position;
                _currentPointIndex = 0;
            }
        }

        if (_loopType == LoopType.Yoyo)
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
            .OnPlay(InvokeMove);
    }

    private void InvokeMove()
    {
        Moved?.Invoke();
    }
}
