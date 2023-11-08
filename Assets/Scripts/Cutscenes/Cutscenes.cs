using System;
using UnityEngine;

public class Cutscenes : MonoBehaviour
{
    [SerializeField] private Cutscene[] _cutscenes;
    public event Action<bool> ActiveChanged;

    private void OnEnable()
    {
        foreach (Cutscene cutscene in _cutscenes)
        {
            cutscene.Started += OnCutsceneStarted;
            cutscene.Ended += OnCutsceneEnded;
        }
    }

    private void OnDisable()
    {
        foreach (Cutscene cutscene in _cutscenes)
        {
            cutscene.Started -= OnCutsceneStarted;
            cutscene.Ended -= OnCutsceneEnded;
        }
    }

    private void OnCutsceneStarted()
    {
        ActiveChanged?.Invoke(true);
    }

    private void OnCutsceneEnded()
    {
        ActiveChanged?.Invoke(false);
    }
}