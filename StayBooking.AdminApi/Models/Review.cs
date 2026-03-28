using System.ComponentModel.DataAnnotations;

namespace StayBooking.AdminApi.Models
{
    public class Review
    {
        public int Id { get; set; }

        [Required]
        public int BookingId { get; set; }

        [Required]
        public int ListingId { get; set; }

        [Required]
        public int GuestId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Listing? Listing { get; set; }
        public User? Guest { get; set; }
    }
}