using Midterm.Models;



namespace Midterm.Repositories.Interfaces
{
    public interface IBookingRepository
    {
        Task AddAsync(Booking booking);
        Task<Booking?> GetByIdAsync(int id);
        Task<Booking?> GetBookingByIdAndGuestIdAsync(int bookingId, int guestId);
        Task<bool> ExistsOverlappingBookingAsync(int listingId, DateTime fromDate, DateTime toDate);
        Task<bool> SaveChangesAsync();
    }
}