namespace StayBooking.AdminApi.DTOs.Reports
{
    public class ListingReportItemDto
    {
        public int ListingId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal AverageRating { get; set; }
        public int ReviewCount { get; set; }
    }
}