using MediatR;
using  BCrypt.Net;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Application.Interfaces;
using MyWeatherApplication.Domain.Common;
using MyWeatherApplication.Domain.Entities;
using MyWeatherApplication.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;

namespace MyWeatherApplication.Infrastructure.Auth;

public class AuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly ILogger<AuthService> _logger; 
    public AuthService(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, ILogger<AuthService> logger)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _logger = logger; 
    }
    public async Task<Result<Unit>> RegisterAsync(RegisterRequest registerRequest)
    {
        _logger.LogInformation("Registration attempt for {Email}", registerRequest.Email); 
        if (await _userRepository.EmailExistAsync(registerRequest.Email)){
            _logger.LogWarning("Registration failed: Email {Email} already exists",registerRequest.Email ); 
            return Result<Unit>.Failure("Email already registered");
        }
        var user = new User { Email = registerRequest.Email, PasswordHash = HashPassword(registerRequest.Password) };   
        await _userRepository.AddAsync(user);
         _logger.LogInformation("User {UserId} registered successfully with email {Email}", user.Id, user.Email);
        return Result<Unit>.Success(Unit.Value);

    }
    public async Task<Result<AuthTokenDTO>> LoginAsync(LoginRequest loginRequest)
    {
        _logger.LogInformation("Login attempt for {Email}", loginRequest.Email); 
        var user = await _userRepository.GetByEmailAsync(loginRequest.Email);
        if (user == null){
            _logger.LogWarning("Login failed: User {Email} not found", loginRequest.Email); 
            return Result<AuthTokenDTO>.Failure("User Not Found");
        }
        if (!VerifyPassword(loginRequest.Password, user.PasswordHash))
        {
            _logger.LogWarning("Login failed: Wrong password for {Email}", loginRequest.Email); 
            return Result<AuthTokenDTO>.Failure("Wrong password");
        }
        string token = _jwtTokenGenerator.GenerateToken(user);
        JwtSecurityToken jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
        var expires = jwtToken.ValidTo;
        _logger.LogInformation("User {UserId} logged in successfully. Token expires at {ExpiresAt}", user.Id, expires); 
        return Result<AuthTokenDTO>.Success(new AuthTokenDTO
        {
            AccessToken = token,
            ExpiresAt = expires
        });

    }
    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
    private bool VerifyPassword(string password, string hashPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashPassword);
    }
}

