using Microsoft.AspNetCore.Mvc;
using MediatR;
using MyWeatherApplication.Application.Queries;
using MyWeatherApplication.Domain.Common;
using MyWeatherApplication.Application.DTOs;
namespace MyWeatherApplication.WeatherAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly ISender _sender;
        public WeatherController(ISender sender)
        {
            _sender = sender;
        }
        /// <summary>
        /// method which gets current weather
        /// </summary>
        /// <param name="location">location name (city, ...) </param>
        /// <returns>current weather information</returns>
        [HttpGet("current")]
        public async Task<IActionResult> GetCurrentWeather([FromQuery] string location)
        {
            if (string.IsNullOrWhiteSpace(location)) return BadRequest("location parameter is required");
            var query = new GetCurrentWeatherQuery(location);
            Result<CurrentWeatherDto> result = await _sender.Send(query);
            if (!result.IsSuccess) return BadRequest(result.Error);

            return Ok(result.Data);
        }
        /// <summary>
        /// method which gets forecast for 3 days foward
        /// </summary>
        /// <param name="location">location name (city, ...)</param>
        ///  <param name="days">days forecast range</param>
        /// <returns>forecast information</returns>
        [HttpGet("forecast")]
        public async Task<IActionResult> GetForecastWeather(string location, int days = 3)
        {
            if (string.IsNullOrWhiteSpace(location) || days < 0) return BadRequest("location parameter is invalid or days may be invalid either");
            var query = new GetForecastWeatherQuery(location, days);
            Result<ForecastWeatherDto> result = await _sender.Send(query);
            if (!result.IsSuccess) return BadRequest(result.Error);
            return Ok(result.Data);
        }
    }
}