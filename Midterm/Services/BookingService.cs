using Midterm.DTOs.Bookings;
using Midterm.Models;
using Midterm.Repositories.Interfaces;
using Midterm.Services.Interfaces;
using System.Text.Json;



namespace Midterm.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IListingRepository _listingRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IListingRepository listingRepository)
        {
            _bookingRepository = bookingRepository;
            _listingRepository = listingRepository;
        }

        public async Task<BookingResponseDto> BookStayAsync(int guestId, BookStayRequestDto request)
        {
            if (request.FromDate.Date >= request.ToDate.Date)
            {
                throw new ArgumentException("FromDate must be earlier than ToDate.");
            }

            var listing = await _listingRepository.GetByIdAsync(request.ListingId);
            if (listing == null || !listing.IsActive)
            {
                throw new KeyNotFoundException("Listing not found.");
            }

            if (request.PeopleNames == null || request.PeopleNames.Count == 0)
            {
                throw new ArgumentException("At least one guest name must be provided.");
            }

            if (request.PeopleNames.Count > listing.NoOfPeople)
            {
                throw new InvalidOperationException("Listing capacity is not enough for the requested people count.");
            }

            var hasOverlap = await _bookingRepository.ExistsOverlappingBookingAsync(
                request.ListingId,
                request.FromDate,
                request.ToDate);

            if (hasOverlap)
            {
                throw new InvalidOperationException("This listing is already booked for the selected dates.");
            }

            var booking = new Booking
            {
                ListingId = request.ListingId,
                GuestId = guestId,
                FromDate = request.FromDate.Date,
                ToDate = request.ToDate.Date,
                PeopleNamesJson = JsonSerializer.Serialize(request.PeopleNames),
                Status = "Confirmed",
                CreatedAt = DateTime.UtcNow
            };

            await _bookingRepository.AddAsync(booking);
            var saved = await _bookingRepository.SaveChangesAsync();

            if (!saved)
            {
                throw new Exception("Booking could not be created.");
            }

            return new BookingResponseDto
            {
                BookingId = booking.Id,
                ListingId = booking.ListingId,
                FromDate = booking.FromDate,
                ToDate = booking.ToDate,
                Status = booking.Status,
                Message = "Stay booked successfully."
            };
        }
    }
}