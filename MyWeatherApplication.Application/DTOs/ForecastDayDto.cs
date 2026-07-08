namespace MyWeatherApplication.Application.DTOs;
public class ForecastDayDto
{
    public string Date { get; set; } = string.Empty;
    public string Sunrise { get; set; } = string.Empty;
    public string Sunset { get; set; } = string.Empty;
    public string MoonPhase { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public double MaxTempC { get; set; }
    public double MinTempC { get; set; }
    public double AvgTempC { get; set; }
}

