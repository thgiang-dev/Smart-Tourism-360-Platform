using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Panorama;

namespace SmartTourism360.Application.Interfaces
{
    public interface IPanoramaService
    {
        Task<ApiResponse<List<PanoramaDto>>> GetByTourIdAsync(Guid tourId);
        Task<ApiResponse<PanoramaDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<PanoramaDto>> CreateAsync(Guid tourId, CreatePanoramaRequest request);
        Task<ApiResponse<List<PanoramaDto>>> CreateBulkAsync(Guid tourId, List<CreatePanoramaRequest> requests);
        Task<ApiResponse<PanoramaDto>> UpdateAsync(Guid id, UpdatePanoramaRequest request);
        Task<ApiResponse<PanoramaDto>> SetInitialViewAsync(Guid id, decimal yaw, decimal pitch, decimal fov);
        Task<ApiResponse<bool>> ReorderAsync(Guid tourId, List<Guid> orderedIds);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
