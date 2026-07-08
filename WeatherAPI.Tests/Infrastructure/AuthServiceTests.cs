using Microsoft.Extensions.Logging;
using Moq;
using MyWeatherApplication.Application.DTOs;
using MyWeatherApplication.Application.Interfaces;
using MyWeatherApplication.Domain.Entities;
using MyWeatherApplication.Domain.Enums;
using MyWeatherApplication.Domain.Interfaces;
using MyWeatherApplication.Infrastructure.Auth;

namespace WeatherAPI.Tests.Infrastructure;


public class AuthServiceTests
{
    private readonly Mock<IUserRepository> userRepositoryMock = new();
    private readonly Mock<IJwtTokenGenerator> jwtTokenGeneratorMock = new();
    private readonly Mock<ILogger<AuthService>> _loggerMock = new(); 

    [Fact]
    public async Task RegisterAsync_EmailDoesntExists_ReturnSuccess()
    {
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };
        userRepositoryMock.Setup(r => r.EmailExistAsync(request.Email)).ReturnsAsync(false);
        var authService = new AuthService(userRepositoryMock.Object, jwtTokenGeneratorMock.Object, _loggerMock.Object);

        //Act
        var result = await authService.RegisterAsync(request);
        await Verifier.Verify(result);

    }
    [Fact]
    public async Task RegisterAsync_EmailExists_ReturnFailure()
    {
        var request = new RegisterRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };
        userRepositoryMock.Setup(r => r.EmailExistAsync(request.Email)).ReturnsAsync(true);
        var authService = new AuthService(userRepositoryMock.Object, jwtTokenGeneratorMock.Object, _loggerMock.Object);

        //Act
        var result = await authService.RegisterAsync(request);
        await Verifier.Verify(result);
    }
    [Fact]
    public async Task RegisterAsync_RepositoryThrowException_ReturnFailure()
    {
        var request = new RegisterRequest { Email = "test@example.com", Password = "password123" };
        userRepositoryMock.Setup(r => r.AddAsync(It.IsAny<User>())).ThrowsAsync(new Exception("repo error"));
        var authService = new AuthService(userRepositoryMock.Object, jwtTokenGeneratorMock.Object, _loggerMock.Object);
        await Assert.ThrowsAsync<Exception>(() => authService.RegisterAsync(request));

    }

    [Fact]
    public async Task LoginAsync_ValidEmailAndPassword_ReturnToken()
    {

        var email = "test@example.com";
        var password = "password123";
        var request = new LoginRequest { Email = email, Password = password };
        var hashPassword = BCrypt.Net.BCrypt.HashPassword(password);
        var testUser = new User
        {
            Email = email,
            Role = Role.User,
            PasswordHash = hashPassword
        };
       var sampleToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
        jwtTokenGeneratorMock
            .Setup(g => g.GenerateToken(It.IsAny<User>()))
            .Returns(sampleToken);
        userRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(testUser);
        var authService = new AuthService(userRepositoryMock.Object, jwtTokenGeneratorMock.Object, _loggerMock.Object);
        var result = await authService.LoginAsync(request);
        await Verifier.Verify(result);
    }
    [Fact]
    public async Task LoginAsync_EmailDoesntExists_ReturnFailure()
    {
        var email = "test@example.com";
        var password = "password123";
        var request = new LoginRequest { Email = email, Password = password };
        string mockToken = "fake-jwt-token";
        jwtTokenGeneratorMock
            .Setup(g => g.GenerateToken(It.IsAny<User>()))
            .Returns(mockToken);
        userRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync((User?)null);
        var authService = new AuthService(userRepositoryMock.Object, jwtTokenGeneratorMock.Object, _loggerMock.Object);
        var result = await authService.LoginAsync(request);
        await Verifier.Verify(result);
    }
    [Fact]
    public async Task LoginAsync_WrongPassword_ReturnFailure()
    {
        var email = "test@example.com";
        var password = "password123";
        var request = new LoginRequest { Email = email, Password = password };
        var hashPassword = BCrypt.Net.BCrypt.HashPassword(password + "wrong");
        var testUser = new User
        {
            Email = email,
            Role = Role.User,
            PasswordHash = hashPassword
        };
        string mockToken = "fake-jwt-token";
        jwtTokenGeneratorMock
            .Setup(g => g.GenerateToken(It.IsAny<User>()))
            .Returns(mockToken);
        userRepositoryMock.Setup(r => r.GetByEmailAsync(email)).ReturnsAsync(testUser);
        var authService = new AuthService(userRepositoryMock.Object, jwtTokenGeneratorMock.Object, _loggerMock.Object);
        var result = await authService.LoginAsync(request);
        await Verifier.Verify(result);
    }
}