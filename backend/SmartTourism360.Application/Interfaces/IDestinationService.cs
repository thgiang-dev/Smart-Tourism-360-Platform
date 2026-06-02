using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Destination;

namespace SmartTourism360.Application.Interfaces
{
    public interface IDestinationService
    {
        // Public API Methods
        Task<ApiResponse<PagedResult<DestinationListDto>>> GetPublicDestinationsAsync(
            string? keyword, Guid? regionId, Guid? categoryId, bool? hasVirtualTour, int page, int pageSize);

        Task<ApiResponse<List<DestinationMapDto>>> GetPublicMapDestinationsAsync(
            string? keyword, Guid? regionId, Guid? categoryId, bool? hasVirtualTour);

        Task<ApiResponse<DestinationDetailDto>> GetPublicDetailAsync(string identifier);

        // Admin API Methods
        Task<ApiResponse<PagedResult<DestinationListDto>>> GetAdminDestinationsAsync(
            string? keyword, Guid? regionId, Guid? categoryId, string? status, int page, int pageSize);

        Task<ApiResponse<DestinationDetailDto>> GetAdminDetailAsync(Guid id);
        Task<ApiResponse<DestinationDetailDto>> CreateAsync(CreateDestinationRequest request);
        Task<ApiResponse<DestinationDetailDto>> UpdateAsync(Guid id, UpdateDestinationRequest request);
        Task<ApiResponse<DestinationDetailDto>> UpdateLocationAsync(Guid id, UpdateDestinationLocationRequest request);
        Task<ApiResponse<DestinationDetailDto>> UpdateStatusAsync(Guid id, UpdateDestinationStatusRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
