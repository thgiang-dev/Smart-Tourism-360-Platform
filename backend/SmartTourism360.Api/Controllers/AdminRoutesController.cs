using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.DTOs.Route;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class AdminRoutesController : BaseApiController
    {
        private readonly IRouteService _routeService;

        public AdminRoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet("/api/admin/routes")]
        public async Task<IActionResult> GetAdminList(
            [FromQuery] Guid? regionId,
            [FromQuery] string? theme,
            [FromQuery] string? status,
            [FromQuery] string? keyword,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var result = await _routeService.GetAdminRoutesAsync(regionId, theme, status, keyword, page, pageSize);
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/admin/routes/{id:guid}")]
        public async Task<IActionResult> GetAdminDetail(Guid id)
        {
            var result = await _routeService.GetAdminRouteDetailAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPost("/api/admin/routes")]
        public async Task<IActionResult> Create([FromBody] CreateRouteRequest request)
        {
            var result = await _routeService.CreateRouteAsync(request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message, 201);
        }

        [HttpPut("/api/admin/routes/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRouteRequest request)
        {
            var result = await _routeService.UpdateRouteAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPatch("/api/admin/routes/{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateRouteStatusRequest request)
        {
            var result = await _routeService.UpdateRouteStatusAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpDelete("/api/admin/routes/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _routeService.DeleteRouteAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        // --- Route Destination Management Endpoints ---

        [HttpPost("/api/admin/routes/{routeId:guid}/destinations/bulk")]
        public async Task<IActionResult> AddDestinationsBulk(Guid routeId, [FromBody] AddRouteDestinationsBulkRequest request)
        {
            var result = await _routeService.AddDestinationsBulkToRouteAsync(routeId, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPatch("/api/admin/routes/{routeId:guid}/destinations/reorder")]
        public async Task<IActionResult> ReorderDestinations(Guid routeId, [FromBody] ReorderRouteDestinationsRequest request)
        {
            var result = await _routeService.ReorderRouteDestinationsAsync(routeId, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPut("/api/admin/routes/{routeId:guid}/destinations/{routeDestinationId:guid}")]
        public async Task<IActionResult> UpdateRouteDestination(
            Guid routeId, Guid routeDestinationId, [FromBody] UpdateRouteDestinationRequest request)
        {
            var result = await _routeService.UpdateRouteDestinationAsync(routeId, routeDestinationId, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpDelete("/api/admin/routes/{routeId:guid}/destinations/{routeDestinationId:guid}")]
        public async Task<IActionResult> RemoveDestination(Guid routeId, Guid routeDestinationId)
        {
            var result = await _routeService.RemoveDestinationFromRouteAsync(routeId, routeDestinationId);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }
    }
}
