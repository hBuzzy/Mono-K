using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PathMover : MonoBehaviour
{
    [SerializeField] private Transform _path;
    [SerializeField, Range(0f, 3f)] private float _waitBeforeStart;
    
    [Header("Sequence settings")]
    [SerializeField] private LoopType _loopType;
    [SerializeField] private int _loopsNumber;
    [SerializeField] private bool _isLooped;
    [SerializeField] private float _delay;

    [Header("Move")]
    [SerializeField] private float _duration;
    [SerializeField] private bool _isSpeedBased;
    [SerializeField] private float _waitAtPoint;
    [SerializeField] private Ease _ease;
    
    private List<Vector3> _wayPoints;

    public event Action Moving;
    public event Action WaitingAtPoint;

    private void Start()
    {
        _loopsNumber = _isLooped ? -1 : _loopsNumber;

        FillPoints();

        if (_wayPoints.Count > 0)
        {
            transform.position = _wayPoints[0];
            StartCoroutine(PlaySequence());
        }
        else
        {
            throw new InvalidDataException();
        }
    }

    private IEnumerator PlaySequence()
    {
        int index = 0;
        int loopNumber = 0;

        yield return new WaitForSeconds(Random.Range(0, 2));
        
        while (enabled)
        {
            if (index >= _wayPoints.Count)
            {
                yield return new WaitForSeconds(_delay);
                
                index = 0;
                
                if (_loopType == LoopType.Yoyo)
                {
                    _wayPoints.Reverse();
                    index = 1;
                }
            }

            
            yield return GetTween(_wayPoints[index]).WaitForCompletion();

            if (_waitAtPoint > 0)
            {
                WaitingAtPoint?.Invoke();
                yield return new WaitForSeconds(_waitAtPoint);
            }

            index++;
        }
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
        Moving?.Invoke();
    }
    
    private void FillPoints()
    {
        _wayPoints = new List<Vector3>();

        for (int i = 0; i < _path.childCount; i++)
        {
            _wayPoints.Add(_path.GetChild(i).position);
        }
    }
}
