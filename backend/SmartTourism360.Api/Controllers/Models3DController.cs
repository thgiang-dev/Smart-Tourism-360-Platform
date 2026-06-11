using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    public class Models3DController : BaseApiController
    {
        private readonly IModel3DService _modelService;

        public Models3DController(IModel3DService modelService)
        {
            _modelService = modelService;
        }

        [HttpGet("/api/destinations/{destinationId:guid}/models-3d")]
        public async Task<IActionResult> GetModelsByDestination(Guid destinationId)
        {
            var result = await _modelService.GetPublicModelsByDestinationAsync(destinationId);
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/panoramas/{panoramaId:guid}/models-3d")]
        public async Task<IActionResult> GetModelsByPanorama(Guid panoramaId)
        {
            var result = await _modelService.GetPublicModelsByPanoramaAsync(panoramaId);
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/models-3d/{id:guid}")]
        public async Task<IActionResult> GetDetail(Guid id)
        {
            var result = await _modelService.GetByIdAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }
    }
}
