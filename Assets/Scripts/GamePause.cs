using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePause : MonoBehaviour
{
    [SerializeField] private PauseMenu _pausePauseMenu;
    [SerializeField] private GameOver _gameOver;

    private PlayerInputActions _playerInput;
    
    public static GamePause Instance { get; private set; }
    
    public event Action<bool> PauseChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _playerInput = new PlayerInputActions();
    }

    private void OnEnable()
    {
        _playerInput.UI.Enable();
        _playerInput.UI.OpenPauseMenu.performed += OnPauseMenuOpened;
        _pausePauseMenu.Closed += Resume;
        _gameOver.GameOverChanged += OnGameOverChanged;
    }

    private void OnDisable()
    {
        _playerInput.UI.Disable();
        _playerInput.UI.OpenPauseMenu.performed -= OnPauseMenuOpened;
        _pausePauseMenu.Closed -= Resume;
        _gameOver.GameOverChanged += OnGameOverChanged;
    }

    private void Pause()
    {
        Time.timeScale = 0f;
        PauseChanged?.Invoke(true);
    }

    private void Resume()
    {
        Time.timeScale = 1f;
        PauseChanged?.Invoke(false);
    }

    private void OnPauseMenuOpened(InputAction.CallbackContext obj)
    {
        _pausePauseMenu.gameObject.SetActive(true);//todo: ADD BOOLEAN for check if in cutscene
        
        Pause();
    }

    private void OnGameOverChanged(bool isOver)
    {
        if (isOver)
        {
            Pause();
        }
        else
        {
            Resume();
        }
    }
}