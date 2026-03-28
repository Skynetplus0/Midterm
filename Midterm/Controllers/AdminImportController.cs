using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm.DTOs.Listings;
using Midterm.Services.Interfaces;




//https://localhost:7252/swagger/index.html



namespace Midterm.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/admin/listings")]
    public class AdminImportController : ControllerBase
    {
        private readonly ICsvImportService _csvImportService;

        public AdminImportController(ICsvImportService csvImportService)
        {
            _csvImportService = csvImportService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(typeof(CsvImportResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> ImportListings(/* [FromForm] */ IFormFile file,/* [FromForm]*/ int hostId)
        {
            var response = await _csvImportService.ImportListingsAsync(file, hostId);
            return Ok(response);
        }
    }
}