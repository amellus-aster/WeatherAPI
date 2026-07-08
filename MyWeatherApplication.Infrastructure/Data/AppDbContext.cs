using MyWeatherApplication.Domain.Entities;
using Microsoft.EntityFrameworkCore;
namespace MyWeatherApplication.Infrastructure.Data;


public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<User> Users => Set<User>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
        var user = modelBuilder.Entity<User>();
        user.HasKey(u => u.Id);
        
        user.Property(x => x.Email).IsRequired().HasMaxLength(320);
        user.HasIndex(x => x.Email).IsUnique();

        user.Property(x => x.PasswordHash).IsRequired();
        user.Property(x => x.Role).IsRequired();
        user.Property(x => x.CreatedAtUtc).IsRequired();
        base.OnModelCreating(modelBuilder);
    }
}