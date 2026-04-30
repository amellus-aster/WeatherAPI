using MediatR;
using VerifyXunit;
using Xunit;
using Moq;
using MyWeatherApplication.Application.Queries;
using MyWeatherApplication.Domain.Common;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.WeatherAPI.Controllers;
public class WeatherControllerTests
{
    private readonly Mock<ISender> _senderMock = new();
    [Fact]
    public async Task GetCurrentWeather_ValidRequest_ReturnSuccessStatus()
    {
        var query = new GetCurrentWeatherQuery("Ha Noi");
        _senderMock.Setup(s => s.Send(query)).ReturnsAsync(Result<CurrentWeatherDto>.Success(new CurrentWeatherDto
        {
            Location = "London",
            Country = "United Kingdom",
            TemperatureC = 25,
            Condition = "Sunny",
            Humidity = 1,
            FeelslikeC = 26,
            Cloud = 1
        }));
        var controller = new WeatherController(_senderMock.Object);
        //Act
        var result = controller.GetCurrentWeather("Ha Noi");
        //Assert
        await Verifier.Verify(result);
    }
    [Fact]
    public async Task GetCurrentWeather_HandlerError_ReturnBadStatus()
    {
        var query = new GetCurrentWeatherQuery("Ha Noi");
        _senderMock.Setup(s => s.Send(query)).ReturnsAsync(Result<CurrentWeatherDto>.Failure("example error"));
        var controller = new WeatherController(_senderMock.Object);
        var result = controller.GetCurrentWeather("Ha Noi");
        //Assert
        await Verifier.Verify(result);
    }
    [Fact]
    public async Task GetForecastWeather_ValidRequest_ReturnSuccessStatus()
    {
        var query = new GetForecastWeatherQuery("Ha Noi");
        _senderMock.Setup(s => s.Send(query)).ReturnsAsync(Result<ForecastWeatherDto>.Success(new ForecastWeatherDto
        {
            Location = "Ha Noi",
            Country = "Viet Nam",
            Forecast = new List<ForecastDayDto>
{
    new ForecastDayDto
    {
        Date = "2026-05-01",
        Sunrise = "05:35 AM",
        Sunset = "08:22 PM",
        MoonPhase = "Waxing Gibbous",
        Condition = "Sunny",
        MaxTempC = 17.6,
        MinTempC = 9.9,
        AvgTempC = 13.6
    },
    new ForecastDayDto
    {
        Date = "2026-05-02",
        Sunrise = "05:34 AM",
        Sunset = "08:23 PM",
        MoonPhase = "Full Moon",
        Condition = "Partly Cloudy",
        MaxTempC = 20.1,
        MinTempC = 11.2,
        AvgTempC = 15.5
    },
    new ForecastDayDto
    {
        Date = "2026-05-03",
        Sunrise = "05:33 AM",
        Sunset = "08:24 PM",
        MoonPhase = "Waning Gibbous",
        Condition = "Light Rain",
        MaxTempC = 15.8,
        MinTempC = 10.5,
        AvgTempC = 13.1
    }
}
        }));
        var controller = new WeatherController(_senderMock.Object);
        //Act
        var result = controller.GetForecastWeather("Ha Noi");
        //Assert
        await Verifier.Verify(result);
    }
    [Fact]
    public async Task GetForecastWeather_HandlerError_ReturnBadStatus()
    {
        var query = new GetForecastWeatherQuery("Ha Noi");
        _senderMock.Setup(s => s.Send(query)).ReturnsAsync(Result<ForecastWeatherDto>.Failure("example error"));
        var controller = new WeatherController(_senderMock.Object);
        var result = controller.GetForecastWeather("Ha Noi");
        //Assert
        await Verifier.Verify(result);
    }
}