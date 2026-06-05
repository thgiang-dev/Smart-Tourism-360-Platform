using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.DTOs.VirtualTour;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class VirtualToursController : BaseApiController
    {
        private readonly IVirtualTourService _virtualTourService;

        public VirtualToursController(IVirtualTourService virtualTourService)
        {
            _virtualTourService = virtualTourService;
        }

        [HttpGet("/api/admin/virtual-tours")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _virtualTourService.GetAllAsync();
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/admin/destinations/{destinationId:guid}/virtual-tours")]
        public async Task<IActionResult> GetByDestinationId(Guid destinationId)
        {
            var result = await _virtualTourService.GetByDestinationIdAsync(destinationId);
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/admin/virtual-tours/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _virtualTourService.GetByIdAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPost("/api/admin/destinations/{destinationId:guid}/virtual-tours")]
        public async Task<IActionResult> Create(Guid destinationId, [FromBody] CreateVirtualTourRequest request)
        {
            var result = await _virtualTourService.CreateAsync(destinationId, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message, 201);
        }

        [HttpPut("/api/admin/virtual-tours/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateVirtualTourRequest request)
        {
            var result = await _virtualTourService.UpdateAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPatch("/api/admin/virtual-tours/{id:guid}/default-panorama")]
        public async Task<IActionResult> SetDefaultPanorama(Guid id, [FromBody] SetDefaultPanoramaRequest request)
        {
            var result = await _virtualTourService.SetDefaultPanoramaAsync(id, request.DefaultPanoramaId);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpDelete("/api/admin/virtual-tours/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _virtualTourService.DeleteAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }
    }

    public class SetDefaultPanoramaRequest
    {
        public Guid DefaultPanoramaId { get; set; }
    }
}
