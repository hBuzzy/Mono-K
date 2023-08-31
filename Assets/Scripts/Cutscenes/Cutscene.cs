using System;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    [SerializeField] private CutsceneTrigger _startTrigger;

    public event Action Started;
    public event Action Ended;

    private void OnEnable()
    {
        _startTrigger.Triggered += OnTriggered;
    }

    private void OnDisable()
    {
        _startTrigger.Triggered -= OnTriggered;
    }

    public void OnEnded()
    {
        Ended?.Invoke();
    }

    private void OnTriggered()
    {
        Started?.Invoke();
    }
} 