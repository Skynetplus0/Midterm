using Midterm.DTOs.Bookings;



namespace Midterm.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingResponseDto> BookStayAsync(int guestId, BookStayRequestDto request);
    }
}