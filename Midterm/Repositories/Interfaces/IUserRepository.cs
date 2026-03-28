using Midterm.Api.Models;

using Midterm.Models;

namespace Midterm.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task<bool> SaveChangesAsync();
    }
}