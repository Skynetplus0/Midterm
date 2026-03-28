using Midterm.Data;
using Midterm.Models;
using Midterm.Repositories.Interfaces;



using Microsoft.EntityFrameworkCore;


namespace Midterm.Repositories
{
    public class ListingRepository : IListingRepository
    {
        private readonly ApplicationDbContext _context;

        public ListingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Listing listing)
        {
            await _context.Listings.AddAsync(listing);
        }

        public async Task<Listing?> GetByIdAsync(int id)
        {
            return await _context.Listings
                .Include(x => x.Reviews)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<(List<Listing> Items, int TotalCount)> QueryAvailableListingsAsync(
            DateTime fromDate,
            DateTime toDate,
            int noOfPeople,
            string country,
            string city,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Listings
                .Include(x => x.Reviews)
                .Where(x => x.IsActive)
                .Where(x => x.NoOfPeople >= noOfPeople)
                .Where(x => x.Country.ToLower() == country.ToLower())
                .Where(x => x.City.ToLower() == city.ToLower())
                .Where(x => !_context.Bookings.Any(b =>
                    b.ListingId == x.Id &&
                    b.Status == "Confirmed" &&
                    fromDate < b.ToDate &&
                    toDate > b.FromDate
                ));

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(x => x.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}