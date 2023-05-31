using System;
using UnityEngine;

public class Times : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] private float _timeScale = 1f;

    private void OnEnable()
    {
        Time.timeScale = 1f;
    }

    private void OnDisable()
    {
        Time.timeScale = _timeScale;
    }
}
