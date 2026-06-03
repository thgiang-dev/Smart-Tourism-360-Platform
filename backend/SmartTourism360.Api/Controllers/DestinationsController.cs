using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.DTOs.Destination;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    public class DestinationsController : BaseApiController
    {
        private readonly IDestinationService _destinationService;

        public DestinationsController(IDestinationService destinationService)
        {
            _destinationService = destinationService;
        }

        // --- Public Endpoints ---

        [HttpGet("/api/destinations")]
        public async Task<IActionResult> GetPublicList(
            [FromQuery] string? keyword,
            [FromQuery] Guid? regionId,
            [FromQuery] Guid? categoryId,
            [FromQuery] bool? hasVirtualTour,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var result = await _destinationService.GetPublicDestinationsAsync(
                keyword, regionId, categoryId, hasVirtualTour, page, pageSize);
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/destinations/map")]
        public async Task<IActionResult> GetPublicMapList(
            [FromQuery] string? keyword,
            [FromQuery] Guid? regionId,
            [FromQuery] Guid? categoryId,
            [FromQuery] bool? hasVirtualTour)
        {
            var result = await _destinationService.GetPublicMapDestinationsAsync(
                keyword, regionId, categoryId, hasVirtualTour);
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/destinations/{id}")]
        public async Task<IActionResult> GetPublicDetail(string id)
        {
            var result = await _destinationService.GetPublicDetailAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/destinations/{destinationId:guid}/virtual-tour")]
        public async Task<IActionResult> GetPublicVirtualTour(Guid destinationId, [FromServices] IVirtualTourService virtualTourService)
        {
            var result = await virtualTourService.GetPublicVirtualTourAsync(destinationId);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }

        // --- Admin Endpoints ---

        [Authorize]
        [HttpGet("/api/admin/destinations")]
        public async Task<IActionResult> GetAdminList(
            [FromQuery] string? keyword,
            [FromQuery] Guid? regionId,
            [FromQuery] Guid? categoryId,
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var result = await _destinationService.GetAdminDestinationsAsync(
                keyword, regionId, categoryId, status, page, pageSize);
            return Success(result.Data, result.Message);
        }

        [Authorize]
        [HttpGet("/api/admin/destinations/{id:guid}")]
        public async Task<IActionResult> GetAdminDetail(Guid id)
        {
            var result = await _destinationService.GetAdminDetailAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }

        [Authorize]
        [HttpPost("/api/admin/destinations")]
        public async Task<IActionResult> Create([FromBody] CreateDestinationRequest request)
        {
            var result = await _destinationService.CreateAsync(request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message, 201);
        }

        [Authorize]
        [HttpPut("/api/admin/destinations/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateDestinationRequest request)
        {
            var result = await _destinationService.UpdateAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [Authorize]
        [HttpPatch("/api/admin/destinations/{id:guid}/location")]
        public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] UpdateDestinationLocationRequest request)
        {
            var result = await _destinationService.UpdateLocationAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [Authorize]
        [HttpPatch("/api/admin/destinations/{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateDestinationStatusRequest request)
        {
            var result = await _destinationService.UpdateStatusAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [Authorize]
        [HttpDelete("/api/admin/destinations/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _destinationService.DeleteAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }
    }
}
