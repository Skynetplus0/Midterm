using Midterm.Data;
using Midterm.Models;
using Midterm.Repositories.Interfaces;


using Microsoft.EntityFrameworkCore;


namespace Midterm.Repositories
{
    public class GuestQueryUsageRepository : IGuestQueryUsageRepository
    {
        private readonly ApplicationDbContext _context;

        public GuestQueryUsageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<GuestQueryUsage?> GetTodayUsageAsync(string clientKey, DateTime queryDate)
        {
            return await _context.GuestQueryUsages
                .FirstOrDefaultAsync(x =>
                    x.ClientKey == clientKey &&
                    x.QueryDate.Date == queryDate.Date);
        }

        public async Task AddAsync(GuestQueryUsage usage)
        {
            await _context.GuestQueryUsages.AddAsync(usage);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}