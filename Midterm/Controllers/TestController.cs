using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace Midterm.Controllers
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                message = "StayBooking API is running successfully."
            });
        }
    }
}