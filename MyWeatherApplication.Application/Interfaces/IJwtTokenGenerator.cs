using MyWeatherApplication.Domain.Entities;

namespace MyWeatherApplication.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user); 
}

