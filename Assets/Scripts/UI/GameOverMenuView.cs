using TMPro;
using UnityEngine;
using Convector = DataConvector;

public class GameOverMenuView : View<Score>
{
    [SerializeField] private GameOverMenu _gameOverMenu;

    [Header("Time score")]
    [SerializeField] private TMP_Text _timeValue;

    [Header("Deaths score")]
    [SerializeField] private TMP_Text _deathsValue;

    [Header("Cakes score")]
    [SerializeField] private TMP_Text _cakesValue;

    private void OnEnable()
    {
        _gameOverMenu.Opened += Render;
    }

    private void OnDisable()
    {
        _gameOverMenu.Opened -= Render;
    }

    protected override void Render(Score score)
    {
        _timeValue.text = score.Time;
        _deathsValue.text = score.DeathsCount;
        _cakesValue.text = $"{score.CookiesCount} / {score.CookiesTotal}";
    }
}