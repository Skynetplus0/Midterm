using Midterm.DTOs.Common;
using Midterm.DTOs.Listings;
using Midterm.Models;
using Midterm.Repositories.Interfaces;
using Midterm.Services.Interfaces;



namespace Midterm.Services
{
    public class ListingService : IListingService
    {
        private readonly IListingRepository _listingRepository;
        private readonly IGuestQueryUsageRepository _guestQueryUsageRepository;

        public ListingService(
            IListingRepository listingRepository,
            IGuestQueryUsageRepository guestQueryUsageRepository)
        {
            _listingRepository = listingRepository;
            _guestQueryUsageRepository = guestQueryUsageRepository;
        }

        public async Task<ApiResponseDto> CreateListingAsync(int hostId, CreateListingRequestDto request)
        {
            var listing = new Listing
            {
                HostId = hostId,
                Title = request.Title,
                Description = request.Description,
                NoOfPeople = request.NoOfPeople,
                Country = request.Country,
                City = request.City,
                Price = request.Price,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _listingRepository.AddAsync(listing);
            var saved = await _listingRepository.SaveChangesAsync();

            if (!saved)
            {
                throw new Exception("Listing could not be created.");
            }

            return new ApiResponseDto
            {
                Success = true,
                Message = "Listing created successfully."
            };
        }

        public async Task<PagedResponseDto<ListingResponseDto>> QueryListingsAsync(QueryListingsRequestDto request, string clientKey)
        {
            if (request.FromDate.Date >= request.ToDate.Date)
            {
                throw new ArgumentException("FromDate must be earlier than ToDate.");
            }

            var today = DateTime.UtcNow.Date;

            var usage = await _guestQueryUsageRepository.GetTodayUsageAsync(clientKey, today);

            if (usage == null)
            {
                usage = new GuestQueryUsage
                {
                    ClientKey = clientKey,
                    QueryDate = today,
                    Count = 1
                };

                await _guestQueryUsageRepository.AddAsync(usage);
                await _guestQueryUsageRepository.SaveChangesAsync();
            }
            else
            {
                if (usage.Count >= 3)
                {
                    throw new InvalidOperationException("Daily query limit exceeded. Maximum 3 calls per day.");
                }

                usage.Count += 1;
                await _guestQueryUsageRepository.SaveChangesAsync();
            }

            var result = await _listingRepository.QueryAvailableListingsAsync(
                request.FromDate,
                request.ToDate,
                request.NoOfPeople,
                request.Country,
                request.City,
                request.PageNumber,
                request.PageSize);

            var items = result.Items.Select(x => new ListingResponseDto
            {
                Id = x.Id,
                HostId = x.HostId,
                Title = x.Title,
                Description = x.Description,
                NoOfPeople = x.NoOfPeople,
                Country = x.Country,
                City = x.City,
                Price = x.Price,
                AverageRating = x.Reviews.Any()
                    ? Math.Round((decimal)x.Reviews.Average(r => r.Rating), 2)
                    : 0
            }).ToList();

            return new PagedResponseDto<ListingResponseDto>
            {
                Items = items,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = (int)Math.Ceiling((double)result.TotalCount / request.PageSize)
            };
        }
    }
}