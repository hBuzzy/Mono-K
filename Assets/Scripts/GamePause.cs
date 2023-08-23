using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GamePause : MonoBehaviour
{
    [SerializeField] private PauseMenu _pauseMenu;

    private PlayerInputActions _playerInput;
    
    public static GamePause Instance { get; private set; } //TODO: Need to be on top?
    
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
        _playerInput.UI.OpenPauseMenu.performed += Pause;
        _pauseMenu.Closed += Resume;
    }

    private void OnDisable()
    {
        _playerInput.UI.Disable();
        _playerInput.UI.OpenPauseMenu.performed -= Pause;
        _pauseMenu.Closed -= Resume;
    }

    private void Pause(InputAction.CallbackContext obj)
    {
        _pauseMenu.gameObject.SetActive(true);
        
        Time.timeScale = 0f;
        PauseChanged?.Invoke(true);
    }

    private void Resume()
    {
        Time.timeScale = 1f;
        PauseChanged?.Invoke(false);
    }
}