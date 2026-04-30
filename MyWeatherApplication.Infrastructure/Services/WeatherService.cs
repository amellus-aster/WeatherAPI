using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using MyWeatherApplication.Domain.Entities;
using MyWeatherApplication.Domain.Interfaces;
using MyWeatherApplication.Infrastructure.Models;
namespace MyWeatherApplication.Infrastructure.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiSettings _setting;
    public WeatherService(HttpClient httpClient, IOptions<WeatherApiSettings> setting)
    {
        _httpClient = httpClient;
        _setting = setting.Value;
    }
    public async Task<CurrentWeather?> GetCurrentWeatherAsync(string location)
    {
        var url = $"{_setting.BaseUrl}/current.json?key={_setting.ApiKey}&q={location}&aqi=no";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var jsonContent = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<WeatherApiCurrentResponse>(jsonContent);
        if (apiResponse == null) return null;
        return new CurrentWeather
        {
            Location = apiResponse.Location.Name,
            Country = apiResponse.Location.Country,
            TemperatureC = apiResponse.Current.TempC,
            Condition = apiResponse.Current.Condition!.Text,
            WindSpeed = apiResponse.Current.WindMph,
            Humidity = apiResponse.Current.Humidity,
            FeelslikeC = apiResponse.Current.FeelslikeC,
            UvIndex = apiResponse.Current.Uv,
            Cloud = apiResponse.Current.Cloud,
        };
    }
    public async Task<ForecastWeather?> GetForecastWeatherAsync(string location, int days)
    {
        var url = $"{_setting.BaseUrl}/forecast.json?key={_setting.ApiKey}&q={location}&days={days}&aqi=no";
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var jsonContent = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<WeatherApiForecastResponse>(jsonContent);
        if (apiResponse == null) return null;
        List<Forecastday>  forecastfList = apiResponse.Forecastdata.Forecastdays;
        return new ForecastWeather
        {
            Location = apiResponse.Location.Name, 
            Country = apiResponse.Location.Country,
            Forecast = apiResponse.Forecastdata.Forecastdays.Select(f => new DailyForecast
            {
                Date = f.Date, 
                Sunrise = f.Astro.Sunrise,
                Sunset = f.Astro.Sunset,
                MoonPhase = f.Astro.MoonPhase,
                Condition = apiResponse.Current.Condition!.Text,
                MaxTempC = f.Day.MaxTempC,
                MinTempC = f.Day.MinTempC,
                AvgTempC = f.Day.AvgTempC,
                MaxWindKph = f.Day.MaxWindKph,
                AvgHumidity = f.Day.AvgHumidity
            }).ToList()
        }; 

    }
}

