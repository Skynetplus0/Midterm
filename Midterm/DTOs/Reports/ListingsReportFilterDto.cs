using System.ComponentModel.DataAnnotations;



namespace Midterm.DTOs.Reports
{
    public class ListingsReportFilterDto
    {
        [MaxLength(100)]
        public string? Country { get; set; }

        [MaxLength(100)]
        public string? City { get; set; }

        [Range(0, 5)]
        public decimal? MinRating { get; set; }

        [Range(0, 5)]
        public decimal? MaxRating { get; set; }

        [Range(1, int.MaxValue)]
        public int PageNumber { get; set; } = 1;

        [Range(1, 100)]
        public int PageSize { get; set; } = 10;
    }
}