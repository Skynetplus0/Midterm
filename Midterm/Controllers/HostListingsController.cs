using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm.DTOs.Common;
using Midterm.DTOs.Listings;
using Midterm.Services.Interfaces;
using System.Security.Claims;




namespace Midterm.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/hosts/listings")]
    public class HostListingsController : ControllerBase
    {
        private readonly IListingService _listingService;

        public HostListingsController(IListingService listingService)
        {
            _listingService = listingService;
        }

        [Authorize(Roles = "Host")]
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateListing([FromBody] CreateListingRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return Unauthorized();
            }

            var hostId = int.Parse(userIdClaim);

            var response = await _listingService.CreateListingAsync(hostId, request);
            return Ok(response);
        }
    }
}