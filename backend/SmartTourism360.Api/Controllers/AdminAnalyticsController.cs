using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/admin/analytics")]
    public class AdminAnalyticsController : BaseApiController
    {
        private readonly IAnalyticsService _analyticsService;

        public AdminAnalyticsController(IAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] Guid? regionId)
        {
            try
            {
                var summary = await _analyticsService.GetSummaryAsync(fromDate, toDate, regionId);
                return Success(summary, "Lấy tổng quan thống kê thành công.");
            }
            catch (Exception ex)
            {
                return Failure($"Lỗi hệ thống khi lấy tổng quan thống kê: {ex.Message}", statusCode: 500);
            }
        }

        [HttpGet("top-destinations")]
        public async Task<IActionResult> GetTopDestinations(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int limit = 10)
        {
            try
            {
                var topDestinations = await _analyticsService.GetTopDestinationsAsync(fromDate, toDate, limit);
                return Success(topDestinations, "Lấy danh sách địa điểm hàng đầu thành công.");
            }
            catch (Exception ex)
            {
                return Failure($"Lỗi hệ thống khi lấy danh sách địa điểm hàng đầu: {ex.Message}", statusCode: 500);
            }
        }

        [HttpGet("top-tours")]
        public async Task<IActionResult> GetTopTours(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int limit = 10)
        {
            try
            {
                var topTours = await _analyticsService.GetTopToursAsync(fromDate, toDate, limit);
                return Success(topTours, "Lấy danh sách tour ảo hàng đầu thành công.");
            }
            catch (Exception ex)
            {
                return Failure($"Lỗi hệ thống khi lấy danh sách tour ảo hàng đầu: {ex.Message}", statusCode: 500);
            }
        }

        [HttpGet("top-hotspots")]
        public async Task<IActionResult> GetTopHotspots(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int limit = 10)
        {
            try
            {
                var topHotspots = await _analyticsService.GetTopHotspotsAsync(fromDate, toDate, limit);
                return Success(topHotspots, "Lấy danh sách hotspot hàng đầu thành công.");
            }
            catch (Exception ex)
            {
                return Failure($"Lỗi hệ thống khi lấy danh sách hotspot hàng đầu: {ex.Message}", statusCode: 500);
            }
        }

        [HttpGet("top-routes")]
        public async Task<IActionResult> GetTopRoutes(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] int limit = 10)
        {
            try
            {
                var topRoutes = await _analyticsService.GetTopRoutesAsync(fromDate, toDate, limit);
                return Success(topRoutes, "Lấy danh sách tuyến đường hàng đầu thành công.");
            }
            catch (Exception ex)
            {
                return Failure($"Lỗi hệ thống khi lấy danh sách tuyến đường hàng đầu: {ex.Message}", statusCode: 500);
            }
        }

        [HttpGet("events-by-day")]
        public async Task<IActionResult> GetEventsByDay(
            [FromQuery] DateTime? fromDate,
            [FromQuery] DateTime? toDate,
            [FromQuery] string? eventName)
        {
            try
            {
                var eventsByDay = await _analyticsService.GetEventsByDayAsync(fromDate, toDate, eventName);
                return Success(eventsByDay, "Lấy dữ liệu thống kê sự kiện theo ngày thành công.");
            }
            catch (Exception ex)
            {
                return Failure($"Lỗi hệ thống khi lấy thống kê sự kiện theo ngày: {ex.Message}", statusCode: 500);
            }
        }
    }
}
