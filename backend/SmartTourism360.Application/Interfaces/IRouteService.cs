using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Route;

namespace SmartTourism360.Application.Interfaces
{
    public interface IRouteService
    {
        // Public API
        Task<ApiResponse<PagedResult<RouteListDto>>> GetPublicRoutesAsync(
            Guid? regionId, string? theme, string? keyword, int page, int pageSize);

        Task<ApiResponse<RouteDetailDto>> GetPublicRouteDetailAsync(string identifier);

        // Admin API
        Task<ApiResponse<PagedResult<RouteListDto>>> GetAdminRoutesAsync(
            Guid? regionId, string? theme, string? status, string? keyword, int page, int pageSize);

        Task<ApiResponse<RouteDetailDto>> GetAdminRouteDetailAsync(Guid id);
        
        Task<ApiResponse<RouteDetailDto>> CreateRouteAsync(CreateRouteRequest request);
        
        Task<ApiResponse<RouteDetailDto>> UpdateRouteAsync(Guid id, UpdateRouteRequest request);
        
        Task<ApiResponse<RouteDetailDto>> UpdateRouteStatusAsync(Guid id, UpdateRouteStatusRequest request);
        
        Task<ApiResponse<bool>> DeleteRouteAsync(Guid id);

        // Route Destination Management
        Task<ApiResponse<RouteDetailDto>> AddDestinationToRouteAsync(
            Guid routeId, AddRouteDestinationRequest request);

        Task<ApiResponse<RouteDetailDto>> AddDestinationsBulkToRouteAsync(
            Guid routeId, AddRouteDestinationsBulkRequest request);

        Task<ApiResponse<RouteDetailDto>> UpdateRouteDestinationAsync(
            Guid routeId, Guid routeDestinationId, UpdateRouteDestinationRequest request);

        Task<ApiResponse<RouteDetailDto>> ReorderRouteDestinationsAsync(
            Guid routeId, ReorderRouteDestinationsRequest request);

        Task<ApiResponse<RouteDetailDto>> RemoveDestinationFromRouteAsync(
            Guid routeId, Guid routeDestinationId);
    }
}
