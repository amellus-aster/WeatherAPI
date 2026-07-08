using Microsoft.EntityFrameworkCore;
using MyWeatherApplication.Domain.Entities;
using MyWeatherApplication.Domain.Interfaces;
using MyWeatherApplication.Infrastructure.Data;

namespace MyWeatherApplication.Infrastructure.Repositories;


public class UserRepository : IUserRepository
{
    private readonly AppDbContext _db;
    public UserRepository(AppDbContext db)
    {
        _db = db;
    }
    public Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var normalizedeEmail = email.Trim().ToLowerInvariant();
        return _db.Users.FirstOrDefaultAsync(u => u.Email == normalizedeEmail, cancellationToken);
    }
    public Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken)
    => _db.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
    public Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken)
    {
        var normalizedeEmail = email.Trim().ToLowerInvariant();
        return _db.Users.AnyAsync(u => u.Email == normalizedeEmail, cancellationToken);
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken)
    {
        await _db.Users.AddAsync(user, cancellationToken);
        await _db.SaveChangesAsync();
    }
}