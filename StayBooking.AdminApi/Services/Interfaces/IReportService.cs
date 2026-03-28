using StayBooking.AdminApi.DTOs.Common;
using StayBooking.AdminApi.DTOs.Reports;

namespace StayBooking.AdminApi.Services.Interfaces
{
    public interface IReportService
    {
        Task<PagedResponseDto<ListingReportItemDto>> GetListingsReportAsync(ListingsReportFilterDto filter);
    }
}