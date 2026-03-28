

namespace Midterm.DTOs.Reviews
{
    public class ReviewResponseDto
    {
        public int ReviewId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
    }
}