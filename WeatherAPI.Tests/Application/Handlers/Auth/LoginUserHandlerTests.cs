using Moq;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Application.Interfaces;
using MyWeatherApplication.Domain.Common;

namespace WeatherAPI.Tests.Application.Handlers.Auth;

public class LoginUserHandlerTests
{
    private readonly Mock<IAuthService> authServiceMock = new();
    [Fact]
    public async Task Handle_ValidUser_ReturnSuccessDto()
    {
        var request = new LoginRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };
        authServiceMock.Setup(s => s.LoginAsync(request)).ReturnsAsync(Result<AuthTokenDTO>.Success(new AuthTokenDTO
        { AccessToken = "token", ExpiresAt = DateTime.UtcNow }));
        var query  =  new LoginUserCommand(request); 
        var handler = new LoginUserHandler(authServiceMock.Object);
        var result = await handler.Handle(query, CancellationToken.None); 
        await Verifier.Verify(result); 
    }

}
