namespace MyWeatherApplication.Infrastructure.Models;

using System.Text.Json.Serialization;
public class Condition
{
    [JsonPropertyName("text")]
    public string Text { get; set; } = string.Empty;

    [JsonPropertyName("icon")]
    public string Icon { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public double Code { get; set; }
}