using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Application.Queries;
using MyWeatherApplication.Domain.Common;
using MyWeatherApplication.Domain.Interfaces;
namespace MyWeatherApplication.Application.Handlers;

public class GetCurrentWeatherHandler : IRequestHandler<GetCurrentWeatherQuery, Result<CurrentWeatherDto>>
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<GetCurrentWeatherHandler> _logger;
    public GetCurrentWeatherHandler(IWeatherService weatherService, ILogger<GetCurrentWeatherHandler> logger)
    {
        _weatherService = weatherService;
        _logger = logger;

    }
    public async Task<Result<CurrentWeatherDto>> Handle(GetCurrentWeatherQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetCurrentWeather for {Location}", query.Location);
        try
        {
            var weather = await _weatherService.GetCurrentWeatherAsync(query.Location);
            if (weather == null)
            {
                _logger.LogWarning("Weather data not found for {Location}", query.Location);
                return Result<CurrentWeatherDto>.Failure("null");
            }
            var dto = new CurrentWeatherDto
            {
                Location = weather.Location,
                Country = weather.Country,
                TemperatureC = weather.TemperatureC,
                Condition = weather.Condition,
                Humidity = weather.Humidity,
                FeelslikeC = weather.FeelslikeC,
                Cloud = weather.Cloud,
            };
            _logger.LogInformation("Successfully retrieved weather for {Location}. Temp: {Temp}°C",
                query.Location, dto.TemperatureC);
            return Result<CurrentWeatherDto>.Success(dto);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "External API error when getting weather for {Location}", query.Location);
            return Result<CurrentWeatherDto>.Failure($"Weather service unavailable: {ex.Message}");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing error for weather data from {Location}", query.Location);
            return Result<CurrentWeatherDto>.Failure("Error processing weather data");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Unexpected error in GetCurrentWeatherHandler for {Location}", query.Location);
            return Result<CurrentWeatherDto>.Failure("An unexpected error occurred");
        }
    }
}

