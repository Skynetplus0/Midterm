using Midterm.DTOs.Common;
using Midterm.DTOs.Listings;



namespace Midterm.Services.Interfaces
{
    public interface IListingService
    {
        Task<ApiResponseDto> CreateListingAsync(int hostId, CreateListingRequestDto request);
        Task<PagedResponseDto<ListingResponseDto>> QueryListingsAsync(QueryListingsRequestDto request, string clientKey);
    }
}