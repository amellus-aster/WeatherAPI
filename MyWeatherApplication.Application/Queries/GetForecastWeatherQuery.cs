using MediatR;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Domain.Common;
namespace MyWeatherApplication.Application.Queries;


public record GetForecastWeatherQuery(string Location, int Days = 3) : IRequest<Result<ForecastWeatherDto>>; 