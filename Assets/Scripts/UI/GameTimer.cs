using System;
using UnityEngine;

[RequireComponent(typeof(GameTimerView))]

public class GameTimer : MonoBehaviour
{
    private bool _isActive;
    
    private float _elapsedTIme;
    private int _currentSeconds;

    public float ElapsedTIme => _elapsedTIme;
    
    public event Action<float> TimeChanged;

    private void OnEnable()
    {
        _elapsedTIme = 0;
    }

    private void Update()
    {
        _elapsedTIme += Time.deltaTime;
        
        int passedSeconds = (int)_elapsedTIme;

        if (passedSeconds == _currentSeconds)
            return;

        _currentSeconds = passedSeconds;
        TimeChanged?.Invoke(_elapsedTIme);
    }
}