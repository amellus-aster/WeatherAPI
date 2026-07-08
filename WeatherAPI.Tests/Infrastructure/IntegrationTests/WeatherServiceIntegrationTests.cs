
using System.Text.Json;
using Microsoft.Extensions.Options;
using MyWeatherApplication.Domain.Interfaces;
using MyWeatherApplication.Infrastructure;
using MyWeatherApplication.Infrastructure.Services;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Matchers;
using Moq;
using Microsoft.Extensions.Logging;

namespace WeatherAPI.Tests.Infrastructure.IntegrationTests;

public class WeatherServiceIntegrationTests : IDisposable
{
    private readonly WireMockServer _server;
    private readonly IWeatherService _weatherService;
    private readonly HttpClient _httpClient;
    private readonly Mock<ILogger<WeatherService>> _loggerMock = new(); 
    public WeatherServiceIntegrationTests()
    {
        
        _server = WireMockServer.Start();
        var settings = Options.Create(
            new WeatherApiSettings()
            {
                BaseUrl = _server.Urls[0],
                ApiKey = "f4bf3a7c704f48dc985132921262404",
            }
        );
        _httpClient = new HttpClient();
        _weatherService = new WeatherService(_httpClient, settings, _loggerMock.Object);
    }
    public void Dispose()
    {
        _server?.Stop();
        _server?.Dispose();
        _httpClient?.Dispose();
    }
    [Fact]
    public async Task GetCurrentWeatherAsync_ValidResponse_ReturnEntity()
    {
        var location = "London";
        var jsonResponse = JsonFileReader.Read("current_weather_valid.json");
        _server
        .Given(Request.Create()
            .WithPath("/current.json")
            .WithParam("q", location)
            .WithParam("key", new WildcardMatcher("*"))
            .UsingGet())
        .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBody(jsonResponse));
        var result = await _weatherService.GetCurrentWeatherAsync(location);
        var requests = _server.FindLogEntries(Request.Create().WithPath("/current.json").UsingGet());
        Assert.Single(requests);
        await Verifier.Verify(result);
    }
    [Fact]
    public async Task GetForecastWeatherAsync_ValidResponse_ReturnEntity()
    {
        var location = "London";
        var days = "3";
        var jsonResponse = JsonFileReader.Read("forecast_weather_3days.json");
        _server
        .Given(Request.Create()
          .WithPath("/forecast.json")
          .WithParam("q", location)
          .WithParam("days", days)
          .WithParam("key", new WildcardMatcher("*"))
          .UsingGet())
        .RespondWith(Response.Create()
          .WithStatusCode(200)
          .WithHeader("Content-Type", "application/json")
          .WithBody(jsonResponse));
        var result = await _weatherService.GetForecastWeatherAsync(location, int.Parse(days));
        var requests = _server.FindLogEntries(Request.Create().WithPath("/forecast.json").UsingGet());
        Assert.Single(requests);
        await Verifier.Verify(result);
    }
    [Fact]
    public async Task GetCurrentWeatherAsync_LocationNotFound_ThrowsHttpRequestException()
    {
        var location = "invisible city";
        _server
      .Given(Request.Create()
        .WithPath("/current.json")
        .WithParam("q", location)
        .WithParam("key", new WildcardMatcher("*"))
        .UsingGet())
      .RespondWith(Response.Create()
        .WithStatusCode(404)
        .WithHeader("Content-Type", "application/json"));
        await Assert.ThrowsAsync<HttpRequestException>(() => _weatherService.GetCurrentWeatherAsync(location));

    }
    [Fact]
    public async Task GetCurrentWeatherAsync_InvalidApiKey_ThrowsHttpRequestException()
    {
        var location = "London";
        var invalidKey = "Key_sai_bet";
        var settings = Options.Create(
            new WeatherApiSettings()
            {
                BaseUrl = _server.Urls[0],
                ApiKey = invalidKey,
            }
        );
        var httpClient = new HttpClient();
        var _newWeatherService = new WeatherService(httpClient, settings, _loggerMock.Object);

        _server
      .Given(Request.Create()
        .WithPath("/current.json")
        .WithParam("q", location)
        .WithParam("key", invalidKey)
        .UsingGet())
      .RespondWith(Response.Create()
        .WithStatusCode(401)
        .WithHeader("Content-Type", "application/json"));
        await Assert.ThrowsAsync<HttpRequestException>(() => _newWeatherService.GetCurrentWeatherAsync(location));
    }
    [Fact]
    public async Task GetCurrentWeatherAsync_ApiReturnsMalformedJson_ThrowsJsonException()
    {
        var location = "London";
        var jsonResponse = "688{44994yyyhhhhyhyyy}"; //MalformedJson
        _server
        .Given(Request.Create()
            .WithPath("/current.json")
            .WithParam("q", location)
            .WithParam("key", new WildcardMatcher("*"))
            .UsingGet())
        .RespondWith(Response.Create()
            .WithStatusCode(200)
            .WithHeader("Content-Type", "application/json")
            .WithBody(jsonResponse));
        await Assert.ThrowsAsync<JsonException>(() => _weatherService.GetCurrentWeatherAsync(location));

    }
}

