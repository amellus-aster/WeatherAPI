namespace MyWeatherApplication.Application.DTOs;

public class AuthTokenDTO
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string? RefreshToken { get; set; }
    public string TokenType { get; set; } = "Bearer";

}

