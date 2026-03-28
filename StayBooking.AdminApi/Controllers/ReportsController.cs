using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StayBooking.AdminApi.DTOs.Common;
using StayBooking.AdminApi.DTOs.Reports;
using StayBooking.AdminApi.Services.Interfaces;

namespace StayBooking.AdminApi.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admin/reports")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("listings")]
        [ProducesResponseType(typeof(PagedResponseDto<ListingReportItemDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetListingsReport([FromQuery] ListingsReportFilterDto filter)
        {
            var response = await _reportService.GetListingsReportAsync(filter);
            return Ok(response);
        }
    }
}