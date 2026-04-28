using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Http;
using MyWeatherApplication.Domain.Interfaces;
using MyWeatherApplication.Infrastructure.Services;
namespace MyWeatherApplication.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<WeatherApiSettings>()
            .Bind(configuration.GetSection(WeatherApiSettings.SectionName));
        services.AddHttpClient<IWeatherService, WeatherService>(); 
        // services.AddScoped<IWeatherService, WeatherService>(); 
        return services; 
    }
}

