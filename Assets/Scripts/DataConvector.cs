using System;

public static class DataConvector
{
    public static string NumberToString(float number)
    {
        return $"{number:#,#}";
    }

    public static string TimeToString(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        
        string hours = time.Hours == 0 ? "" : $"{time.Hours:00}:";
        
        return $"{hours}{time.Minutes:00}:{time.Seconds:00}";
    }
}