using Midterm.Data;
using Midterm.Models;
using Midterm.Repositories.Interfaces;




using Microsoft.EntityFrameworkCore;


namespace Midterm.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly ApplicationDbContext _context;

        public BookingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Booking booking)
        {
            await _context.Bookings.AddAsync(booking);
        }

        public async Task<Booking?> GetByIdAsync(int id)
        {
            return await _context.Bookings
                .Include(x => x.Listing)
                .Include(x => x.Guest)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Booking?> GetBookingByIdAndGuestIdAsync(int bookingId, int guestId)
        {
            return await _context.Bookings
                .Include(x => x.Listing)
                .Include(x => x.Guest)
                .FirstOrDefaultAsync(x => x.Id == bookingId && x.GuestId == guestId);
        }

        public async Task<bool> ExistsOverlappingBookingAsync(int listingId, DateTime fromDate, DateTime toDate)
        {
            return await _context.Bookings.AnyAsync(b =>
                b.ListingId == listingId &&
                b.Status == "Confirmed" &&
                fromDate < b.ToDate &&
                toDate > b.FromDate);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}