
using MediatR;
using Moq;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Application.Interfaces;
using MyWeatherApplication.Domain.Common;
namespace WeatherAPI.Tests.Application.Handlers.Auth;


public class RegisterUserHandlerTests
{
    private readonly Mock<IAuthService> authServiceMock = new();
    [Fact]
    public async Task Handle_ValidUser_ReturnSuccessDto()
    {
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };
        authServiceMock.Setup(s => s.RegisterAsync(request)).ReturnsAsync(Result<Unit>.Success(Unit.Value));
        var query = new RegisterUserCommand(request);
        var handler = new RegisterUserHandler(authServiceMock.Object);
        var result = await handler.Handle(query, CancellationToken.None);
        await Verifier.Verify(result);

    }
}