using System;
using UnityEngine;

public class Cutscenes : MonoBehaviour
{
    [SerializeField] private Cutscene[] _cutscenes;

    public event Action<bool> ActivationChanged; //TODO: Rennamed?

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
        ActivationChanged?.Invoke(true);
    }

    private void OnCutsceneEnded()
    {
        ActivationChanged?.Invoke(false);
    }
}