using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using MyWeatherApplication.Domain.Entities;
using MyWeatherApplication.Domain.Enums;
using MyWeatherApplication.Infrastructure.Jwt;

namespace WeatherAPI.Tests.Infrastructure;
public class JwtTokenGeneratorTests
{
    [Fact]
public void GenerateToken_ShouldReturnValidToken_WithCorrectClaims()
{
    // 1. Arrange: Chuẩn bị dữ liệu mẫu và Mock Options
    var user = new User { Id = Guid.NewGuid(), Email = "test@example.com", Role = Role.Admin};
    var settings = Options.Create(new JwtSettings 
    { 
        SecretKey = "Day_La_Mot_Cai_Key_Sieu_Bao_Mat_123456", 
        Issuer = "MyWeatherApp", 
        Audience = "WeatherUsers", 
        ExpirationMinutes = 60 
    });
    var generator = new JwtTokenGenerator(settings);

    // 2. Act: Chạy hàm tạo Token
    var tokenString = generator.GenerateToken(user);

    // 3. Assert: Giải mã Token để kiểm tra
    var handler = new JwtSecurityTokenHandler();
    var jwtToken = handler.ReadJwtToken(tokenString);

    Assert.Equal("MyWeatherApp", jwtToken.Issuer);
    Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Email && c.Value == user.Email);
    Assert.Contains(jwtToken.Claims, c => c.Type == ClaimTypes.Role && c.Value == Role.Admin.ToString());
    Assert.True(jwtToken.ValidTo > DateTime.UtcNow); 
}
}

