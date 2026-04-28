using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyWeatherApplication.Infrastructure.Models
{
    public class ForecastData
    {
        [JsonPropertyName("forecastday")]
        public List<Forecastday> Forecastdays { get; set; } = new();
    }

    public class Forecastday
    {
        [JsonPropertyName("date")]
        public string Date { get; set; } = string.Empty;

        [JsonPropertyName("date_epoch")]
        public int DateEpoch { get; set; }

        [JsonPropertyName("day")]
        public Day Day { get; set; } = new();

        [JsonPropertyName("astro")]
        public Astro Astro { get; set; } = new();

        [JsonPropertyName("hour")]
        public List<Hour> Hour { get; set; } = new();
    }
}