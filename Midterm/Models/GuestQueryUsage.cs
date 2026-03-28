using System.ComponentModel.DataAnnotations;


namespace Midterm.Models
{
    public class GuestQueryUsage
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string ClientKey { get; set; } = string.Empty;

        [Required]
        public DateTime QueryDate { get; set; }

        [Required]
        public int Count { get; set; } = 0;
    }
}