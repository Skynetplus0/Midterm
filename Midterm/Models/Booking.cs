using Midterm.Api.Models;
using Midterm.Models;
using System.ComponentModel.DataAnnotations;


namespace Midterm.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [Required]
        public int ListingId { get; set; }

        [Required]
        public int GuestId { get; set; }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        // JSON formatında kişi isimlerini tutacağız
        [MaxLength(2000)]
        public string PeopleNamesJson { get; set; } = "[]";

        [Required]
        [MaxLength(30)]
        public string Status { get; set; } = "Confirmed";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Listing? Listing { get; set; }
        public User? Guest { get; set; }
        public Review? Review { get; set; }
    }
}