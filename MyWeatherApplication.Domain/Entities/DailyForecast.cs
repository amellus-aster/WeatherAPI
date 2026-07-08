namespace MyWeatherApplication.Domain.Entities;

public class DailyForecast
{
    public string Date { get; set; } = string.Empty;
    public string Sunrise { get; set; } = string.Empty;
    public string Sunset { get; set; } = string.Empty;
    public string MoonPhase { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public double MaxTempC { get; set; }
    public double MinTempC { get; set; }
    public double AvgTempC { get; set; }
    public double MaxWindKph { get; set; }
    public double AvgHumidity { get; set; }
}

