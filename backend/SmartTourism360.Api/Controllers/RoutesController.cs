using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    public class RoutesController : BaseApiController
    {
        private readonly IRouteService _routeService;

        public RoutesController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet("/api/routes")]
        public async Task<IActionResult> GetPublicList(
            [FromQuery] Guid? regionId,
            [FromQuery] string? theme,
            [FromQuery] string? keyword,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 10;
            if (pageSize > 100) pageSize = 100;

            var result = await _routeService.GetPublicRoutesAsync(regionId, theme, keyword, page, pageSize);
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/routes/{idOrSlug}")]
        public async Task<IActionResult> GetPublicDetail(string idOrSlug)
        {
            var result = await _routeService.GetPublicRouteDetailAsync(idOrSlug);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }
    }
}
