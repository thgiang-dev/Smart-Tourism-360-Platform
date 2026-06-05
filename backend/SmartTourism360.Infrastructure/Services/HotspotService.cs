using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Hotspot;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Domain.Entities;
using SmartTourism360.Infrastructure.Data;

namespace SmartTourism360.Infrastructure.Services
{
    public class HotspotService : IHotspotService
    {
        private readonly SmartTourism360DbContext _context;

        public HotspotService(SmartTourism360DbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<HotspotDto>>> GetByPanoramaIdAsync(Guid panoramaId)
        {
            var hotspots = await _context.Hotspots
                .Include(h => h.TargetPanorama)
                .Include(h => h.Media)
                .Where(h => h.PanoramaId == panoramaId)
                .OrderBy(h => h.DisplayOrder)
                .ThenBy(h => h.CreatedAt)
                .ToListAsync();

            var dtos = hotspots.Select(MapToDto).ToList();
            return ApiResponse<List<HotspotDto>>.SuccessResponse(dtos, "Lấy danh sách điểm tương tác thành công.");
        }

        public async Task<ApiResponse<HotspotDto>> GetByIdAsync(Guid id)
        {
            var h = await _context.Hotspots
                .Include(h => h.TargetPanorama)
                .Include(h => h.Media)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (h == null)
            {
                return ApiResponse<HotspotDto>.FailureResponse("Không tìm thấy điểm tương tác.");
            }

            return ApiResponse<HotspotDto>.SuccessResponse(MapToDto(h), "Lấy thông tin điểm tương tác thành công.");
        }

        public async Task<ApiResponse<HotspotDto>> CreateAsync(Guid panoramaId, CreateHotspotRequest request)
        {
            var panorama = await _context.Panoramas.FindAsync(panoramaId);
            if (panorama == null)
            {
                return ApiResponse<HotspotDto>.FailureResponse("Cảnh toàn cảnh liên kết không tồn tại.");
            }

            var type = request.Type.ToLower().Trim();
            var validateResult = await ValidateHotspotDetailsAsync(type, panorama.VirtualTourId, panoramaId, request.TargetPanoramaId, request.MediaId, request.ExternalUrl);
            if (!validateResult.Success)
            {
                return ApiResponse<HotspotDto>.FailureResponse(validateResult.Message);
            }

            var h = new Hotspot
            {
                PanoramaId = panoramaId,
                Type = type,
                Title = request.Title,
                Description = request.Description,
                Yaw = request.Yaw,
                Pitch = request.Pitch,
                TargetPanoramaId = type == "navigation" ? request.TargetPanoramaId : null,
                MediaId = (type == "audio" || type == "video" || type == "image" || type == "model3d") ? request.MediaId : null,
                ExternalUrl = type == "link" ? request.ExternalUrl : null,
                Icon = request.Icon,
                DisplayOrder = request.DisplayOrder,
                Status = "published", // Mặc định xuất bản khi tạo hotspot
                CreatedAt = DateTime.UtcNow
            };

            await _context.Hotspots.AddAsync(h);
            await _context.SaveChangesAsync();

            // Load relations for response
            if (h.TargetPanoramaId.HasValue)
            {
                await _context.Entry(h).Reference(x => x.TargetPanorama).LoadAsync();
            }
            if (h.MediaId.HasValue)
            {
                await _context.Entry(h).Reference(x => x.Media).LoadAsync();
            }

            return ApiResponse<HotspotDto>.SuccessResponse(MapToDto(h), "Tạo điểm tương tác thành công.");
        }

        public async Task<ApiResponse<HotspotDto>> UpdateAsync(Guid id, UpdateHotspotRequest request)
        {
            var h = await _context.Hotspots
                .Include(x => x.Panorama)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (h == null)
            {
                return ApiResponse<HotspotDto>.FailureResponse("Không tìm thấy điểm tương tác để cập nhật.");
            }

            var type = request.Type.ToLower().Trim();
            var validateResult = await ValidateHotspotDetailsAsync(type, h.Panorama.VirtualTourId, h.PanoramaId, request.TargetPanoramaId, request.MediaId, request.ExternalUrl);
            if (!validateResult.Success)
            {
                return ApiResponse<HotspotDto>.FailureResponse(validateResult.Message);
            }

            h.Type = type;
            h.Title = request.Title;
            h.Description = request.Description;
            h.Yaw = request.Yaw;
            h.Pitch = request.Pitch;
            h.TargetPanoramaId = type == "navigation" ? request.TargetPanoramaId : null;
            h.MediaId = (type == "audio" || type == "video" || type == "image" || type == "model3d") ? request.MediaId : null;
            h.ExternalUrl = type == "link" ? request.ExternalUrl : null;
            h.Icon = request.Icon;
            h.DisplayOrder = request.DisplayOrder;
            h.Status = request.Status;
            h.UpdatedAt = DateTime.UtcNow;

            _context.Hotspots.Update(h);
            await _context.SaveChangesAsync();

            // Reload references
            await _context.Entry(h).Reference(x => x.TargetPanorama).LoadAsync();
            await _context.Entry(h).Reference(x => x.Media).LoadAsync();

            return ApiResponse<HotspotDto>.SuccessResponse(MapToDto(h), "Cập nhật điểm tương tác thành công.");
        }

        public async Task<ApiResponse<HotspotDto>> UpdatePositionAsync(Guid id, decimal yaw, decimal pitch)
        {
            var h = await _context.Hotspots
                .Include(x => x.TargetPanorama)
                .Include(x => x.Media)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (h == null)
            {
                return ApiResponse<HotspotDto>.FailureResponse("Không tìm thấy điểm tương tác.");
            }

            h.Yaw = yaw;
            h.Pitch = pitch;
            h.UpdatedAt = DateTime.UtcNow;

            _context.Hotspots.Update(h);
            await _context.SaveChangesAsync();

            return ApiResponse<HotspotDto>.SuccessResponse(MapToDto(h), "Cập nhật vị trí thành công.");
        }

        public async Task<ApiResponse<HotspotDto>> UpdateStatusAsync(Guid id, string status)
        {
            var h = await _context.Hotspots
                .Include(x => x.TargetPanorama)
                .Include(x => x.Media)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (h == null)
            {
                return ApiResponse<HotspotDto>.FailureResponse("Không tìm thấy điểm tương tác.");
            }

            h.Status = status;
            h.UpdatedAt = DateTime.UtcNow;

            _context.Hotspots.Update(h);
            await _context.SaveChangesAsync();

            return ApiResponse<HotspotDto>.SuccessResponse(MapToDto(h), "Cập nhật trạng thái thành công.");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var h = await _context.Hotspots.FindAsync(id);
            if (h == null)
            {
                return ApiResponse<bool>.FailureResponse("Không tìm thấy điểm tương tác để xóa.");
            }

            _context.Hotspots.Remove(h);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Xóa điểm tương tác thành công.");
        }

        private async Task<(bool Success, string Message)> ValidateHotspotDetailsAsync(
            string type, 
            Guid tourId, 
            Guid panoramaId, 
            Guid? targetPanoramaId, 
            Guid? mediaId, 
            string? externalUrl)
        {
            var allowedTypes = new[] { "info", "navigation", "audio", "video", "image", "model3d", "link" };
            if (!allowedTypes.Contains(type))
            {
                return (false, "Loại điểm Hotspot không hợp lệ. Chỉ chấp nhận: info, navigation, audio, video, image, model3d, link.");
            }

            if (type == "navigation")
            {
                if (!targetPanoramaId.HasValue)
                {
                    return (false, "Cảnh đích (TargetPanoramaId) là bắt buộc đối với loại điểm chuyển cảnh.");
                }

                if (targetPanoramaId.Value == panoramaId)
                {
                    return (false, "Cảnh đích không được trùng với cảnh hiện tại.");
                }

                var targetPano = await _context.Panoramas.FindAsync(targetPanoramaId.Value);
                if (targetPano == null)
                {
                    return (false, "Cảnh đích không tồn tại trong hệ thống.");
                }

                if (targetPano.VirtualTourId != tourId)
                {
                    return (false, "Cảnh đích phải thuộc cùng một Tour ảo với cảnh hiện tại.");
                }
            }
            else if (type == "audio" || type == "video" || type == "image" || type == "model3d")
            {
                if (!mediaId.HasValue)
                {
                    return (false, "Tập tin Media (MediaId) là bắt buộc đối với loại điểm tương tác đa phương tiện.");
                }

                var mediaExists = await _context.MediaFiles.AnyAsync(m => m.Id == mediaId.Value);
                if (!mediaExists)
                {
                    return (false, "Tập tin Media được chọn không tồn tại trong hệ thống.");
                }
            }
            else if (type == "link")
            {
                if (string.IsNullOrWhiteSpace(externalUrl))
                {
                    return (false, "Đường dẫn liên kết ngoài (ExternalUrl) là bắt buộc đối với loại điểm liên kết.");
                }
            }

            return (true, string.Empty);
        }

        private static HotspotDto MapToDto(Hotspot h)
        {
            return new HotspotDto
            {
                Id = h.Id,
                PanoramaId = h.PanoramaId,
                Type = h.Type,
                Title = h.Title,
                Description = h.Description,
                Yaw = h.Yaw,
                Pitch = h.Pitch,
                TargetPanoramaId = h.TargetPanoramaId,
                TargetPanoramaTitle = h.TargetPanorama?.Title,
                MediaId = h.MediaId,
                MediaUrl = h.Media?.FileUrl,
                ExternalUrl = h.ExternalUrl,
                Icon = h.Icon,
                DisplayOrder = h.DisplayOrder,
                Status = h.Status,
                CreatedAt = h.CreatedAt
            };
        }
    }
}
