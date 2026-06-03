using Microsoft.AspNetCore.Mvc;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet("status")]
        public IActionResult GetStatus()
        {
            return Ok(new
            {
                Status = "Healthy",
                Message = "Smart Tourism 360 Platform API is running successfully.",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
