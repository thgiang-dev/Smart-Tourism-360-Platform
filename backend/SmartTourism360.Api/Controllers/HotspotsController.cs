using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.DTOs.Hotspot;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class HotspotsController : BaseApiController
    {
        private readonly IHotspotService _hotspotService;

        public HotspotsController(IHotspotService hotspotService)
        {
            _hotspotService = hotspotService;
        }

        [HttpGet("/api/admin/panoramas/{panoramaId:guid}/hotspots")]
        public async Task<IActionResult> GetByPanoramaId(Guid panoramaId)
        {
            var result = await _hotspotService.GetByPanoramaIdAsync(panoramaId);
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/admin/hotspots/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _hotspotService.GetByIdAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPost("/api/admin/panoramas/{panoramaId:guid}/hotspots")]
        public async Task<IActionResult> Create(Guid panoramaId, [FromBody] CreateHotspotRequest request)
        {
            var result = await _hotspotService.CreateAsync(panoramaId, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message, 201);
        }

        [HttpPut("/api/admin/hotspots/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateHotspotRequest request)
        {
            var result = await _hotspotService.UpdateAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPatch("/api/admin/hotspots/{id:guid}/position")]
        public async Task<IActionResult> UpdatePosition(Guid id, [FromBody] UpdateHotspotPositionRequest request)
        {
            var result = await _hotspotService.UpdatePositionAsync(id, request.Yaw, request.Pitch);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPatch("/api/admin/hotspots/{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateHotspotStatusRequest request)
        {
            var result = await _hotspotService.UpdateStatusAsync(id, request.Status);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpDelete("/api/admin/hotspots/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _hotspotService.DeleteAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }
    }

    public class UpdateHotspotPositionRequest
    {
        public decimal Yaw { get; set; }
        public decimal Pitch { get; set; }
    }

    public class UpdateHotspotStatusRequest
    {
        public string Status { get; set; } = null!;
    }
}
