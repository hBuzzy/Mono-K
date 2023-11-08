public class Score
{
    private readonly string _time;
    private readonly string _deathsCount;
    private readonly string _cookiesCount;
    private readonly string _cookiesTotal;
    
    public Score(float time, int deathsCount, int cookiesCount, int cookiesTotal)
    {
        _time = DataConvector.TimeToString(time);
        _deathsCount = DataConvector.NumberToString(deathsCount);
        _cookiesCount = DataConvector.NumberToString(cookiesCount);
        _cookiesTotal = DataConvector.NumberToString(cookiesTotal);
    }

    public string Time => _time;
    public string DeathsCount => _deathsCount;
    public string CookiesCount => _cookiesCount;
    public string CookiesTotal => _cookiesTotal;
}