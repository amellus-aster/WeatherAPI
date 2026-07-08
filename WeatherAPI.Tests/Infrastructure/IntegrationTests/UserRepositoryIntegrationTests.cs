using MyWeatherApplication.Infrastructure.Data;
using MyWeatherApplication.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using MyWeatherApplication.Domain.Entities;
using MyWeatherApplication.Domain.Enums;
using Testcontainers.PostgreSql; 

namespace WeatherAPI.Tests.Infrastructure.IntegrationTests;

public class UserRepositoryIntegrationTests : IAsyncLifetime
{
    // Khai báo container Postgres ảo của Docker
    private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder("postgres:15.18-trixie")
        .WithDatabase("weather_test_db")
        .WithUsername("test_user")
        .WithPassword("test_password")
        .Build();

    private AppDbContext _dbContext = null!;
    private UserRepository _repository = null!;

    public async Task InitializeAsync()
    {
        // 1. Kích hoạt Docker tự dựng container Postgres lên ngầm
        await _postgresContainer.StartAsync();

        // 2. Lấy Connection String động do Docker cấp phát
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseNpgsql(_postgresContainer.GetConnectionString()) // Kết nối tới Postgres thật
            .Options;

        _dbContext = new AppDbContext(options);
        
        // 3. Tạo cấu trúc bảng (Migration/Schema) trên database Postgres ảo đó
        await _dbContext.Database.EnsureCreatedAsync();

        _repository = new UserRepository(_dbContext);
    }

    public async Task DisposeAsync()
    {
        // Dọn dẹp db và tự động XÓA CONTAINER khỏi Docker, không để lại rác
        if (_dbContext != null) await _dbContext.DisposeAsync();
        await _postgresContainer.DisposeAsync();
    }

    [Fact]
    public async Task AddAsync_ShouldSaveUserToDatabase()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com",
            PasswordHash = "hashed123",
            Role = Role.User,
            CreatedAtUtc = DateTime.UtcNow
        };

        // Act
        await _repository.AddAsync(user, CancellationToken.None);

        // Assert
        var savedUser = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
        Assert.NotNull(savedUser);
        await Verifier.Verify(savedUser);
    }

    [Fact]
    public async Task EmailExistAsync_WhenEmailExists_ShouldReturnTrue()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "existing@example.com",
            PasswordHash = "hash"
        };
        await _repository.AddAsync(user, CancellationToken.None);

        // Act
        var exists = await _repository.EmailExistAsync("existing@example.com", CancellationToken.None);

        // Assert
        Assert.True(exists);
    }

    [Fact]
    public async Task EmailExistAsync_WhenEmailNotExists_ShouldReturnFalse()
    {
        // Act
        var exists = await _repository.EmailExistAsync("nonexistent@example.com", CancellationToken.None);

        // Assert
        Assert.False(exists);
    }

    [Fact]
    public async Task GetByEmailAsync_WithDifferentCase_ShouldFindUser()
    {
        // Arrange
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@example.com", 
            PasswordHash = "hash",  
        };
        await _repository.AddAsync(user, CancellationToken.None);

        // Act
        var found = await _repository.GetByEmailAsync("  TEST@Example.COM  ", CancellationToken.None);

        // Assert
        Assert.NotNull(found);
    }

    [Fact]
    public async Task GetByEmailAsync_WhenEmailNotExists_ShouldReturnNull()
    {
        // Act
        var found = await _repository.GetByEmailAsync("notexist@example.com", CancellationToken.None);

        // Assert
        Assert.Null(found);
    }
}