using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Route;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Domain.Entities;
using SmartTourism360.Infrastructure.Data;

namespace SmartTourism360.Infrastructure.Services
{
    public class RouteService : IRouteService
    {
        private readonly SmartTourism360DbContext _context;

        public RouteService(SmartTourism360DbContext context)
        {
            _context = context;
        }

        // --- Public API Implementation ---

        public async Task<ApiResponse<PagedResult<RouteListDto>>> GetPublicRoutesAsync(
            Guid? regionId, string? theme, string? keyword, int page, int pageSize)
        {
            var query = _context.Routes
                .Include(r => r.Region)
                .Include(r => r.Thumbnail)
                .Include(r => r.RouteDestinations)
                .Where(r => r.Status == "published")
                .AsQueryable();

            if (regionId.HasValue)
            {
                query = query.Where(r => r.RegionId == regionId.Value);
            }

            if (!string.IsNullOrWhiteSpace(theme))
            {
                query = query.Where(r => r.Theme == theme);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var lowerKeyword = keyword.ToLower();
                query = query.Where(r => r.Title.ToLower().Contains(lowerKeyword) || 
                                         (r.Description != null && r.Description.ToLower().Contains(lowerKeyword)));
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderBy(r => r.Title)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = items.Select(MapToListDto).ToList();

            var pagedResult = new PagedResult<RouteListDto>
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return ApiResponse<PagedResult<RouteListDto>>.SuccessResponse(pagedResult, "Lấy danh sách tuyến tham quan thành công.");
        }

        public async Task<ApiResponse<RouteDetailDto>> GetPublicRouteDetailAsync(string identifier)
        {
            var isGuid = Guid.TryParse(identifier, out var id);
            var query = _context.Routes
                .Include(r => r.Region)
                .Include(r => r.Thumbnail)
                .Include(r => r.RouteDestinations)
                    .ThenInclude(rd => rd.Destination)
                        .ThenInclude(d => d.Category)
                .Include(r => r.RouteDestinations)
                    .ThenInclude(rd => rd.Destination)
                        .ThenInclude(d => d.CoverImage)
                .Where(r => r.Status == "published")
                .AsQueryable();

            var route = isGuid
                ? await query.FirstOrDefaultAsync(r => r.Id == id)
                : await query.FirstOrDefaultAsync(r => r.Slug == identifier);

            if (route == null)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Không tìm thấy tuyến tham quan yêu cầu hoặc tuyến chưa được xuất bản.");
            }

            var dto = MapToDetailDto(route);
            return ApiResponse<RouteDetailDto>.SuccessResponse(dto, "Lấy chi tiết tuyến tham quan thành công.");
        }

        // --- Admin API Implementation ---

        public async Task<ApiResponse<PagedResult<RouteListDto>>> GetAdminRoutesAsync(
            Guid? regionId, string? theme, string? status, string? keyword, int page, int pageSize)
        {
            var query = _context.Routes
                .Include(r => r.Region)
                .Include(r => r.Thumbnail)
                .Include(r => r.RouteDestinations)
                .AsQueryable();

            if (regionId.HasValue)
            {
                query = query.Where(r => r.RegionId == regionId.Value);
            }

            if (!string.IsNullOrWhiteSpace(theme))
            {
                query = query.Where(r => r.Theme == theme);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(r => r.Status == status);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var lowerKeyword = keyword.ToLower();
                query = query.Where(r => r.Title.ToLower().Contains(lowerKeyword) || 
                                         (r.Description != null && r.Description.ToLower().Contains(lowerKeyword)));
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(r => r.UpdatedAt ?? r.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = items.Select(MapToListDto).ToList();

            var pagedResult = new PagedResult<RouteListDto>
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return ApiResponse<PagedResult<RouteListDto>>.SuccessResponse(pagedResult, "Lấy danh sách quản trị tuyến tham quan thành công.");
        }

        public async Task<ApiResponse<RouteDetailDto>> GetAdminRouteDetailAsync(Guid id)
        {
            var route = await _context.Routes
                .Include(r => r.Region)
                .Include(r => r.Thumbnail)
                .Include(r => r.RouteDestinations)
                    .ThenInclude(rd => rd.Destination)
                        .ThenInclude(d => d.Category)
                .Include(r => r.RouteDestinations)
                    .ThenInclude(rd => rd.Destination)
                        .ThenInclude(d => d.CoverImage)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (route == null)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Không tìm thấy tuyến tham quan yêu cầu.");
            }

            var dto = MapToDetailDto(route);
            return ApiResponse<RouteDetailDto>.SuccessResponse(dto, "Lấy chi tiết quản trị tuyến tham quan thành công.");
        }

        public async Task<ApiResponse<RouteDetailDto>> CreateRouteAsync(CreateRouteRequest request)
        {
            // Kiểm tra region tồn tại
            var region = await _context.Regions.FindAsync(request.RegionId);
            if (region == null)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Khu vực liên kết không tồn tại.",
                    new List<ValidationError> { new ValidationError { Field = "regionId", Message = "Khu vực không tồn tại." } });
            }

