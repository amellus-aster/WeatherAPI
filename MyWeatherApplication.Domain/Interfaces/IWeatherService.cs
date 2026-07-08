using MyWeatherApplication.Domain.Entities;

namespace MyWeatherApplication.Domain.Interfaces;
public interface IWeatherService
{
    Task<CurrentWeather?> GetCurrentWeatherAsync(string location); 
    Task<ForecastWeather?> GetForecastWeatherAsync(string location, int days = 3); 
}
