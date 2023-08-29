public class Score
{
    private float _parameterValue;
    private int _scorePoints;

    public float ParameterValue => _parameterValue;
    public int ScorePoints => _scorePoints;
    
    public Score(float parameterValue, int scorePoints)
    {
        _parameterValue = parameterValue;
        _scorePoints = scorePoints;
    }    
}