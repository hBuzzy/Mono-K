using System;
using TMPro;
using UnityEngine;

public class TimeView : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;

    public void Render(float time)
    {
        TimeSpan timeSpan = TimeSpan.FromSeconds(time);
        
        string hours = timeSpan.Hours == 0 ? "" : $"{timeSpan.Hours}:";
        
        _timeText.text = $"{hours}{timeSpan.Minutes:00}:{timeSpan.Seconds:00}";
    }
}