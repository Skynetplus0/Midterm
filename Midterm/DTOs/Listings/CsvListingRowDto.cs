


namespace Midterm.DTOs.Listings
{
    public class CsvListingRowDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int NoOfPeople { get; set; }
        public string Country { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public decimal Price { get; set; }
    }
}