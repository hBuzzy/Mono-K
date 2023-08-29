using TMPro;
using UnityEngine;

public class GameTimerView : View<float>
{
    [SerializeField] private TMP_Text _timeText;
    [SerializeField] private GameTimer _gameTimer;

    private void OnEnable()
    {
        _gameTimer.TimeChanged += Render;
    }

    private void OnDisable()
    {
        _gameTimer.TimeChanged -= Render;
    }

    protected override void Render(float seconds)
    {
        _timeText.text = DataConvector.TimeToString(seconds);
    }
}