using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Hotspot;

namespace SmartTourism360.Application.Interfaces
{
    public interface IHotspotService
    {
        Task<ApiResponse<List<HotspotDto>>> GetByPanoramaIdAsync(Guid panoramaId);
        Task<ApiResponse<HotspotDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<HotspotDto>> CreateAsync(Guid panoramaId, CreateHotspotRequest request);
        Task<ApiResponse<HotspotDto>> UpdateAsync(Guid id, UpdateHotspotRequest request);
        Task<ApiResponse<HotspotDto>> UpdatePositionAsync(Guid id, decimal yaw, decimal pitch);
        Task<ApiResponse<HotspotDto>> UpdateStatusAsync(Guid id, string status);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
