using MyWeatherApplication.Domain.Entities;

namespace MyWeatherApplication.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default); //lay user theo email
    Task<User?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default); //lay user theo id
    Task<bool> EmailExistAsync(string email, CancellationToken cancellationToken = default); //kiem  tra email ton tai 
    Task AddAsync(User user, CancellationToken cancellationToken = default); //them user 

}

