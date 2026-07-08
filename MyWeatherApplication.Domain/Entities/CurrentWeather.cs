namespace MyWeatherApplication.Domain.Entities;

public class CurrentWeather
{
    public string Location { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double TemperatureC { get; set; }
    public string Condition { get; set; } = string.Empty;
    public double WindSpeed { get; set; }
    public double Humidity { get; set; }
    public double FeelslikeC { get; set; }
    public double UvIndex { get; set; }
    public double Cloud { get; set; }
}
