using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm.DTOs.Bookings;
using Midterm.Services.Interfaces;
using System.Security.Claims;



namespace Midterm.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/guests/bookings")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingsController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [Authorize(Roles = "Guest")]
        [HttpPost]
        [ProducesResponseType(typeof(BookingResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> BookStay([FromBody] BookStayRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return Unauthorized();
            }

            var guestId = int.Parse(userIdClaim);

            var response = await _bookingService.BookStayAsync(guestId, request);
            return Ok(response);
        }
    }
}