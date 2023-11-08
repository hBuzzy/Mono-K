using System;

public static class DataConvector
{
    public static string NumberToString(float number)
    {
        string zeroNumberFormat = "0";

        string formattedResult = $"{number:#,##}";

        return string.IsNullOrEmpty(formattedResult) ? zeroNumberFormat : formattedResult;
    }

    public static string TimeToString(float seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        
        string hours = time.Hours == 0f ? string.Empty : $"{time.Hours:00}:";
        
        return $"{hours}{time.Minutes:00}:{time.Seconds:00}";
    }
}