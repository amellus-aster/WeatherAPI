using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyWeatherApplication.Application.DTOs;

namespace WeatherAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;
    private readonly ILogger<AuthController> _logger;
    public AuthController(ISender sender, ILogger<AuthController> logger)
    {
        _sender = sender;
        _logger = logger;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
    {
        if (registerRequest == null)
        {
            _logger.LogWarning("Register API called with null request");
            return BadRequest("Empty Email or Password");
        }
        _logger.LogInformation("Register API called for {Email}", registerRequest.Email);
        var query = new RegisterUserCommand(registerRequest);
        var result = await _sender.Send(query);
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Register API failed for {Email}: {Error}", registerRequest.Email, result.Error);
            return BadRequest(result.Error);
        }
        _logger.LogInformation("User registered successfully via API for {Email}", registerRequest.Email);
        return Ok(result.Data);
    }
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (loginRequest == null)
        {
            _logger.LogWarning("Login API called with null request");
            return BadRequest("Empty Email or Password");
        }
        _logger.LogInformation("Login API called for {Email}", loginRequest.Email);
        var query = new LoginUserCommand(loginRequest);
        var result = await _sender.Send(query);
        if (!result.IsSuccess)
        {
            _logger.LogWarning("Login API failed for {Email}: {Error}", loginRequest.Email, result.Error);
            return BadRequest(result.Error);
        }
        _logger.LogInformation("User logged in successfully via API for {Email}", loginRequest.Email);
        return Ok(result.Data);
    }

}

