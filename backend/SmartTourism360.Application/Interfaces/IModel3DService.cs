using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Model3D;

namespace SmartTourism360.Application.Interfaces
{
    public interface IModel3DService
    {
        Task<ApiResponse<List<Model3DDetailDto>>> GetPublicModelsByDestinationAsync(Guid destinationId);
        Task<ApiResponse<List<Model3DDetailDto>>> GetPublicModelsByPanoramaAsync(Guid panoramaId);
        Task<ApiResponse<Model3DDetailDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<PagedResult<Model3DListItemDto>>> GetAdminModelsAsync(
            string? keyword, string? targetType, Guid? targetId, string? status, int page, int pageSize);
        Task<ApiResponse<Model3DDetailDto>> CreateModelAsync(CreateModel3DRequest request);
        Task<ApiResponse<Model3DDetailDto>> UpdateModelAsync(Guid id, UpdateModel3DRequest request);
        Task<ApiResponse<Model3DDetailDto>> UpdateModelStatusAsync(Guid id, UpdateModel3DStatusRequest request);
        Task<ApiResponse<bool>> DeleteModelAsync(Guid id);
    }
}
