using Midterm.Data;
using Midterm.Models;
using Midterm.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;


namespace Midterm.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);
        }

        public async Task<bool> ExistsByBookingIdAsync(int bookingId)
        {
            return await _context.Reviews.AnyAsync(x => x.BookingId == bookingId);
        }

        public async Task<Review?> GetByBookingIdAsync(int bookingId)
        {
            return await _context.Reviews.FirstOrDefaultAsync(x => x.BookingId == bookingId);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}