using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.DTOs.Region;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    public class RegionsController : BaseApiController
    {
        private readonly IRegionService _regionService;

        public RegionsController(IRegionService regionService)
        {
            _regionService = regionService;
        }

        // --- Public Endpoints ---

        [HttpGet("/api/regions/current")]
        public async Task<IActionResult> GetCurrent()
        {
            var result = await _regionService.GetCurrentRegionAsync();
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }

        // --- Admin Endpoints ---

        [Authorize]
        [HttpGet("/api/admin/regions")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var result = await _regionService.GetAllAdminRegionsAsync();
            return Success(result.Data, result.Message);
        }

        [Authorize]
        [HttpGet("/api/admin/regions/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _regionService.GetByIdAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }

        [Authorize]
        [HttpPost("/api/admin/regions")]
        public async Task<IActionResult> Create([FromBody] CreateRegionRequest request)
        {
            var result = await _regionService.CreateAsync(request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message, 201);
        }

        [Authorize]
        [HttpPut("/api/admin/regions/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRegionRequest request)
        {
            var result = await _regionService.UpdateAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [Authorize]
        [HttpDelete("/api/admin/regions/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _regionService.DeleteAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }
    }
}
