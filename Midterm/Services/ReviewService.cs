using Midterm.DTOs.Reviews;
using Midterm.Models;
using Midterm.Repositories.Interfaces;
using Midterm.Services.Interfaces;



namespace Midterm.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(
            IBookingRepository bookingRepository,
            IReviewRepository reviewRepository)
        {
            _bookingRepository = bookingRepository;
            _reviewRepository = reviewRepository;
        }

        public async Task<ReviewResponseDto> CreateReviewAsync(int guestId, CreateReviewRequestDto request)
        {
            var booking = await _bookingRepository.GetBookingByIdAndGuestIdAsync(request.BookingId, guestId);

            if (booking == null)
            {
                throw new UnauthorizedAccessException("Only the guest who booked this stay can review it.");
            }

            var alreadyReviewed = await _reviewRepository.ExistsByBookingIdAsync(request.BookingId);
            if (alreadyReviewed)
            {
                throw new InvalidOperationException("This booking already has a review.");
            }

            var review = new Review
            {
                BookingId = booking.Id,
                ListingId = booking.ListingId,
                GuestId = guestId,
                Rating = request.Rating,
                Comment = request.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepository.AddAsync(review);
            var saved = await _reviewRepository.SaveChangesAsync();

            if (!saved)
            {
                throw new Exception("Review could not be created.");
            }

            return new ReviewResponseDto
            {
                ReviewId = review.Id,
                Status = "Successful",
                Message = "Review submitted successfully."
            };
        }
    }
}