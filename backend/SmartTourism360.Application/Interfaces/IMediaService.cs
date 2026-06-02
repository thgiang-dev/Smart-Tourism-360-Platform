using System;
using System.IO;
using System.Threading.Tasks;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Media;

namespace SmartTourism360.Application.Interfaces
{
    public interface IMediaService
    {
        Task<ApiResponse<MediaFileDto>> UploadAsync(Stream fileStream, string fileName, string contentType, string? mediaType, string? caption, string? altText, Guid userId);
        Task<ApiResponse<PagedResult<MediaFileDto>>> GetMediaFilesAsync(string? mediaType, int page, int pageSize);
        Task<ApiResponse<MediaFileDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<MediaFileDto>> UpdateAsync(Guid id, UpdateMediaRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
