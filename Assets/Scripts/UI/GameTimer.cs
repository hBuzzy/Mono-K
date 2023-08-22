using System;
using UnityEngine;

[RequireComponent(typeof(TimerView))]

public class GameTimer : MonoBehaviour //TODO: refactor connection betwen model and view
{
    private float _elapsedTime;
    private bool _isActive;

    private TimerView _view;

    private void Start()
    {
        _view = GetComponent<TimerView>();
    }

    private void OnEnable()
    {
        //TODO: Subscribe to night started
        //TODO: Subscribe to game (reached end pouint) ended
    }

    private void OnDisable()
    {
        //throw new NotImplementedException();
    }

    private void Update()
    {
        _elapsedTime += Time.deltaTime;
        _view.Render(_elapsedTime);
    }

    private void OnStart()
    {
        
    }

    private void OnPause()
    {
        
    }

    private void OnStop()
    {
        
    }
}