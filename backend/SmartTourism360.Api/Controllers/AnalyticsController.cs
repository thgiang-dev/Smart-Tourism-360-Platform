using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.DTOs.Analytics;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    [Route("api/analytics")]
    public class AnalyticsController : BaseApiController
    {
        private readonly IAnalyticsService _analyticsService;

        public AnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpPost("events")]
        public async Task<IActionResult> TrackEvent([FromBody] TrackEventRequest request)
        {
            if (!ModelState.IsValid)
            {
                return Failure("Dữ liệu sự kiện không hợp lệ.");
            }

            try
            {
                Guid? userId = null;
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out var parsedId))
                {
                    userId = parsedId;
                }

                await _analyticsService.TrackEventAsync(request, userId);
                return Success<object>(null!, "Sự kiện được ghi nhận thành công.");
            }
            catch (ArgumentException ex)
            {
                return Failure(ex.Message, statusCode: 400);
            }
            catch (Exception ex)
            {
                return Failure($"Lỗi hệ thống khi ghi nhận sự kiện: {ex.Message}", statusCode: 500);
            }
        }
    }
}
