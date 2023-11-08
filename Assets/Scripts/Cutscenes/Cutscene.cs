using System;
using Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

[RequireComponent(typeof(PlayableDirector))]

public class Cutscene : MonoBehaviour
{
    [SerializeField] private CutsceneTrigger _startTrigger;
    [SerializeField] private CinemachineVirtualCamera _camera;
    private PlayableDirector _cutscene;
    private float _elapsedTime;
    private bool _isActive;
    
    public event Action Started;
    public event Action Ended;

    private void Awake()
    {
        _cutscene = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        _startTrigger.Triggered += OnTriggered;
    }

    private void OnDisable()
    {
        _startTrigger.Triggered -= OnTriggered;
    }

    private void Update()
    {
        if (_isActive == false)
            return;
        
        _elapsedTime += Time.deltaTime;

        if (_elapsedTime >= _cutscene.duration)
        {
            _isActive = false;
            CameraController.Instance.SetDefaultCamera();
            Ended?.Invoke();
        }
    }

    private void OnTriggered()
    {
        _isActive = true;
        _elapsedTime = 0f;
        _cutscene.Play();
        CameraController.Instance.SetCamera(_camera);
        Started?.Invoke();
    }
} 