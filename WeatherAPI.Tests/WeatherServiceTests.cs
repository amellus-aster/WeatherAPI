

using Microsoft.Extensions.Options;
using MyWeatherApplication.Infrastructure;
using MyWeatherApplication.Infrastructure.Services;
using System.Diagnostics;

namespace WeatherAPI.Tests
{
    public class WeatherServiceTests
    {
        [Fact]
        public async Task GetCurrentWeatherAsync_ShouldGetRealDataFromApi()
        {
            var httpClient = new HttpClient();

            var service = new WeatherService(httpClient, GetSettings());
            var result = await service.GetCurrentWeatherAsync("London");
            //Assert
            Assert.NotNull(result);
            Assert.Equal("London", result.Location);
            Assert.Equal("United Kingdom", result.Country);
            Assert.True(result.TemperatureC > -10 && result.TemperatureC < 30);

        }
        [Theory]
        [InlineData("Ha noi")]
        [InlineData("Ho Chi Minh City")]
        [InlineData("Da Nang")]
        [InlineData("New York")]
        [InlineData("London")]
        public async Task GetCurrentWeather_MultipleCities_ShouldWork(string city)
        {
            var service = new WeatherService(new HttpClient(), GetSettings());
            var result = await service.GetCurrentWeatherAsync(city);

            Assert.NotNull(result);
            Console.WriteLine($"{city}: {result.TemperatureC}°C - {result.Condition}");
        }
        private static IOptions<WeatherApiSettings> GetSettings()
        {
            var settingOption = Options.Create(new WeatherApiSettings
            {
                BaseUrl = "https://api.weatherapi.com/v1",
                ApiKey = "f4bf3a7c704f48dc985132921262404"
            });
            return settingOption;
        }
    }
}
