using System.Text.Json;
using MediatR;
using Microsoft.Extensions.Logging;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Application.Queries;
using MyWeatherApplication.Domain.Common;
using MyWeatherApplication.Domain.Entities;
using MyWeatherApplication.Domain.Interfaces;
namespace MyWeatherApplication.Application.Handlers;

public class GetForecastWeatherHandler : IRequestHandler<GetForecastWeatherQuery, Result<ForecastWeatherDto>>
{
    private readonly IWeatherService _weatherService;
    private readonly ILogger<GetForecastWeatherHandler> _logger;
    public GetForecastWeatherHandler(IWeatherService weatherService, ILogger<GetForecastWeatherHandler> logger)
    {
        _weatherService = weatherService;
        _logger = logger;
    }
    public async Task<Result<ForecastWeatherDto>> Handle(GetForecastWeatherQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling  GetForecastWeather for {Location}", query.Location);
        try
        {
            ForecastWeather? weather = await _weatherService.GetForecastWeatherAsync(query.Location, query.Days);
            if (weather == null)
            {
                _logger.LogWarning("Weather data not found for {Location}", query.Location);
                return Result<ForecastWeatherDto>.Failure("null");
            }

            var dto = new ForecastWeatherDto
            {
                Location = weather.Location,
                Country = weather.Country,
                Forecast = weather.Forecast.Select(
                    f => new ForecastDayDto
                    {
                        Date = f.Date,
                        Sunrise = f.Sunrise,
                        Sunset = f.Sunset,
                        MoonPhase = f.MoonPhase,
                        Condition = f.Condition,
                        MaxTempC = f.MaxTempC,
                        MinTempC = f.MinTempC,
                        AvgTempC = f.AvgTempC
                    }
                 ).ToList()
            };
            _logger.LogInformation("Successfully retrieved forecast weather for {Location}. in  {Temp} days",
               query.Location, dto.Forecast.Count);
            return Result<ForecastWeatherDto>.Success(dto);
        }
          catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "External API error when getting weather for {Location}", query.Location);
            return Result<ForecastWeatherDto>.Failure($"Weather service unavailable: {ex.Message}");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "JSON parsing error for weather data from {Location}", query.Location);
            return Result<ForecastWeatherDto>.Failure("Error processing weather data");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Unexpected error in GetCurrentWeatherHandler for {Location}", query.Location);
            return Result<ForecastWeatherDto>.Failure("An unexpected error occurred");
        }
    }
}

