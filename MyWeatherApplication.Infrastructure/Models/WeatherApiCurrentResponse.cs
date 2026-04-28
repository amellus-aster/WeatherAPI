using System.Text.Json.Serialization;
namespace MyWeatherApplication.Infrastructure.Models;

public class WeatherApiCurrentResponse
{
    [JsonPropertyName("location")]
    public LocationData Location { get; set; } = new();
    [JsonPropertyName("current")]
    public CurrentData Current { get; set; } = new();
}
