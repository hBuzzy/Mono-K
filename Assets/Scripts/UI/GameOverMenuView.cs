using TMPro;
using UnityEngine;

public class GameOverMenuView : View<Score[]>
{
    [SerializeField] private GameOverMenu _gameOverMenu;

    [Header("Time score")]
    [SerializeField] private TMP_Text _timeValue;
    [SerializeField] private TMP_Text _timeScore;

    [Header("Deaths score")]
    [SerializeField] private TMP_Text _deathsValue;
    [SerializeField] private TMP_Text _deathsSScore;

    [Header("Cakes score")]
    [SerializeField] private TMP_Text _cakesValue;
    [SerializeField] private TMP_Text _cakesScore;

    [Header("Total score")]
    [SerializeField] private TMP_Text _totalScore;

    private Score _cakesScore1;
    private Score _timeScore1;
    private Score _deathsScore1;
    
    private void OnEnable()
    {
        _gameOverMenu.Opened += Render1;
    }

    private void OnDisable()
    {
        _gameOverMenu.Opened -= Render1;
    }

    protected override void Render(Score[] scores)
    {
        throw new System.NotImplementedException();
    }

    private void Render1()
    {
        RenderTimeScore();
        RenderDeathsScore();
        RenderCakesScore();
        RenderTotalScore();
    }

    private void RenderTimeScore()
    {
        _timeScore1 = _gameOverMenu.GetTImeScore();
        _timeValue.text = DataConvector.TimeToString(_timeScore1.ParameterValue);
        _timeScore.text = DataConvector.NumberToString(_timeScore1.ScorePoints);
    }

    private void RenderDeathsScore()
    {
        _deathsScore1 = _gameOverMenu.GetDeathsScore();
        _deathsValue.text = DataConvector.TimeToString(_deathsScore1.ParameterValue);
        _deathsSScore.text = DataConvector.NumberToString(_deathsScore1.ScorePoints);
    }

    private void RenderCakesScore()
    {
       _cakesScore1 = _gameOverMenu.GetCakesScore();
        _cakesValue.text = DataConvector.TimeToString(_cakesScore1.ParameterValue);
        _cakesScore.text = DataConvector.NumberToString(_cakesScore1.ScorePoints);
    }

    private void RenderTotalScore()
    {
        int totalScore = 50000 + _timeScore1.ScorePoints + _deathsScore1.ScorePoints + _cakesScore1.ScorePoints;
        
        _totalScore.text = DataConvector.NumberToString(totalScore);
    }

}