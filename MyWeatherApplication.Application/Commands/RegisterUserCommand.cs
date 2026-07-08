using MediatR;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Domain.Common;

public record RegisterUserCommand(RegisterRequest RegisterRequest) : IRequest<Result<Unit>>; 