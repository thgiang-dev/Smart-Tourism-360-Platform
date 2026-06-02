using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Region;

namespace SmartTourism360.Application.Interfaces
{
    public interface IRegionService
    {
        Task<ApiResponse<RegionDto>> GetCurrentRegionAsync();
        Task<ApiResponse<List<RegionDto>>> GetAllAdminRegionsAsync();
        Task<ApiResponse<RegionDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<RegionDto>> CreateAsync(CreateRegionRequest request);
        Task<ApiResponse<RegionDto>> UpdateAsync(Guid id, UpdateRegionRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
