using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm.DTOs.Common;
using Midterm.DTOs.Reports;
using Midterm.Services.Interfaces;









namespace Midterm.Controllers
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