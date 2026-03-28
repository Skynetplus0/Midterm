using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Midterm.DTOs.Common;
using Midterm.DTOs.Listings;
using Midterm.Services.Interfaces;


namespace Midterm.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/guests/listings")]
    public class GuestListingsController : ControllerBase
    {
        private readonly IListingService _listingService;

        public GuestListingsController(IListingService listingService)
        {
            _listingService = listingService;
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(PagedResponseDto<ListingResponseDto>), StatusCodes.Status200OK)]
        public async Task<IActionResult> QueryListings([FromQuery] QueryListingsRequestDto request)
        {
            var clientKey = Request.Headers["X-Client-Id"].FirstOrDefault();

            if (string.IsNullOrWhiteSpace(clientKey))
            {
                clientKey = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
            }

            var response = await _listingService.QueryListingsAsync(request, clientKey);
            return Ok(response);
        }
    }
}