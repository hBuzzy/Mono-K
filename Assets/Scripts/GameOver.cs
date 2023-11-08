using System;
using UnityEngine;

[RequireComponent(typeof(MainMenuButton))]

public class GameOver : MonoBehaviour
{
    [SerializeField] private Cutscene _endingCutscene;
    [SerializeField] private GameOverMenu _gameOverMenu;

    private MainMenuButton _mainMenuButton;
    
    public event Action<bool> GameOverChanged;

    private void Start()
    {
        _mainMenuButton = GetComponent<MainMenuButton>();
    }

    private void OnEnable()
    {
        _endingCutscene.Ended += OnCutsceneEnded;
        _gameOverMenu.Closed += OnGameOverMenuClosed;
    }

    private void OnDisable()
    {
        _endingCutscene.Ended -= OnCutsceneEnded;
        _gameOverMenu.Closed -= OnGameOverMenuClosed;
    }

    private void OnCutsceneEnded()
    {
        GameOverChanged?.Invoke(true);
        _gameOverMenu.gameObject.SetActive(true);
    }

    private void OnGameOverMenuClosed()
    {
        _mainMenuButton.LoadMainMenu();
    }
}