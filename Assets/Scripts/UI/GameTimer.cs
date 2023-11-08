using System;
using UnityEngine;

[RequireComponent(typeof(GameTimerView))]

public class GameTimer : MonoBehaviour
{
    private bool _isActive;
    
    private float _elapsedTime;
    private int _currentSeconds;

    public float ElapsedTime => _elapsedTime;
    
    public event Action<float> TimeChanged;

    private void OnEnable()
    {
        _elapsedTime = 0;
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        
        int passedSeconds = (int)_elapsedTime;

        if (passedSeconds == _currentSeconds)
            return;

        _currentSeconds = passedSeconds;
        TimeChanged?.Invoke(_elapsedTime);
    }
}