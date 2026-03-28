using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Midterm.DTOs.Reviews;
using Midterm.Services.Interfaces;
using System.Security.Claims;



namespace Midterm.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/guests/reviews")]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [Authorize(Roles = "Guest")]
        [HttpPost]
        [ProducesResponseType(typeof(ReviewResponseDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewRequestDto request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return Unauthorized();
            }

            var guestId = int.Parse(userIdClaim);

            var response = await _reviewService.CreateReviewAsync(guestId, request);
            return Ok(response);
        }
    }
}