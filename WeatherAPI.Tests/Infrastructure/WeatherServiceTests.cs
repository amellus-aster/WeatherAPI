
using VerifyXunit;
using System.Text.Json;
using MyWeatherApplication.Infrastructure.Services;
using RichardSzalay.MockHttp;
using System.Net;
using Microsoft.Extensions.Logging;
using Moq;

namespace WeatherAPI.Tests.Infrastructure;


public class WeatherServiceTests
{
    private readonly MockHttpMessageHandler mockHttp = new();
    private readonly Mock<ILogger<WeatherService>> _loggerMock = new(); 
    [Fact]
    public async Task GetCurrentWeatherAsync_ValidResponse_ReturnEntity()
    {
        var jsonResponse = JsonFileReader.Read("current_weather_valid.json");
        mockHttp.When("https://api.weatherapi.com/v1/current.json*").Respond("application/json", jsonResponse);
        var mockHttpClient = mockHttp.ToHttpClient();
        var setting = ConfigureSetting.GetMockSettings();
        var weatherService = new WeatherService(mockHttpClient, setting, _loggerMock.Object);
        //Act
        var result = await weatherService.GetCurrentWeatherAsync("London");
        //Assert
        await Verifier.Verify(result);
    }
    [Fact]
    public async Task GetCurrentWeatherAsync_ApiError_ReturnException()
    {
        mockHttp.When("https://api.weatherapi.com/v1/current.json*").Respond(HttpStatusCode.BadRequest);
        var mockHttpClient = mockHttp.ToHttpClient();
        var setting = ConfigureSetting.GetMockSettings();
        var weatherService = new WeatherService(mockHttpClient, setting, _loggerMock.Object);
        //Act
        // var result = await weatherService.GetCurrentWeatherAsync("InvalidCity");
        //Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => weatherService.GetCurrentWeatherAsync("InvalidCity"));
    }
    [Fact]
    public async Task GetForecastWeatherAsync_ValidResponse_ReturnEntity()
    {
        var jsonResponse = JsonFileReader.Read("forecast_weather_3days.json"); 
        mockHttp.When("https://api.weatherapi.com/v1/forecast.json*").Respond("application/json", jsonResponse);
        var mockHttpClient = mockHttp.ToHttpClient();
        var setting = ConfigureSetting.GetMockSettings();
        var weatherService = new WeatherService(mockHttpClient, setting, _loggerMock.Object);
        //Act
        var result = await weatherService.GetForecastWeatherAsync("London", 3);
        //Assert
        await Verifier.Verify(result);
    }
    [Fact]
    public async Task GetForecastWeatherAsync_ApiError_ReturnException()
    {
        mockHttp.When("https://api.weatherapi.com/v1/forecast.json*").Respond(HttpStatusCode.BadRequest);
        var mockHttpClient = mockHttp.ToHttpClient();
        var setting = ConfigureSetting.GetMockSettings();
        var weatherService = new WeatherService(mockHttpClient, setting, _loggerMock.Object);
        //Act
        // var result = await weatherService.GetCurrentWeatherAsync("InvalidCity");
        //Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => weatherService.GetForecastWeatherAsync("InvalidCity", 3));
    }
}