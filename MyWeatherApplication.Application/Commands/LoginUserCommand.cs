using MediatR;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Domain.Common;

public record LoginUserCommand(LoginRequest LoginRequest) : IRequest<Result<AuthTokenDTO>>; 