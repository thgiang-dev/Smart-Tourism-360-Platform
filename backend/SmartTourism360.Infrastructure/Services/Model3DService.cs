using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Model3D;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Domain.Entities;
using SmartTourism360.Infrastructure.Data;

namespace SmartTourism360.Infrastructure.Services
{
    public class Model3DService : IModel3DService
    {
        private readonly SmartTourism360DbContext _context;

        public Model3DService(SmartTourism360DbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<Model3DDetailDto>>> GetPublicModelsByDestinationAsync(Guid destinationId)
        {
            var models = await _context.Models3D
                .Include(m => m.Media)
                .Include(m => m.Thumbnail)
                .Where(m => m.TargetType == "destination" && m.TargetId == destinationId && m.Status == "published")
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            var dest = await _context.Destinations.FindAsync(destinationId);
            var destName = dest?.Name ?? string.Empty;

            var dtos = models.Select(m => MapToDetailDto(m, destName)).ToList();
            return ApiResponse<List<Model3DDetailDto>>.SuccessResponse(dtos, "Lấy danh sách mô hình 3D thành công.");
        }

        public async Task<ApiResponse<List<Model3DDetailDto>>> GetPublicModelsByPanoramaAsync(Guid panoramaId)
        {
            var models = await _context.Models3D
                .Include(m => m.Media)
                .Include(m => m.Thumbnail)
                .Where(m => m.TargetType == "panorama" && m.TargetId == panoramaId && m.Status == "published")
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync();

            var pano = await _context.Panoramas.FindAsync(panoramaId);
            var panoTitle = pano?.Title ?? string.Empty;

            var dtos = models.Select(m => MapToDetailDto(m, panoTitle)).ToList();
            return ApiResponse<List<Model3DDetailDto>>.SuccessResponse(dtos, "Lấy danh sách mô hình 3D cho cảnh 360 thành công.");
        }

        public async Task<ApiResponse<Model3DDetailDto>> GetByIdAsync(Guid id)
        {
            var model = await _context.Models3D
                .Include(m => m.Media)
                .Include(m => m.Thumbnail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return ApiResponse<Model3DDetailDto>.FailureResponse("Không tìm thấy mô hình 3D.");
            }

            string targetName = await ResolveTargetNameAsync(model.TargetType, model.TargetId);
            var dto = MapToDetailDto(model, targetName);

            return ApiResponse<Model3DDetailDto>.SuccessResponse(dto, "Lấy chi tiết mô hình 3D thành công.");
        }

        public async Task<ApiResponse<PagedResult<Model3DListItemDto>>> GetAdminModelsAsync(
            string? keyword, string? targetType, Guid? targetId, string? status, int page, int pageSize)
        {
            var query = _context.Models3D
                .Include(m => m.Thumbnail)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var lowerKeyword = keyword.ToLower();
                query = query.Where(m => m.Title.ToLower().Contains(lowerKeyword) ||
                                         (m.Description != null && m.Description.ToLower().Contains(lowerKeyword)));
            }

            if (!string.IsNullOrWhiteSpace(targetType))
            {
                query = query.Where(m => m.TargetType == targetType);
            }

            if (targetId.HasValue)
            {
                query = query.Where(m => m.TargetId == targetId.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(m => m.Status == status);
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(m => m.UpdatedAt ?? m.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Resolve target names efficiently
            var destIds = items.Where(i => i.TargetType == "destination").Select(i => i.TargetId).Distinct().ToList();
            var destNames = await _context.Destinations
                .Where(d => destIds.Contains(d.Id))
                .ToDictionaryAsync(d => d.Id, d => d.Name);

            var hotspotIds = items.Where(i => i.TargetType == "hotspot").Select(i => i.TargetId).Distinct().ToList();
            var hotspotNames = await _context.Hotspots
                .Include(h => h.Panorama)
                .Where(h => hotspotIds.Contains(h.Id))
                .ToDictionaryAsync(h => h.Id, h => $"{h.Title} ({h.Panorama.Title})");

            var panoramaIds = items.Where(i => i.TargetType == "panorama").Select(i => i.TargetId).Distinct().ToList();
            var panoramaNames = await _context.Panoramas
                .Where(p => panoramaIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id, p => p.Title);

            var dtos = items.Select(m =>
            {
                string targetName = m.TargetType;
                if (m.TargetType == "destination" && destNames.TryGetValue(m.TargetId, out var name))
                {
                    targetName = name;
                }
                else if (m.TargetType == "hotspot" && hotspotNames.TryGetValue(m.TargetId, out var hName))
                {
                    targetName = hName;
                }
                else if (m.TargetType == "panorama" && panoramaNames.TryGetValue(m.TargetId, out var pName))
                {
                    targetName = pName;
                }
                return MapToListItemDto(m, targetName);
            }).ToList();

            var result = new PagedResult<Model3DListItemDto>
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return ApiResponse<PagedResult<Model3DListItemDto>>.SuccessResponse(result, "Lấy danh sách quản trị mô hình 3D thành công.");
        }

        public async Task<ApiResponse<Model3DDetailDto>> CreateModelAsync(CreateModel3DRequest request)
        {
            // 1. Kiểm tra MediaId có tồn tại và đúng type là model3d
            var media = await _context.MediaFiles.FindAsync(request.MediaId);
            if (media == null)
            {
                return ApiResponse<Model3DDetailDto>.FailureResponse("Tệp tin mô hình 3D không tồn tại.");
            }
            if (media.MediaType != "model3d")
            {
                return ApiResponse<Model3DDetailDto>.FailureResponse("Tệp tin liên kết không phải định dạng mô hình 3D.");
            }

            // 2. Kiểm tra ThumbnailId nếu có
            if (request.ThumbnailId.HasValue)
            {
                var thumb = await _context.MediaFiles.FindAsync(request.ThumbnailId.Value);
                if (thumb == null)
                {
                    return ApiResponse<Model3DDetailDto>.FailureResponse("Tệp tin hình ảnh đại diện không tồn tại.");
                }
            }

            // 3. Kiểm tra đối tượng liên kết
            if (request.TargetType == "destination")
            {
                var destExists = await _context.Destinations.AnyAsync(d => d.Id == request.TargetId);
                if (!destExists)
                {
                    return ApiResponse<Model3DDetailDto>.FailureResponse("Địa điểm du lịch liên kết không tồn tại.");
                }
            }
            else if (request.TargetType == "panorama")
            {
                var panoExists = await _context.Panoramas.AnyAsync(p => p.Id == request.TargetId);
                if (!panoExists)
                {
                    return ApiResponse<Model3DDetailDto>.FailureResponse("Cảnh 360 độ liên kết không tồn tại.");
                }
            }
            else if (request.TargetType == "hotspot")
            {
                var hotspotExists = await _context.Hotspots.AnyAsync(h => h.Id == request.TargetId);
                if (!hotspotExists)
                {
                    return ApiResponse<Model3DDetailDto>.FailureResponse("Hotspot liên kết không tồn tại.");
                }
            }

            var model = new Model3D
            {
                Title = request.Title,
                Description = request.Description,
                MediaId = request.MediaId,
                ThumbnailId = request.ThumbnailId,
                TargetType = request.TargetType,
                TargetId = request.TargetId,
                Format = request.Format ?? (media.Extension.Replace(".", "").ToLower()),
                PolygonCount = request.PolygonCount,
                Status = request.Status,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Models3D.AddAsync(model);
            await _context.SaveChangesAsync();

            // Load properties for mapping
            await _context.Entry(model).Reference(m => m.Media).LoadAsync();
            if (model.ThumbnailId.HasValue)
            {
                await _context.Entry(model).Reference(m => m.Thumbnail).LoadAsync();
            }

            if (model.TargetType == "destination")
            {
                await UpdateDestinationHas3dModelFlagAsync(model.TargetId);
            }

            string targetName = await ResolveTargetNameAsync(model.TargetType, model.TargetId);
            return ApiResponse<Model3DDetailDto>.SuccessResponse(MapToDetailDto(model, targetName), "Tạo mô hình 3D thành công.");
        }

        public async Task<ApiResponse<Model3DDetailDto>> UpdateModelAsync(Guid id, UpdateModel3DRequest request)
        {
            var model = await _context.Models3D
                .Include(m => m.Media)
                .Include(m => m.Thumbnail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return ApiResponse<Model3DDetailDto>.FailureResponse("Không tìm thấy mô hình 3D để cập nhật.");
            }

            // 1. Kiểm tra MediaId
            var media = await _context.MediaFiles.FindAsync(request.MediaId);
            if (media == null)
            {
                return ApiResponse<Model3DDetailDto>.FailureResponse("Tệp tin mô hình 3D không tồn tại.");
            }
            if (media.MediaType != "model3d")
            {
                return ApiResponse<Model3DDetailDto>.FailureResponse("Tệp tin liên kết không phải định dạng mô hình 3D.");
            }

            // 2. Kiểm tra ThumbnailId
            if (request.ThumbnailId.HasValue)
            {
                var thumb = await _context.MediaFiles.FindAsync(request.ThumbnailId.Value);
                if (thumb == null)
                {
                    return ApiResponse<Model3DDetailDto>.FailureResponse("Tệp tin hình ảnh đại diện không tồn tại.");
                }
            }

            // 3. Kiểm tra đối tượng liên kết
            if (request.TargetType == "destination")
            {
                var destExists = await _context.Destinations.AnyAsync(d => d.Id == request.TargetId);
                if (!destExists)
                {
                    return ApiResponse<Model3DDetailDto>.FailureResponse("Địa điểm du lịch liên kết không tồn tại.");
                }
            }
            else if (request.TargetType == "panorama")
            {
                var panoExists = await _context.Panoramas.AnyAsync(p => p.Id == request.TargetId);
                if (!panoExists)
                {
                    return ApiResponse<Model3DDetailDto>.FailureResponse("Cảnh 360 độ liên kết không tồn tại.");
                }
            }
            else if (request.TargetType == "hotspot")
            {
                var hotspotExists = await _context.Hotspots.AnyAsync(h => h.Id == request.TargetId);
                if (!hotspotExists)
                {
                    return ApiResponse<Model3DDetailDto>.FailureResponse("Hotspot liên kết không tồn tại.");
                }
            }

            var oldTargetId = model.TargetId;
            var oldTargetType = model.TargetType;

            model.Title = request.Title;
            model.Description = request.Description;
            model.MediaId = request.MediaId;
            model.ThumbnailId = request.ThumbnailId;
            model.TargetType = request.TargetType;
            model.TargetId = request.TargetId;
            model.Format = request.Format ?? (media.Extension.Replace(".", "").ToLower());
            model.PolygonCount = request.PolygonCount;
            model.Status = request.Status;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Models3D.Update(model);
            await _context.SaveChangesAsync();

            // Sync flags for destinations
            if (oldTargetType == "destination")
            {
                await UpdateDestinationHas3dModelFlagAsync(oldTargetId);
            }
            if (model.TargetType == "destination" && model.TargetId != oldTargetId)
            {
                await UpdateDestinationHas3dModelFlagAsync(model.TargetId);
            }

            await _context.Entry(model).Reference(m => m.Media).LoadAsync();
            if (model.ThumbnailId.HasValue)
            {
                await _context.Entry(model).Reference(m => m.Thumbnail).LoadAsync();
            }

            string targetName = await ResolveTargetNameAsync(model.TargetType, model.TargetId);
            return ApiResponse<Model3DDetailDto>.SuccessResponse(MapToDetailDto(model, targetName), "Cập nhật mô hình 3D thành công.");
        }

        public async Task<ApiResponse<Model3DDetailDto>> UpdateModelStatusAsync(Guid id, UpdateModel3DStatusRequest request)
        {
            var model = await _context.Models3D
                .Include(m => m.Media)
                .Include(m => m.Thumbnail)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (model == null)
            {
                return ApiResponse<Model3DDetailDto>.FailureResponse("Không tìm thấy mô hình 3D.");
            }

            model.Status = request.Status;
            model.UpdatedAt = DateTime.UtcNow;

            _context.Models3D.Update(model);
            await _context.SaveChangesAsync();

            if (model.TargetType == "destination")
            {
                await UpdateDestinationHas3dModelFlagAsync(model.TargetId);
            }

            string targetName = await ResolveTargetNameAsync(model.TargetType, model.TargetId);
            return ApiResponse<Model3DDetailDto>.SuccessResponse(MapToDetailDto(model, targetName), "Cập nhật trạng thái mô hình 3D thành công.");
        }

        public async Task<ApiResponse<bool>> DeleteModelAsync(Guid id)
        {
            var model = await _context.Models3D.FindAsync(id);
            if (model == null)
            {
                return ApiResponse<bool>.FailureResponse("Không tìm thấy mô hình 3D để xóa.");
            }

            var targetId = model.TargetId;
            var targetType = model.TargetType;

            _context.Models3D.Remove(model);
            await _context.SaveChangesAsync();

            if (targetType == "destination")
            {
                await UpdateDestinationHas3dModelFlagAsync(targetId);
            }

            return ApiResponse<bool>.SuccessResponse(true, "Xóa mô hình 3D thành công.");
        }

        // --- Helper Helpers ---

        private async Task UpdateDestinationHas3dModelFlagAsync(Guid destinationId)
        {
            var destination = await _context.Destinations.FindAsync(destinationId);
            if (destination != null)
            {
                var hasPublishedModel = await _context.Models3D
                    .AnyAsync(m => m.TargetType == "destination" && m.TargetId == destinationId && m.Status == "published");
                destination.Has3dModel = hasPublishedModel;
                _context.Destinations.Update(destination);
                await _context.SaveChangesAsync();
            }
        }

        private async Task<string> ResolveTargetNameAsync(string targetType, Guid targetId)
        {
            if (targetType == "destination")
            {
                var dest = await _context.Destinations.FindAsync(targetId);
                return dest?.Name ?? string.Empty;
            }
            if (targetType == "hotspot")
            {
                var hotspot = await _context.Hotspots
                    .Include(h => h.Panorama)
                    .FirstOrDefaultAsync(h => h.Id == targetId);
                return hotspot != null ? $"{hotspot.Title} ({hotspot.Panorama?.Title ?? "N/A"})" : "Hotspot không tồn tại";
            }
            if (targetType == "panorama")
            {
                var pano = await _context.Panoramas.FindAsync(targetId);
                return pano != null ? pano.Title : "Panorama không tồn tại";
            }
            return targetType;
        }

        private static Model3DListItemDto MapToListItemDto(Model3D m, string targetName)
        {
            return new Model3DListItemDto
            {
                Id = m.Id,
                Title = m.Title,
                Format = m.Format,
                ThumbnailUrl = m.Thumbnail?.FileUrl,
                TargetType = m.TargetType,
                TargetId = m.TargetId,
                TargetName = targetName,
                Status = m.Status,
                CreatedAt = m.CreatedAt
            };
        }

        private static Model3DDetailDto MapToDetailDto(Model3D m, string targetName)
        {
            return new Model3DDetailDto
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                MediaId = m.MediaId,
                ModelUrl = m.Media?.FileUrl ?? string.Empty,
                ThumbnailId = m.ThumbnailId,
                ThumbnailUrl = m.Thumbnail?.FileUrl,
                TargetType = m.TargetType,
                TargetId = m.TargetId,
                TargetName = targetName,
                Format = m.Format,
                PolygonCount = m.PolygonCount,
                Status = m.Status,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            };
        }
    }
}
