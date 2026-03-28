using System.ComponentModel.DataAnnotations;



namespace Midterm.DTOs.Reviews
{
    public class CreateReviewRequestDto
    {
        [Required]
        public int BookingId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }
    }
}