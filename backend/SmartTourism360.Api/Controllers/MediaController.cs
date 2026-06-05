using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Media;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class MediaController : BaseApiController
    {
        private readonly IMediaService _mediaService;

        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }

        [HttpPost("/api/admin/media/upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload(
            IFormFile file, 
            [FromForm] string? mediaType = null, 
            [FromForm] string? caption = null, 
            [FromForm] string? altText = null)
        {
            if (file == null || file.Length == 0)
            {
                return Failure("Tập tin tải lên không được trống.");
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out var userId))
            {
                return StatusCode(401, ApiResponse<object>.FailureResponse("Phiên đăng nhập không hợp lệ hoặc đã hết hạn."));
            }

            using (var stream = file.OpenReadStream())
            {
                var result = await _mediaService.UploadAsync(stream, file.FileName, file.ContentType, mediaType, caption, altText, userId);
                if (!result.Success)
                {
                    return Failure(result.Message, result.Errors, 400);
                }

                return Success(result.Data, result.Message, 201);
            }
        }

        [HttpGet("/api/admin/media")]
        public async Task<IActionResult> GetMediaList(
            [FromQuery] string? mediaType,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 12)
        {
            if (page < 1) page = 1;
            if (pageSize < 1) pageSize = 12;
            if (pageSize > 100) pageSize = 100;

            var result = await _mediaService.GetMediaFilesAsync(mediaType, page, pageSize);
            return Success(result.Data, result.Message);
        }

        [HttpGet("/api/admin/media/{id:guid}")]
        public async Task<IActionResult> GetMediaDetail(Guid id)
        {
            var result = await _mediaService.GetByIdAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }

            return Success(result.Data, result.Message);
        }

        [HttpPut("/api/admin/media/{id:guid}")]
        public async Task<IActionResult> UpdateMedia(Guid id, [FromBody] UpdateMediaRequest request)
        {
            var result = await _mediaService.UpdateAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }

            return Success(result.Data, result.Message);
        }

        [HttpDelete("/api/admin/media/{id:guid}")]
        public async Task<IActionResult> DeleteMedia(Guid id)
        {
            var result = await _mediaService.DeleteAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }

            return Success(result.Data, result.Message);
        }
    }
}
