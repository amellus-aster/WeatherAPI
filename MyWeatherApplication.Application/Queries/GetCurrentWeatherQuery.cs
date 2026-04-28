using MediatR;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Domain.Common;
namespace MyWeatherApplication.Application.Queries;


public record GetCurrentWeatherQuery(string Location) : IRequest<Result<CurrentWeatherDto>>; 
