using Midterm.Models;



namespace Midterm.Repositories.Interfaces
{
    public interface IReviewRepository
    {
        Task AddAsync(Review review);
        Task<bool> ExistsByBookingIdAsync(int bookingId);
        Task<Review?> GetByBookingIdAsync(int bookingId);
        Task<bool> SaveChangesAsync();
    }
}