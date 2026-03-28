using Midterm.DTOs.Listings;



using Microsoft.AspNetCore.Http;


namespace Midterm.Services.Interfaces
{
    public interface ICsvImportService
    {
        Task<CsvImportResponseDto> ImportListingsAsync(IFormFile file, int hostId);
    }
}