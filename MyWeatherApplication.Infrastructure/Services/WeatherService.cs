using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using MyWeatherApplication.Domain.Entities;
using MyWeatherApplication.Domain.Interfaces;
using MyWeatherApplication.Infrastructure.Models;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
namespace MyWeatherApplication.Infrastructure.Services;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly WeatherApiSettings _setting;
    private readonly ILogger<WeatherService> _logger;
    public WeatherService(HttpClient httpClient, IOptions<WeatherApiSettings> setting, ILogger<WeatherService> logger)
    {
        _httpClient = httpClient;
        _setting = setting.Value;
        _logger = logger;

    }
    public async Task<CurrentWeather?> GetCurrentWeatherAsync(string location)
    {
        var stopwatch = Stopwatch.StartNew();
        var url = $"{_setting.BaseUrl}/current.json?key={_setting.ApiKey}&q={location}&aqi=no";
        _logger.LogTrace("Calling api.weatherapi.com/v1/current for {Location} ", location);
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode) //fail
        {
            _logger.LogWarning("WeatherAPI returned {StatusCode} for {Location} after {Elapsed} ms", response.StatusCode, location, stopwatch.ElapsedMilliseconds);
            response.EnsureSuccessStatusCode();
        }

        _logger.LogDebug("External API call succeeded in {Elapsed}ms", stopwatch.ElapsedMilliseconds);
        var jsonContent = await response.Content.ReadAsStringAsync();
        _logger.LogTrace("WeatherAPI response for {Location} : {Response}", location, jsonContent);
        var apiResponse = JsonSerializer.Deserialize<WeatherApiCurrentResponse>(jsonContent);
        if (apiResponse == null)
        {
            _logger.LogWarning("Failed to deserialize WeatherAPI response for {Location}", location);
            return null;
        }
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
        var stopwatch = Stopwatch.StartNew();
        var url = $"{_setting.BaseUrl}/forecast.json?key={_setting.ApiKey}&q={location}&days={days}&aqi=no";
        _logger.LogTrace("Calling WeatherAPI for forecast at {Location}, {Days} days", location, days);
        var response = await _httpClient.GetAsync(url);
        stopwatch.Stop();
        if (!response.IsSuccessStatusCode) //fail
        {
            _logger.LogWarning("WeatherAPI returned {StatusCode} for {Location} after {Elapsed} ms", response.StatusCode, location, stopwatch.ElapsedMilliseconds);
            response.EnsureSuccessStatusCode();
        }
        _logger.LogDebug("External API call succeeded in {Elapsed}ms", stopwatch.ElapsedMilliseconds);
        var jsonContent = await response.Content.ReadAsStringAsync();
        var apiResponse = JsonSerializer.Deserialize<WeatherApiForecastResponse>(jsonContent);
        if (apiResponse == null)
        {
            _logger.LogWarning("Failed to deserialize WeatherAPI response for {Location}", location);
            return null;
        }
        List<Forecastday> forecastfList = apiResponse.Forecastdata.Forecastdays;
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

