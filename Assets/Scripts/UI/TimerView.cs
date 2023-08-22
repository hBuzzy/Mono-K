using TMPro;
using UnityEngine;

public class TimerView : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;

    private const int SecondsInMinute = 60;

    public void Render(float time)
    {
        int seconds = Mathf.FloorToInt(time % SecondsInMinute);
        int minutes = Mathf.FloorToInt(time / SecondsInMinute);
        
        _timeText.text = $"{minutes:00}:{seconds:00}";
    }
}