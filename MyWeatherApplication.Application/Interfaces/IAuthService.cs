using MediatR;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Domain.Common;

namespace MyWeatherApplication.Application.Interfaces;

public interface IAuthService
{
    Task<Result<Unit>> RegisterAsync(RegisterRequest registerRequest); 
    Task<Result<AuthTokenDTO>> LoginAsync(LoginRequest loginRequest); 
}

