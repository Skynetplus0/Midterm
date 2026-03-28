using Microsoft.EntityFrameworkCore;
using StayBooking.AdminApi.Data;
using StayBooking.AdminApi.DTOs.Common;
using StayBooking.AdminApi.DTOs.Reports;
using StayBooking.AdminApi.Services.Interfaces;

namespace StayBooking.AdminApi.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResponseDto<ListingReportItemDto>> GetListingsReportAsync(ListingsReportFilterDto filter)
        {
            var query = _context.Listings
                .Include(x => x.Reviews)
                .Where(x => x.IsActive)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.Country))
            {
                query = query.Where(x => x.Country.ToLower() == filter.Country.ToLower());
            }

            if (!string.IsNullOrWhiteSpace(filter.City))
            {
                query = query.Where(x => x.City.ToLower() == filter.City.ToLower());
            }

            var projectedQuery = query
                .Select(x => new ListingReportItemDto
                {
                    ListingId = x.Id,
                    Title = x.Title,
                    Country = x.Country,
                    City = x.City,
                    Price = x.Price,
                    AverageRating = x.Reviews.Any()
                        ? Math.Round((decimal)x.Reviews.Average(r => r.Rating), 2)
                        : 0,
                    ReviewCount = x.Reviews.Count
                })
                .AsQueryable();

            if (filter.MinRating.HasValue)
            {
                projectedQuery = projectedQuery.Where(x => x.AverageRating >= filter.MinRating.Value);
            }

            if (filter.MaxRating.HasValue)
            {
                projectedQuery = projectedQuery.Where(x => x.AverageRating <= filter.MaxRating.Value);
            }

            var totalCount = await projectedQuery.CountAsync();

            var items = await projectedQuery
                .OrderByDescending(x => x.AverageRating)
                .ThenBy(x => x.ListingId)
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return new PagedResponseDto<ListingReportItemDto>
            {
                Items = items,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize)
            };
        }
    }
}