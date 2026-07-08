using MyWeatherApplication.Application.DTOs;

namespace MyWeatherApplication.Application.DTOs
{
    // DTO (Data Transfer Object) that represents a weather forecast result.
    // Typically contains multiple daily forecast records returned to the client.
    public class ForecastWeatherDto
    {
        public string Location { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public List<ForecastDayDto> Forecast { get; set; } = new();
    }
}