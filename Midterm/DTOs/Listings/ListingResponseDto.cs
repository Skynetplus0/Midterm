
namespace Midterm.DTOs.Listings
{
    public class ListingResponseDto
    {
        public int Id { get; set; }
        public int HostId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public int NoOfPeople { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal AverageRating { get; set; }
    }
}