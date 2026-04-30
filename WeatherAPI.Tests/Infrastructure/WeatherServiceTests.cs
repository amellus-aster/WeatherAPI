
using VerifyXunit;
using System.Text.Json;
using MyWeatherApplication.Infrastructure.Services;
using RichardSzalay.MockHttp;
using System.Net;

namespace WeatherAPI.Tests.Infrastructure;


public class WeatherServiceTests
{
    private readonly MockHttpMessageHandler mockHttp = new();
    [Fact]
    public async Task GetCurrentWeatherAsync_ValidResponse_ReturnEntity()
    {
        var jsonResponse = JsonSerializer.Serialize(new
        {
            location = new
            {
                name = "London",
                region = "City of London, Greater London",
                country = "United Kingdom",
                lat = 51.5171,
                lon = -0.1062,
                tz_id = "Europe/London",
                localtime_epoch = 1777475980,
                localtime = "2026-04-29 16:19",
            },
            current = new
            {
                last_updated_epoch = 1777475700,
                last_updated = "2026-04-29 16:15",
                temp_c = 18.1,
                temp_f = 64.6,
                is_day = 1,
                condition = new
                {
                    text = "Partly Cloudy",
                    icon = "//cdn.weatherapi.com/weather/64x64/day/116.png",
                    code = 1003
                },
                wind_mph = 17.7,
                wind_kph = 28.4,
                wind_degree = 77,
                wind_dir = "ENE",
                pressure_mb = 1023.0,
                pressure_in = 30.21,
                precip_mm = 0.0,
                precip_in = 0.0,
                humidity = 30,
                cloud = 0,
                feelslike_c = 18.1,
                feelslike_f = 64.6,
                windchill_c = 15.5,
                windchill_f = 59.9,
                heatindex_c = 15.5,
                heatindex_f = 59.9,
                dewpoint_c = 2.9,
                dewpoint_f = 37.3,
                vis_km = 10.0,
                vis_miles = 6.0,
                uv = 2.7,
                gust_mph = 20.3,
                gust_kph = 32.7,
                short_rad = 753.62,
                diff_rad = 120.78,
                dni = 884.07,
                gti = 177.34
            }
        });
        mockHttp.When("https://api.weatherapi.com/v1/current.json*").Respond("application/json", jsonResponse);
        var mockHttpClient = mockHttp.ToHttpClient();
        var setting = ConfigureSetting.GetMockSettings();
        var weatherService = new WeatherService(mockHttpClient, setting);
        //Act
        var result = await weatherService.GetCurrentWeatherAsync("London");
        //Assert
        await Verifier.Verify(result);
    }
    [Fact]
    public async Task GetCurrentWeatherAsync_ApiError_ReturnException()
    {
        mockHttp.When("https://api.weatherapi.com/v1/current.json*").Respond(HttpStatusCode.BadRequest);
        var mockHttpClient = mockHttp.ToHttpClient();
        var setting = ConfigureSetting.GetMockSettings();
        var weatherService = new WeatherService(mockHttpClient, setting);
        //Act
        // var result = await weatherService.GetCurrentWeatherAsync("InvalidCity");
        //Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => weatherService.GetCurrentWeatherAsync("InvalidCity"));
    }
    [Fact]
    public async Task GetForecastWeatherAsync_ValidResponse_ReturnException()
    {
        var jsonResponse = JsonSerializer.Serialize(new
        {
            location = new
            {
                name = "London",
                region = "City of London, Greater London",
                country = "United Kingdom",
                lat = 51.5171,
                lon = -0.1062,
                tz_id = "Europe/London",
                localtime_epoch = 1777475980,
                localtime = "2026-04-29 16:19",
            },
            current = new
            {
                last_updated_epoch = 1777475700,
                last_updated = "2026-04-29 16:15",
                temp_c = 18.1,
                temp_f = 64.6,
                is_day = 1,
                condition = new
                {
                    text = "Partly Cloudy",
                    icon = "//cdn.weatherapi.com/weather/64x64/day/116.png",
                    code = 1003
                },
                wind_mph = 17.7,
                wind_kph = 28.4,
                wind_degree = 77,
                wind_dir = "ENE",
                pressure_mb = 1023.0,
                pressure_in = 30.21,
                precip_mm = 0.0,
                precip_in = 0.0,
                humidity = 30,
                cloud = 0,
                feelslike_c = 18.1,
                feelslike_f = 64.6,
                windchill_c = 15.5,
                windchill_f = 59.9,
                heatindex_c = 15.5,
                heatindex_f = 59.9,
                dewpoint_c = 2.9,
                dewpoint_f = 37.3,
                vis_km = 10.0,
                vis_miles = 6.0,
                uv = 2.7,
                gust_mph = 20.3,
                gust_kph = 32.7,
                short_rad = 753.62,
                diff_rad = 120.78,
                dni = 884.07,
                gti = 177.34
            },
            forecast = new
            {
                forcastday = new[]
               {
                   new
                   {
                        date = "2026-04-30",
                        date_epoch = 1777507200,
                        day = new {
                                maxtemp_c = 17.6,
                                maxtemp_f = 63.7,
                                mintemp_c = 9.9,
                                mintemp_f = 49.9,
                                avgtemp_c = 13.6,
                                avgtemp_f = 56.5,
                                maxwind_mph = 14.8,
                                maxwind_kph = 23.8,
                                totalprecip_mm = 0.0,
                                totalprecip_in = 0.0,
                                totalsnow_cm = 0.0,
                                avgvis_km = 10.0,
                                avgvis_miles = 6.0,
                                avghumidity = 53,
                                daily_will_it_rain = 0,
                                daily_chance_of_rain = 0,
                                daily_will_it_snow = 0,
                                daily_chance_of_snow = 0,
                                condition = new
                                    {
                                        text = "Sunny",
                                        icon = "//cdn.weatherapi.com/weather/64x64/day/113.png",
                                        code = 1000
                                    },
                                uv = 5.1
                        },
                        astro = new
                        {
                            sunrise = "05:35 AM",
                            sunset = "08:22 PM",
                            moonrise = "07:31 PM",
                            moonset = "04:42 AM",
                            moon_phase = "Waxing Gibbous",
                            moon_illumination = 97,
                            is_moon_up = 1,
                            is_sun_up = 0
                        }
                   },

               }
            }
        });
        mockHttp.When("https://api.weatherapi.com/v1/forecast.json*").Respond("application/json", jsonResponse);
        var mockHttpClient = mockHttp.ToHttpClient();
        var setting = ConfigureSetting.GetMockSettings();
        var weatherService = new WeatherService(mockHttpClient, setting);
        //Act
        var result = await weatherService.GetForecastWeatherAsync("London", 3);
        //Assert
        await Verifier.Verify(result);
    }
    [Fact]
    public async Task GetForecastWeatherAsync_ApiError_ReturnEntity()
    {
        mockHttp.When("https://api.weatherapi.com/v1/forecast.json*").Respond(HttpStatusCode.BadRequest);
        var mockHttpClient = mockHttp.ToHttpClient();
        var setting = ConfigureSetting.GetMockSettings();
        var weatherService = new WeatherService(mockHttpClient, setting);
        //Act
        // var result = await weatherService.GetCurrentWeatherAsync("InvalidCity");
        //Assert
        await Assert.ThrowsAsync<HttpRequestException>(() => weatherService.GetForecastWeatherAsync("InvalidCity", 3));
    }
}