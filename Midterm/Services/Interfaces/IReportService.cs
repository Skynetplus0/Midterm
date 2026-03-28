using Midterm.DTOs.Common;
using Midterm.DTOs.Reports;



namespace Midterm.Services.Interfaces
{
    public interface IReportService
    {
        Task<PagedResponseDto<ListingReportItemDto>> GetListingsReportAsync(ListingsReportFilterDto filter);
    }
}