using System.Text.Json.Serialization;

namespace MyWeatherApplication.Infrastructure.Models
{
    public class Astro
    {
        [JsonPropertyName("sunrise")]
        public string Sunrise { get; set; } = string.Empty;

        [JsonPropertyName("sunset")]
        public string Sunset { get; set; } = string.Empty;

        [JsonPropertyName("moonrise")]
        public string Moonrise { get; set; } = string.Empty;

        [JsonPropertyName("moonset")]
        public string Moonset { get; set; } = string.Empty;

        [JsonPropertyName("moon_phase")]
        public string MoonPhase { get; set; } = string.Empty;

        [JsonPropertyName("moon_illumination")]
        public int MoonIllumination { get; set; }

        [JsonPropertyName("is_moon_up")]
        public int IsMoonUp { get; set; }

        [JsonPropertyName("is_sun_up")]
        public int IsSunUp { get; set; }
    }
}