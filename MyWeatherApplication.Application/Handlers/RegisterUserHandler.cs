using MediatR;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Application.Interfaces;
using MyWeatherApplication.Domain.Common;

public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, Result<Unit>>
{
    private readonly IAuthService _authService; 
    public RegisterUserHandler(IAuthService authService)
    {
        _authService = authService; 
    }
    public async Task<Result<Unit>> Handle(RegisterUserCommand query, CancellationToken cancellationToken)
    {
        return await _authService.RegisterAsync(query.RegisterRequest); 
    }
}