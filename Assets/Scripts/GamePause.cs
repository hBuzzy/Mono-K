using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePause : MonoBehaviour
{
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private GameOver _gameOver;

    private const float PauseTimeScale = 0f;
    private const float ResumeTimeScale = 1f;

    private PlayerInputActions _playerInput;
    private bool _isCutsceneActive;

    public static GamePause Instance { get; private set; }

    public event Action<bool> PauseChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            _playerInput = new PlayerInputActions();
        }
        else
        {
            Destroy(gameObject);
        }
        
        Resume();
    }

    private void OnEnable()
    {
        _playerInput.UI.Enable();
        _playerInput.UI.OpenPauseMenu.performed += OnPauseMenuPerformed;
        _pauseMenu.Closed += Resume;
        _gameOver.GameOverChanged += OnGameOverChanged;
    }

    private void OnDisable()
    {
        _playerInput.UI.Disable();
        _playerInput.UI.OpenPauseMenu.performed -= OnPauseMenuPerformed;
        _pauseMenu.Closed -= Resume;
        _gameOver.GameOverChanged += OnGameOverChanged;
    }

    private void Pause()
    {
        UpdatePauseState(PauseTimeScale);
    }

    private void Resume()
    {
        UpdatePauseState(ResumeTimeScale);
    }

    private void UpdatePauseState(float timeScale)
    {
        bool isPause = timeScale == PauseTimeScale;
        Time.timeScale = timeScale;
        AudioListener.pause = isPause;
        PauseChanged?.Invoke(isPause);
    }

    private void OnPauseMenuPerformed(InputAction.CallbackContext obj)
    {
        _pauseMenu.gameObject.SetActive(true);
        Pause();
    }

    private void OnGameOverChanged(bool isOver)
    {
        if (isOver)
            Pause();
        else
            Resume();
    }
}