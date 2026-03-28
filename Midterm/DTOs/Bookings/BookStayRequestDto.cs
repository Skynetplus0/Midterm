using System.ComponentModel.DataAnnotations;



namespace Midterm.DTOs.Bookings
{
    public class BookStayRequestDto
    {
        [Required]
        public int ListingId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        [MinLength(1)]
        public List<string> PeopleNames { get; set; } = new();
    }
}