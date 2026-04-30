using Xunit;
using Moq;
using MyWeatherApplication.Application.Handlers;
using MyWeatherApplication.Application.Queries;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Domain.Entities;
using MyWeatherApplication.Domain.Common;
using MyWeatherApplication.Domain.Interfaces;
using VerifyXunit;

namespace WeatherAPI.Tests.Application.Handlers;

public class GetCurrentWeatherHandlerTests
{
    private readonly Mock<IWeatherService> weatherServiceMock = new();
    [Fact]
    public async Task Handle_ValidLocation_ReturnSuccessWithDto()
    {
        weatherServiceMock.Setup(s => s.GetCurrentWeatherAsync("Ha Noi")).ReturnsAsync(new CurrentWeather
        {
            Location = "Ha Noi",
            Country = "Viet Nam",
            TemperatureC = 25,
            Condition = "Sunny",
            WindSpeed = 56,
            Humidity = 2,
            FeelslikeC = 26,
            UvIndex = 5,
            Cloud = 0,
        });
        var query = new GetCurrentWeatherQuery("Ha Noi");
        var handler = new GetCurrentWeatherHandler(weatherServiceMock.Object);
        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        //Assert
        await Verifier.Verify(result);

    }
    [Fact]
    public async Task Handle_ServiceReturnNull_ReturnsFailure()
    {
        weatherServiceMock.Setup(s => s.GetCurrentWeatherAsync(It.IsAny<string>())).ReturnsAsync((CurrentWeather?)null);
        var query = new GetCurrentWeatherQuery("Ha Noi");
        var handler = new GetCurrentWeatherHandler(weatherServiceMock.Object);
        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        //Assert
        Assert.False(result.IsSuccess); 
        Assert.Null(result.Data);
        Assert.Equal("null",result.Error );

    }
    [Fact]
    public async Task Handle_ServiceThrowsException_ReturnsFailure()
    {
        weatherServiceMock.Setup(s => s.GetCurrentWeatherAsync(It.IsAny<string>())).ThrowsAsync(new Exception("Network Error"));
        var query = new GetCurrentWeatherQuery("Ha Noi");
        var handler = new GetCurrentWeatherHandler(weatherServiceMock.Object);
        //Act
        var result = await handler.Handle(query, CancellationToken.None);
        //Assert
        await Verifier.Verify(result);
    }
}