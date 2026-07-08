using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Http;
using MyWeatherApplication.Domain.Interfaces;
using MyWeatherApplication.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using MyWeatherApplication.Infrastructure.Jwt;
using MyWeatherApplication.Application.Interfaces;
using MyWeatherApplication.Infrastructure.Auth;
using MyWeatherApplication.Infrastructure.Repositories;
using MyWeatherApplication.Infrastructure.Data;
// using MyWeatherApplication.Infrastructure.Repositories;
namespace MyWeatherApplication.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString  = configuration.GetConnectionString("Default"); 
        services.AddOptions<WeatherApiSettings>()
            .Bind(configuration.GetSection(WeatherApiSettings.SectionName));
        services.AddOptions<JwtSettings>()
            .Bind(configuration.GetSection(JwtSettings.SectionName)); 
        services.AddHttpClient<IWeatherService, WeatherService>();
        services.AddScoped<IUserRepository, UserRepository>();  
        services.AddScoped<IAuthService, AuthService>(); 
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>(); 
        // services.AddScoped<IWeatherService, WeatherService>(); 
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString)); 
        // services.AddScoped<IUserRepository, UserRepository>();
        return services; 
    }
}