            // Kiểm tra trùng slug trong region
            var slugExists = await _context.Routes.AnyAsync(r => r.Slug == request.Slug && r.RegionId == request.RegionId);
            if (slugExists)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Slug đã tồn tại trong khu vực này.",
                    new List<ValidationError> { new ValidationError { Field = "slug", Message = "Slug đã tồn tại." } });
            }

            var route = new Route
            {
                RegionId = request.RegionId,
                Title = request.Title,
                Slug = request.Slug,
                Description = request.Description,
                EstimatedDuration = request.EstimatedDuration,
                DistanceKm = request.DistanceKm,
                Theme = request.Theme,
                ThumbnailId = request.ThumbnailId,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Routes.AddAsync(route);
            await _context.SaveChangesAsync();

            // Load relations for mapping
            await _context.Entry(route).Reference(r => r.Region).LoadAsync();
            if (route.ThumbnailId.HasValue)
            {
                await _context.Entry(route).Reference(r => r.Thumbnail).LoadAsync();
            }

            var dto = MapToDetailDto(route);
            return ApiResponse<RouteDetailDto>.SuccessResponse(dto, "Tạo tuyến tham quan thành công.");
        }

        public async Task<ApiResponse<RouteDetailDto>> UpdateRouteAsync(Guid id, UpdateRouteRequest request)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Không tìm thấy tuyến tham quan để cập nhật.");
            }

            // Kiểm tra region tồn tại
            var region = await _context.Regions.FindAsync(request.RegionId);
            if (region == null)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Khu vực liên kết không tồn tại.",
                    new List<ValidationError> { new ValidationError { Field = "regionId", Message = "Khu vực không tồn tại." } });
            }

            // Kiểm tra trùng slug trong region
            var slugExists = await _context.Routes.AnyAsync(r => r.Slug == request.Slug && r.RegionId == request.RegionId && r.Id != id);
            if (slugExists)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Slug đã tồn tại trong khu vực này.",
                    new List<ValidationError> { new ValidationError { Field = "slug", Message = "Slug đã tồn tại." } });
            }

            // Nếu muốn publish, kiểm tra điều kiện xuất bản
            if (request.Status == "published" && route.Status != "published")
            {
                var destinationCount = await _context.RouteDestinations.CountAsync(rd => rd.RouteId == id);
                if (destinationCount < 2)
                {
                    return ApiResponse<RouteDetailDto>.FailureResponse("Tuyến tham quan phải có ít nhất 2 địa điểm trước khi xuất bản.");
                }
            }

            route.RegionId = request.RegionId;
            route.Title = request.Title;
            route.Slug = request.Slug;
            route.Description = request.Description;
            route.EstimatedDuration = request.EstimatedDuration;
            route.DistanceKm = request.DistanceKm;
            route.Theme = request.Theme;
            route.ThumbnailId = request.ThumbnailId;
            route.Status = request.Status;
            route.UpdatedAt = DateTime.UtcNow;

            _context.Routes.Update(route);
            await _context.SaveChangesAsync();

            // Load relations for mapping
            await _context.Entry(route).Reference(r => r.Region).LoadAsync();
            if (route.ThumbnailId.HasValue)
            {
                await _context.Entry(route).Reference(r => r.Thumbnail).LoadAsync();
            }
            await _context.Entry(route).Collection(r => r.RouteDestinations).Query()
                .Include(rd => rd.Destination).ThenInclude(d => d.Category)
                .Include(rd => rd.Destination).ThenInclude(d => d.CoverImage)
                .LoadAsync();

            var dto = MapToDetailDto(route);
            return ApiResponse<RouteDetailDto>.SuccessResponse(dto, "Cập nhật tuyến tham quan thành công.");
        }

        public async Task<ApiResponse<RouteDetailDto>> UpdateRouteStatusAsync(Guid id, UpdateRouteStatusRequest request)
        {
            var route = await _context.Routes
                .Include(r => r.Region)
                .Include(r => r.Thumbnail)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (route == null)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Không tìm thấy tuyến tham quan.");
            }

            if (request.Status == "published")
            {
                var destinations = await _context.RouteDestinations
                    .Where(rd => rd.RouteId == id)
                    .ToListAsync();

                if (destinations.Count < 2)
                {
                    return ApiResponse<RouteDetailDto>.FailureResponse("Tuyến tham quan phải có ít nhất 2 địa điểm trước khi xuất bản.");
                }

                // Kiểm tra trùng displayOrder
                var hasDuplicates = destinations.GroupBy(rd => rd.DisplayOrder).Any(g => g.Count() > 1);
                if (hasDuplicates)
                {
                    return ApiResponse<RouteDetailDto>.FailureResponse("Thứ tự hiển thị các địa điểm trong tuyến không được trùng nhau.");
                }
            }

            route.Status = request.Status;
            route.UpdatedAt = DateTime.UtcNow;

            _context.Routes.Update(route);
            await _context.SaveChangesAsync();

            await _context.Entry(route).Collection(r => r.RouteDestinations).Query()
                .Include(rd => rd.Destination).ThenInclude(d => d.Category)
                .Include(rd => rd.Destination).ThenInclude(d => d.CoverImage)
                .LoadAsync();

            var dto = MapToDetailDto(route);
            return ApiResponse<RouteDetailDto>.SuccessResponse(dto, "Cập nhật trạng thái tuyến tham quan thành công.");
        }

        public async Task<ApiResponse<bool>> DeleteRouteAsync(Guid id)
        {
            var route = await _context.Routes.FindAsync(id);
            if (route == null)
            {
                return ApiResponse<bool>.FailureResponse("Không tìm thấy tuyến tham quan để xóa.");
            }

            // Archive thay vì xóa cứng
            route.Status = "archived";
            route.UpdatedAt = DateTime.UtcNow;
            _context.Routes.Update(route);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Lưu trữ tuyến tham quan thành công.");
        }

        // --- Route Destination Management Implementation ---

        public async Task<ApiResponse<RouteDetailDto>> AddDestinationToRouteAsync(Guid routeId, AddRouteDestinationRequest request)
        {
            var route = await _context.Routes.FindAsync(routeId);
            if (route == null)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Tuyến tham quan không tồn tại.");
            }

            var destination = await _context.Destinations.FindAsync(request.DestinationId);
            if (destination == null)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Địa điểm thêm vào không tồn tại.");
            }

            // Kiểm tra thuộc cùng region
            if (destination.RegionId != route.RegionId)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Địa điểm và tuyến tham quan phải thuộc cùng một khu vực.");
            }

            // Kiểm tra đã tồn tại địa điểm này trong tuyến chưa
            var exists = await _context.RouteDestinations.AnyAsync(rd => rd.RouteId == routeId && rd.DestinationId == request.DestinationId);
            if (exists)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Địa điểm này đã tồn tại trong tuyến tham quan.");
            }

            // Kiểm tra trùng thứ tự hiển thị
            var orderExists = await _context.RouteDestinations.AnyAsync(rd => rd.RouteId == routeId && rd.DisplayOrder == request.DisplayOrder);
            if (orderExists)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse($"Thứ tự hiển thị {request.DisplayOrder} đã được sử dụng bởi địa điểm khác trong tuyến.");
            }

            var routeDest = new RouteDestination
            {
                RouteId = routeId,
                DestinationId = request.DestinationId,
                DisplayOrder = request.DisplayOrder,
                EstimatedStayTime = request.EstimatedStayTime,
                Note = request.Note,
                CreatedAt = DateTime.UtcNow
            };

            await _context.RouteDestinations.AddAsync(routeDest);
            
            route.UpdatedAt = DateTime.UtcNow;
            _context.Routes.Update(route);

            await _context.SaveChangesAsync();

            return await GetAdminRouteDetailAsync(routeId);
        }

        public async Task<ApiResponse<RouteDetailDto>> AddDestinationsBulkToRouteAsync(Guid routeId, AddRouteDestinationsBulkRequest request)
        {
            var route = await _context.Routes.FindAsync(routeId);
            if (route == null)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Tuyến tham quan không tồn tại.");
            }

            var destIds = request.Items.Select(i => i.DestinationId).ToList();
            var destinations = await _context.Destinations.Where(d => destIds.Contains(d.Id)).ToListAsync();

            if (destinations.Count != destIds.Distinct().Count())
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Một số địa điểm yêu cầu không tồn tại.");
            }

            // Kiểm tra thuộc cùng region
            if (destinations.Any(d => d.RegionId != route.RegionId))
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Tất cả địa điểm thêm vào phải thuộc cùng khu vực với tuyến.");
            }

            // Kiểm tra trùng lặp trong tuyến hiện có
            var existingDestIds = await _context.RouteDestinations
                .Where(rd => rd.RouteId == routeId)
                .Select(rd => rd.DestinationId)
                .ToListAsync();

            if (destIds.Intersect(existingDestIds).Any())
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Một hoặc nhiều địa điểm đã tồn tại trong tuyến tham quan.");
            }

            // Kiểm tra trùng thứ tự hiển thị trong yêu cầu
            if (request.Items.GroupBy(i => i.DisplayOrder).Any(g => g.Count() > 1))
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Thứ tự hiển thị các địa điểm thêm mới không được trùng nhau.");
            }

            // Kiểm tra trùng thứ tự hiển thị với cơ sở dữ liệu
            var existingOrders = await _context.RouteDestinations
                .Where(rd => rd.RouteId == routeId)
                .Select(rd => rd.DisplayOrder)
                .ToListAsync();

            var requestOrders = request.Items.Select(i => i.DisplayOrder).ToList();
            if (requestOrders.Intersect(existingOrders).Any())
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Một hoặc nhiều thứ tự hiển thị mới đã được sử dụng trong tuyến.");
            }

            foreach (var item in request.Items)
            {
                var routeDest = new RouteDestination
                {
                    RouteId = routeId,
                    DestinationId = item.DestinationId,
                    DisplayOrder = item.DisplayOrder,
                    EstimatedStayTime = item.EstimatedStayTime,
                    Note = item.Note,
                    CreatedAt = DateTime.UtcNow
                };
                await _context.RouteDestinations.AddAsync(routeDest);
            }

            route.UpdatedAt = DateTime.UtcNow;
            _context.Routes.Update(route);

            await _context.SaveChangesAsync();

            return await GetAdminRouteDetailAsync(routeId);
        }

        public async Task<ApiResponse<RouteDetailDto>> UpdateRouteDestinationAsync(
            Guid routeId, Guid routeDestinationId, UpdateRouteDestinationRequest request)
        {
            var route = await _context.Routes.FindAsync(routeId);
            if (route == null)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Tuyến tham quan không tồn tại.");
            }

            var routeDest = await _context.RouteDestinations.FindAsync(routeDestinationId);
            if (routeDest == null || routeDest.RouteId != routeId)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Địa điểm trong tuyến không tồn tại.");
            }

            // Kiểm tra trùng thứ tự hiển thị với điểm khác
            var orderExists = await _context.RouteDestinations.AnyAsync(rd => rd.RouteId == routeId && 
                                                                               rd.DisplayOrder == request.DisplayOrder && 
                                                                               rd.Id != routeDestinationId);
            if (orderExists)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse($"Thứ tự hiển thị {request.DisplayOrder} đã được sử dụng bởi địa điểm khác.");
            }

            routeDest.DisplayOrder = request.DisplayOrder;
            routeDest.EstimatedStayTime = request.EstimatedStayTime;
            routeDest.Note = request.Note;
            routeDest.UpdatedAt = DateTime.UtcNow;

            _context.RouteDestinations.Update(routeDest);

            route.UpdatedAt = DateTime.UtcNow;
            _context.Routes.Update(route);

            await _context.SaveChangesAsync();

            return await GetAdminRouteDetailAsync(routeId);
        }

        public async Task<ApiResponse<RouteDetailDto>> ReorderRouteDestinationsAsync(Guid routeId, ReorderRouteDestinationsRequest request)
        {
            var route = await _context.Routes.FindAsync(routeId);
            if (route == null)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Tuyến tham quan không tồn tại.");
            }

            var rdIds = request.Items.Select(i => i.RouteDestinationId).ToList();
            var routeDests = await _context.RouteDestinations
                .Where(rd => rd.RouteId == routeId && rdIds.Contains(rd.Id))
                .ToListAsync();

            if (routeDests.Count != rdIds.Count)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Danh sách địa điểm sắp xếp không khớp với dữ liệu tuyến.");
            }

            // Kiểm tra trùng thứ tự hiển thị trong request
            if (request.Items.GroupBy(i => i.DisplayOrder).Any(g => g.Count() > 1))
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Thứ tự hiển thị không được trùng nhau.");
            }

            try
            {
                foreach (var item in request.Items)
                {
                    var rd = routeDests.First(r => r.Id == item.RouteDestinationId);
                    rd.DisplayOrder = item.DisplayOrder;
                    rd.UpdatedAt = DateTime.UtcNow;
                    _context.RouteDestinations.Update(rd);
                }

                route.UpdatedAt = DateTime.UtcNow;
                _context.Routes.Update(route);

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse($"Có lỗi xảy ra khi sắp xếp lại: {ex.Message}");
            }

            return await GetAdminRouteDetailAsync(routeId);
        }

        public async Task<ApiResponse<RouteDetailDto>> RemoveDestinationFromRouteAsync(Guid routeId, Guid routeDestinationId)
        {
            var route = await _context.Routes.FindAsync(routeId);
            if (route == null)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Tuyến tham quan không tồn tại.");
            }

            var routeDest = await _context.RouteDestinations.FindAsync(routeDestinationId);
            if (routeDest == null || routeDest.RouteId != routeId)
            {
                return ApiResponse<RouteDetailDto>.FailureResponse("Địa điểm liên kết không tồn tại trong tuyến này.");
            }

            _context.RouteDestinations.Remove(routeDest);
            await _context.SaveChangesAsync();

            // Re-order các điểm còn lại để consecutiveness (1, 2, 3...)
            var remainingDests = await _context.RouteDestinations
                .Where(rd => rd.RouteId == routeId)
                .OrderBy(rd => rd.DisplayOrder)
                .ToListAsync();

            for (int i = 0; i < remainingDests.Count; i++)
            {
                remainingDests[i].DisplayOrder = i + 1;
                remainingDests[i].UpdatedAt = DateTime.UtcNow;
                _context.RouteDestinations.Update(remainingDests[i]);
            }

            // Nếu số lượng còn lại < 2 và tuyến đang published, hạ xuống draft
            if (remainingDests.Count < 2 && route.Status == "published")
            {
                route.Status = "draft";
            }

            route.UpdatedAt = DateTime.UtcNow;
            _context.Routes.Update(route);
            await _context.SaveChangesAsync();

            return await GetAdminRouteDetailAsync(routeId);
        }

        // --- Helper Mapping Methods ---

        private static RouteListDto MapToListDto(Route r)
        {
            return new RouteListDto
            {
                Id = r.Id,
                RegionId = r.RegionId,
                RegionName = r.Region?.Name ?? string.Empty,
                Title = r.Title,
                Slug = r.Slug,
                Description = r.Description,
                EstimatedDuration = r.EstimatedDuration,
                DistanceKm = r.DistanceKm,
                Theme = r.Theme,
                ThumbnailUrl = r.Thumbnail?.FileUrl,
                DestinationCount = r.RouteDestinations.Count,
                Status = r.Status,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            };
        }

        private static RouteDetailDto MapToDetailDto(Route r)
        {
            var destinations = r.RouteDestinations
                .OrderBy(rd => rd.DisplayOrder)
                .Select(rd => new RouteDestinationDto
                {
                    Id = rd.Id,
                    DestinationId = rd.DestinationId,
                    Name = rd.Destination?.Name ?? string.Empty,
                    Slug = rd.Destination?.Slug ?? string.Empty,
                    ShortDescription = rd.Destination?.ShortDescription ?? string.Empty,
                    CoverImageUrl = rd.Destination?.CoverImage?.FileUrl,
                    CategoryName = rd.Destination?.Category?.Name ?? string.Empty,
                    Latitude = rd.Destination?.Latitude ?? 0,
                    Longitude = rd.Destination?.Longitude ?? 0,
                    DisplayOrder = rd.DisplayOrder,
                    EstimatedStayTime = rd.EstimatedStayTime,
                    Note = rd.Note,
                    HasVirtualTour = rd.Destination?.HasVirtualTour ?? false
                })
                .ToList();

            return new RouteDetailDto
            {
                Id = r.Id,
                RegionId = r.RegionId,
                RegionName = r.Region?.Name ?? string.Empty,
                Title = r.Title,
                Slug = r.Slug,
                Description = r.Description,
                EstimatedDuration = r.EstimatedDuration,
                DistanceKm = r.DistanceKm,
                Theme = r.Theme,
                ThumbnailId = r.ThumbnailId,
                ThumbnailUrl = r.Thumbnail?.FileUrl,
                Status = r.Status,
                Destinations = destinations
            };
        }
    }
}
