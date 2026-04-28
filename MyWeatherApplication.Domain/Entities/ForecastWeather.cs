namespace MyWeatherApplication.Domain.Entities;

public class ForecastWeather
{
    public string Location { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public List<DailyForecast> Forecast { get; set; } = new();

}