
using Microsoft.Extensions.Options;
using MyWeatherApplication.Infrastructure;
using MyWeatherApplication.Infrastructure.Services;
using System.Diagnostics;

public class ConfigureSetting
{
          public static IOptions<WeatherApiSettings> GetSettings()
        {
            var settingOption = Options.Create(new WeatherApiSettings
            {
                BaseUrl = "https://api.weatherapi.com/v1",
                ApiKey = "f4bf3a7c704f48dc985132921262404"
            });
            return settingOption;
        }
            public static IOptions<WeatherApiSettings> GetMockSettings()
        {
            var settingOption = Options.Create(new WeatherApiSettings
            {
                BaseUrl = "https://api.weatherapi.com/v1",
                ApiKey = "fake_api_key"
            });
            return settingOption;
        }
}