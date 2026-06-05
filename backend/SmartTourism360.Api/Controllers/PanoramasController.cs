using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.DTOs.Panorama;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class PanoramasController : BaseApiController
    {
        private readonly IPanoramaService _panoramaService;

        public PanoramasController(IPanoramaService panoramaService)
        {
            _panoramaService = panoramaService;
        }

        [HttpGet("/api/admin/virtual-tours/{tourId:guid}/panoramas")]
        public async Task<IActionResult> GetByTourId(Guid tourId)
        {
            var result = await _panoramaService.GetByTourIdAsync(tourId);
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/admin/panoramas/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _panoramaService.GetByIdAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPost("/api/admin/virtual-tours/{tourId:guid}/panoramas")]
        public async Task<IActionResult> Create(Guid tourId, [FromBody] CreatePanoramaRequest request)
        {
            var result = await _panoramaService.CreateAsync(tourId, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message, 201);
        }

        [HttpPost("/api/admin/virtual-tours/{tourId:guid}/panoramas/bulk")]
        public async Task<IActionResult> CreateBulk(Guid tourId, [FromBody] List<CreatePanoramaRequest> requests)
        {
            var result = await _panoramaService.CreateBulkAsync(tourId, requests);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message, 201);
        }

        [HttpPut("/api/admin/panoramas/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePanoramaRequest request)
        {
            var result = await _panoramaService.UpdateAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPatch("/api/admin/panoramas/{id:guid}/initial-view")]
        public async Task<IActionResult> SetInitialView(Guid id, [FromBody] SetInitialViewRequest request)
        {
            var result = await _panoramaService.SetInitialViewAsync(id, request.Yaw, request.Pitch, request.Fov);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPatch("/api/admin/virtual-tours/{tourId:guid}/panoramas/reorder")]
        public async Task<IActionResult> Reorder(Guid tourId, [FromBody] ReorderPanoramasRequest request)
        {
            var result = await _panoramaService.ReorderAsync(tourId, request.OrderedIds);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpDelete("/api/admin/panoramas/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _panoramaService.DeleteAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }
    }

    public class SetInitialViewRequest
    {
        public decimal Yaw { get; set; }
        public decimal Pitch { get; set; }
        public decimal Fov { get; set; }
    }

    public class ReorderPanoramasRequest
    {
        public List<Guid> OrderedIds { get; set; } = new();
    }
}
