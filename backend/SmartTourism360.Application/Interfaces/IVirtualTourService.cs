using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.VirtualTour;

namespace SmartTourism360.Application.Interfaces
{
    public interface IVirtualTourService
    {
        Task<ApiResponse<List<VirtualTourDto>>> GetAllAsync();
        Task<ApiResponse<List<VirtualTourDto>>> GetByDestinationIdAsync(Guid destinationId);
        Task<ApiResponse<VirtualTourDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<VirtualTourDto>> CreateAsync(Guid destinationId, CreateVirtualTourRequest request);
        Task<ApiResponse<VirtualTourDto>> UpdateAsync(Guid id, UpdateVirtualTourRequest request);
        Task<ApiResponse<VirtualTourDto>> SetDefaultPanoramaAsync(Guid id, Guid panoramaId);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
        Task<ApiResponse<PublicVirtualTourDto>> GetPublicVirtualTourAsync(Guid destinationId);
    }
}
