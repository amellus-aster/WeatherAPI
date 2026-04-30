using Xunit;
using VerifyXunit;
using Moq;
using MyWeatherApplication.Application.Handlers;
using MyWeatherApplication.Application.Queries;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Domain.Entities;
using MyWeatherApplication.Domain.Common;
using MyWeatherApplication.Domain.Interfaces;

namespace WeatherAPI.Tests.Application.Handlers;

public class GetForecastWeatherHandlerTests
{
    private readonly Mock<IWeatherService> weatherServiceMock = new();
    [Fact]
    public async Task Handle_ValidLocation_ReturnSuccessWithDto()
    {
        weatherServiceMock.Setup(s => s.GetForecastWeatherAsync("Ha Noi", It.IsAny<int>())).ReturnsAsync(new ForecastWeather
        {
            Location = "Ha Noi",
            Country = "Viet Nam",
            Forecast = new List<DailyForecast>
            {
            new DailyForecast
            {
                Date = "2026-04-29",
                MaxTempC = 32.0,
                MinTempC = 25.0,
                Condition = "Sunny"
            },
            new DailyForecast
            {
                Date = "2026-04-30",
                MaxTempC = 30.0,
                MinTempC = 24.0,
                Condition = "Cloudy"
            },
            new DailyForecast
            {
                Date = "2026-05-01",
                MaxTempC = 28.0,
                MinTempC = 23.0,
                Condition = "Rainy"
            },
            new DailyForecast
            {
                Date = "2026-05-02",
                MaxTempC = 29.0,
                MinTempC = 24.0,
                Condition = "Partly Cloudy"
            }
            }
        });
        var query = new GetForecastWeatherQuery("Ha Noi");
        var handler = new GetForecastWeatherHandler(weatherServiceMock.Object);

        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        //Assert
        await Verifier.Verify(result);

    }
    [Fact]
    public async Task Handle_ServiceReturnNull_ReturnsFailure()
    {
        weatherServiceMock.Setup(s => s.GetForecastWeatherAsync(It.IsAny<string>())).ReturnsAsync((ForecastWeather?)null);
        var query = new GetForecastWeatherQuery("Ha Noi");
        var handler = new GetForecastWeatherHandler(weatherServiceMock.Object);
        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        //Assert
        Assert.False(result.IsSuccess);
        Assert.Null(result.Data);
        Assert.Equal("null", result.Error);

    }
    [Fact]
    public async Task Handle_ServiceThrowsException_ReturnsFailure()
    {
        weatherServiceMock.Setup(s => s.GetForecastWeatherAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Network Error"));
        var query = new GetForecastWeatherQuery("Ha Noi");
        var handler = new GetForecastWeatherHandler(weatherServiceMock.Object);
        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        //Assert
        await Verifier.Verify(result);
    }
}