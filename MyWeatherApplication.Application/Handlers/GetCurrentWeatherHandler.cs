using MediatR;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Application.Queries;
using MyWeatherApplication.Domain.Common;
using MyWeatherApplication.Domain.Interfaces;
namespace MyWeatherApplication.Application.Handlers;

public class GetCurrentWeatherHandler : IRequestHandler<GetCurrentWeatherQuery, Result<CurrentWeatherDto>>
{
    private readonly IWeatherService _weatherService;
    public GetCurrentWeatherHandler(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }
    public async Task<Result<CurrentWeatherDto>> Handle(GetCurrentWeatherQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var weather = await _weatherService.GetCurrentWeatherAsync(query.Location);
            if (weather == null) return Result<CurrentWeatherDto>.Failure("null");
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
            return Result<CurrentWeatherDto>.Success(dto);
        }
        catch (Exception ex)
        {

            return Result<CurrentWeatherDto>.Failure($"{ex.Message}");
        }
    }
}

