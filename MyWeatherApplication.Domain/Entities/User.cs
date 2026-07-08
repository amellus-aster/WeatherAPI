using MyWeatherApplication.Domain.Enums;
namespace MyWeatherApplication.Domain.Entities;

public class User
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string PasswordHash { get; set; } = string.Empty;
    
    public Role Role { get; set; } = Role.User;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

