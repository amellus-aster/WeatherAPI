using Microsoft.AspNetCore.Mvc;
using MediatR;
using MyWeatherApplication.Application.Queries;
using MyWeatherApplication.Domain.Common;
using MyWeatherApplication.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
namespace WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ILogger<WeatherController> _logger;
        public WeatherController(ISender sender, ILogger<WeatherController> logger)
        {
            _sender = sender;
            _logger = logger;
        }
        /// <summary>
        /// method which gets current weather (required authentication - valid token)
        /// </summary>
        /// <param name="location">location name (city, ...) </param>
        /// <returns>current weather information</returns>
        [Authorize]
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentWeather([FromQuery] string location)
        {
            _logger.LogInformation("GetCurrentWeather request received for {Location}", location);
            if (string.IsNullOrWhiteSpace(location)){
                _logger.LogWarning("GetCurrentWeather called with missing or empty location parameter"); 
                return BadRequest("location parameter is required");
            }
            var query = new GetCurrentWeatherQuery(location);
            Result<CurrentWeatherDto> result = await _sender.Send(query);
            if (!result.IsSuccess){
                _logger.LogWarning("GetCurrentWeather failed for {Location}. Error: {Error}", location, result.Error);
                return BadRequest(result.Error);
            }
            _logger.LogInformation("GetCurrentWeather succeeded for {Location}. Temp: {Temp}", location, result.Data!.TemperatureC); 
            return Ok(result.Data);
        }
        /// <summary>
        /// method which gets forecast for 3 days foward (required authentication - valid token)
        /// </summary>
        /// <param name="location">location name (city, ...)</param>
        ///  <param name="days">days forecast range</param>
        /// <returns>forecast information</returns>
        [Authorize]
        [HttpGet("forecast")]
        public async Task<IActionResult> GetForecastWeather([FromQuery] string location, int days = 3)
        {
            _logger.LogInformation("GetForecastWeather request received for {Location}", location);
            if (string.IsNullOrWhiteSpace(location) || days < 0)
            {
                _logger.LogWarning("GetForecastWeather called with missing or empty location parameter");
                return BadRequest("location parameter is invalid or days may be invalid either");
            }
            var query = new GetForecastWeatherQuery(location, days);
            Result<ForecastWeatherDto> result = await _sender.Send(query);
            if (!result.IsSuccess)
            {
                _logger.LogWarning("GetForecastWeather failed for {Location}. Error: {Error}", location, result.Error);
                return BadRequest(result.Error);
            }
            _logger.LogInformation("GetForecastWeather succeeded for {Location}. Forecast days: {Days}", location, result.Data!.Forecast.Count); 
            return Ok(result.Data);
        }
    }
}