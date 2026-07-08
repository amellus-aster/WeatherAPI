using MediatR;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Application.Interfaces;
using MyWeatherApplication.Domain.Common;

public class LoginUserHandler : IRequestHandler<LoginUserCommand, Result<AuthTokenDTO>>
{
    private readonly IAuthService _authService; 
    public LoginUserHandler(IAuthService authService)
    {
        _authService = authService; 
    }
    public async Task<Result<AuthTokenDTO>> Handle(LoginUserCommand query, CancellationToken cancellationToken)
    {
        return await _authService.LoginAsync(query.LoginRequest); 
    }
}