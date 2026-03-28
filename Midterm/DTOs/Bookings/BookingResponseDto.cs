

namespace Midterm.DTOs.Bookings
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public int ListingId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}