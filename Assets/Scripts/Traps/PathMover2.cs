using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PathMover2 : MonoBehaviour
{
    [SerializeField] private Transform _path;
    [SerializeField] private PathType _pathType;
    [SerializeField] private Transform _lookAt;
    [SerializeField] private Ease _ease;
    [SerializeField] private LoopType _loopType;
    [SerializeField] private float _duration;

    private Vector3[] _wayPoints;
    
    private void Start()
    {
        FillPoints();

        transform.position = _wayPoints[0];

        transform.DOPath(_wayPoints, 3, _pathType)
            .SetLoops(-1, _loopType)
            .SetEase(_ease);
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