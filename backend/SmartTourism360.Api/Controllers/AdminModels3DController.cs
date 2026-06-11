using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.DTOs.Model3D;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class AdminModels3DController : BaseApiController
    {
        private readonly IModel3DService _modelService;

        public AdminModels3DController(IModel3DService modelService)
        {
            _modelService = modelService;
        }

        [HttpGet("/api/admin/models-3d")]
        public async Task<IActionResult> GetAdminList(
            [FromQuery] string? keyword,
            [FromQuery] string? targetType,
            [FromQuery] Guid? targetId,
            [FromQuery] string? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var result = await _modelService.GetAdminModelsAsync(keyword, targetType, targetId, status, page, pageSize);
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/admin/models-3d/{id:guid}")]
        public async Task<IActionResult> GetAdminDetail(Guid id)
        {
            var result = await _modelService.GetByIdAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPost("/api/admin/models-3d")]
        public async Task<IActionResult> Create([FromBody] CreateModel3DRequest request)
        {
            var result = await _modelService.CreateModelAsync(request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message, 201);
        }

        [HttpPut("/api/admin/models-3d/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateModel3DRequest request)
        {
            var result = await _modelService.UpdateModelAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpPatch("/api/admin/models-3d/{id:guid}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateModel3DStatusRequest request)
        {
            var result = await _modelService.UpdateModelStatusAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [HttpDelete("/api/admin/models-3d/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _modelService.DeleteModelAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }
    }
}
