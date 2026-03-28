using Midterm.Models;



namespace Midterm.Repositories.Interfaces
{
    public interface IGuestQueryUsageRepository
    {
        Task<GuestQueryUsage?> GetTodayUsageAsync(string clientKey, DateTime queryDate);
        Task AddAsync(GuestQueryUsage usage);
        Task<bool> SaveChangesAsync();
    }
}