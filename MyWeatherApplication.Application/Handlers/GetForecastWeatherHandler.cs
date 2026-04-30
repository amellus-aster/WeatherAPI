using MediatR;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Application.Queries;
using MyWeatherApplication.Domain.Common;
using MyWeatherApplication.Domain.Entities;
using MyWeatherApplication.Domain.Interfaces;
namespace MyWeatherApplication.Application.Handlers;

public class GetForecastWeatherHandler : IRequestHandler<GetForecastWeatherQuery, Result<ForecastWeatherDto>>
{
    private readonly IWeatherService _weatherService;
    public GetForecastWeatherHandler(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }
    public async Task<Result<ForecastWeatherDto>> Handle(GetForecastWeatherQuery query, CancellationToken cancellationToken)
    {
        try
        {
            ForecastWeather? weather = await _weatherService.GetForecastWeatherAsync(query.Location, query.Days);
            if (weather == null) return Result<ForecastWeatherDto>.Failure("null");
            var dto = new ForecastWeatherDto
            {
                Location = weather.Location,
                Country = weather.Country,
                Forecast = weather.Forecast.Select(
                    f => new ForecastDayDto
                    {
                        Date = f.Date,
                        Sunrise = f.Sunrise,
                        Sunset = f.Sunrise,
                        MoonPhase = f.MoonPhase,
                        Condition = f.Condition,
                        MaxTempC = f.MaxTempC,
                        MinTempC = f.MinTempC,
                        AvgTempC = f.AvgTempC
                    }
                 ).ToList()
            };
            return Result<ForecastWeatherDto>.Success(dto);
        }
        catch (Exception ex)
        {

            return Result<ForecastWeatherDto>.Failure($"{ex.Message}");
        }
    }
}

