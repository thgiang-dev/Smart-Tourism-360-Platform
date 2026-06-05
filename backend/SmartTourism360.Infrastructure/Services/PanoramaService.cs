using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Panorama;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Domain.Entities;
using SmartTourism360.Infrastructure.Data;

namespace SmartTourism360.Infrastructure.Services
{
    public class PanoramaService : IPanoramaService
    {
        private readonly SmartTourism360DbContext _context;

        public PanoramaService(SmartTourism360DbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<PanoramaDto>>> GetByTourIdAsync(Guid tourId)
        {
            var list = await _context.Panoramas
                .Include(p => p.PanoramaImage)
                .Include(p => p.Thumbnail)
                .Where(p => p.VirtualTourId == tourId)
                .OrderBy(p => p.DisplayOrder)
                .ThenBy(p => p.CreatedAt)
                .ToListAsync();

            var dtos = list.Select(MapToDto).ToList();
            return ApiResponse<List<PanoramaDto>>.SuccessResponse(dtos, "Lấy danh sách cảnh toàn cảnh thành công.");
        }

        public async Task<ApiResponse<PanoramaDto>> GetByIdAsync(Guid id)
        {
            var p = await _context.Panoramas
                .Include(p => p.PanoramaImage)
                .Include(p => p.Thumbnail)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (p == null)
            {
                return ApiResponse<PanoramaDto>.FailureResponse("Không tìm thấy cảnh yêu cầu.");
            }

            return ApiResponse<PanoramaDto>.SuccessResponse(MapToDto(p), "Lấy thông tin cảnh thành công.");
        }

        public async Task<ApiResponse<PanoramaDto>> CreateAsync(Guid tourId, CreatePanoramaRequest request)
        {
            var tourExists = await _context.VirtualTours.AnyAsync(t => t.Id == tourId);
            if (!tourExists)
            {
                return ApiResponse<PanoramaDto>.FailureResponse("Tour ảo liên kết không tồn tại.");
            }

            // Kiểm tra ảnh panorama
            var media = await _context.MediaFiles.FindAsync(request.PanoramaImageId);
            if (media == null)
            {
                return ApiResponse<PanoramaDto>.FailureResponse("Tập tin hình ảnh panorama không tồn tại trong hệ thống.");
            }
            if (media.MediaType != "panorama" && media.MediaType != "image")
            {
                return ApiResponse<PanoramaDto>.FailureResponse("Tập tin tải lên phải là hình ảnh toàn cảnh 360°.");
            }

            // Kiểm tra ThumbnailId có tồn tại
            if (request.ThumbnailId.HasValue)
            {
                var thumbExists = await _context.MediaFiles.AnyAsync(m => m.Id == request.ThumbnailId.Value);
                if (!thumbExists)
                {
                    return ApiResponse<PanoramaDto>.FailureResponse("Hình ảnh thu nhỏ (Thumbnail) không tồn tại.");
                }
            }

            var p = new Panorama
            {
                VirtualTourId = tourId,
                Title = request.Title,
                Description = request.Description,
                PanoramaImageId = request.PanoramaImageId,
                ThumbnailId = request.ThumbnailId,
                DisplayOrder = request.DisplayOrder,
                InitialYaw = request.InitialYaw,
                InitialPitch = request.InitialPitch,
                InitialFov = request.InitialFov,
                Status = "published", // Mặc định là đã xuất bản để hiển thị ở tour công khai
                CreatedAt = DateTime.UtcNow
            };

            await _context.Panoramas.AddAsync(p);
            await _context.SaveChangesAsync();

            // Load navigation properties for response mapping
            await _context.Entry(p).Reference(x => x.PanoramaImage).LoadAsync();
            if (p.ThumbnailId.HasValue)
            {
                await _context.Entry(p).Reference(x => x.Thumbnail).LoadAsync();
            }

            // Nếu đây là cảnh đầu tiên, tự động đặt làm DefaultPanoramaId cho Tour ảo
            var count = await _context.Panoramas.CountAsync(x => x.VirtualTourId == tourId);
            if (count == 1)
            {
                var tour = await _context.VirtualTours.FindAsync(tourId);
                if (tour != null)
                {
                    tour.DefaultPanoramaId = p.Id;
                    _context.VirtualTours.Update(tour);
                    await _context.SaveChangesAsync();
                }
            }

            return ApiResponse<PanoramaDto>.SuccessResponse(MapToDto(p), "Thêm cảnh toàn cảnh thành công.");
        }

        public async Task<ApiResponse<List<PanoramaDto>>> CreateBulkAsync(Guid tourId, List<CreatePanoramaRequest> requests)
        {
            var tourExists = await _context.VirtualTours.AnyAsync(t => t.Id == tourId);
            if (!tourExists)
            {
                return ApiResponse<List<PanoramaDto>>.FailureResponse("Tour ảo liên kết không tồn tại.");
            }

            var addedPanoramas = new List<Panorama>();
            foreach (var req in requests)
            {
                var media = await _context.MediaFiles.FindAsync(req.PanoramaImageId);
                if (media == null || (media.MediaType != "panorama" && media.MediaType != "image"))
                {
                    continue; // Bỏ qua tệp không hợp lệ khi tải lên hàng loạt
                }

                var p = new Panorama
                {
                    VirtualTourId = tourId,
                    Title = req.Title,
                    Description = req.Description,
                    PanoramaImageId = req.PanoramaImageId,
                    ThumbnailId = req.ThumbnailId,
                    DisplayOrder = req.DisplayOrder,
                    InitialYaw = req.InitialYaw,
                    InitialPitch = req.InitialPitch,
                    InitialFov = req.InitialFov,
                    Status = "published", // Mặc định là đã xuất bản để hiển thị ở tour công khai
                    CreatedAt = DateTime.UtcNow
                };

                await _context.Panoramas.AddAsync(p);
                addedPanoramas.Add(p);
            }

            if (addedPanoramas.Count > 0)
            {
                await _context.SaveChangesAsync();
                
                // Load navigation references
                foreach (var p in addedPanoramas)
                {
                    await _context.Entry(p).Reference(x => x.PanoramaImage).LoadAsync();
                    if (p.ThumbnailId.HasValue)
                    {
                        await _context.Entry(p).Reference(x => x.Thumbnail).LoadAsync();
                    }
                }

                // Kiểm tra đặt mặc định cho cảnh đầu tiên
                var tour = await _context.VirtualTours.FindAsync(tourId);
                if (tour != null && tour.DefaultPanoramaId == null)
                {
                    tour.DefaultPanoramaId = addedPanoramas.First().Id;
                    _context.VirtualTours.Update(tour);
                    await _context.SaveChangesAsync();
                }
            }

            var dtos = addedPanoramas.Select(MapToDto).ToList();
            return ApiResponse<List<PanoramaDto>>.SuccessResponse(dtos, $"Thêm hàng loạt {dtos.Count} cảnh thành công.");
        }

        public async Task<ApiResponse<PanoramaDto>> UpdateAsync(Guid id, UpdatePanoramaRequest request)
        {
            var p = await _context.Panoramas
                .Include(p => p.PanoramaImage)
                .Include(p => p.Thumbnail)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (p == null)
            {
                return ApiResponse<PanoramaDto>.FailureResponse("Không tìm thấy cảnh để chỉnh sửa.");
            }

            // Kiểm tra ảnh panorama
            var media = await _context.MediaFiles.FindAsync(request.PanoramaImageId);
            if (media == null || (media.MediaType != "panorama" && media.MediaType != "image"))
            {
                return ApiResponse<PanoramaDto>.FailureResponse("Tập tin hình ảnh panorama không hợp lệ.");
            }

            p.Title = request.Title;
            p.Description = request.Description;
            p.PanoramaImageId = request.PanoramaImageId;
            p.ThumbnailId = request.ThumbnailId;
            p.DisplayOrder = request.DisplayOrder;
            p.InitialYaw = request.InitialYaw;
            p.InitialPitch = request.InitialPitch;
            p.InitialFov = request.InitialFov;
            p.Status = request.Status;
            p.UpdatedAt = DateTime.UtcNow;

            _context.Panoramas.Update(p);
            await _context.SaveChangesAsync();

            return ApiResponse<PanoramaDto>.SuccessResponse(MapToDto(p), "Cập nhật thông tin cảnh thành công.");
        }

        public async Task<ApiResponse<PanoramaDto>> SetInitialViewAsync(Guid id, decimal yaw, decimal pitch, decimal fov)
        {
            var p = await _context.Panoramas
                .Include(p => p.PanoramaImage)
                .Include(p => p.Thumbnail)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (p == null)
            {
                return ApiResponse<PanoramaDto>.FailureResponse("Không tìm thấy cảnh để thiết lập góc nhìn.");
            }

            p.InitialYaw = yaw;
            p.InitialPitch = pitch;
            p.InitialFov = fov;
            p.UpdatedAt = DateTime.UtcNow;

            _context.Panoramas.Update(p);
            await _context.SaveChangesAsync();

            return ApiResponse<PanoramaDto>.SuccessResponse(MapToDto(p), "Thiết lập góc nhìn ban đầu thành công.");
        }

        public async Task<ApiResponse<bool>> ReorderAsync(Guid tourId, List<Guid> orderedIds)
        {
            var panoramas = await _context.Panoramas
                .Where(p => p.VirtualTourId == tourId)
                .ToListAsync();

            for (int i = 0; i < orderedIds.Count; i++)
            {
                var id = orderedIds[i];
                var p = panoramas.FirstOrDefault(x => x.Id == id);
                if (p != null)
                {
                    p.DisplayOrder = i;
                    _context.Panoramas.Update(p);
                }
            }

            await _context.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Sắp xếp thứ tự các cảnh thành công.");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var p = await _context.Panoramas.FindAsync(id);
            if (p == null)
            {
                return ApiResponse<bool>.FailureResponse("Không tìm thấy cảnh để xóa.");
            }

            // 1. Kiểm tra xem có đang là cảnh mặc định của Tour ảo nào hay không
            var isDefault = await _context.VirtualTours.AnyAsync(t => t.DefaultPanoramaId == id);
            if (isDefault)
            {
                return ApiResponse<bool>.FailureResponse("Không thể xóa cảnh này vì đang là cảnh mặc định của tour ảo. Vui lòng đặt cảnh mặc định khác trước.");
            }

            // 2. Kiểm tra xem có Hotspot nào đang trỏ đến cảnh này hay không
            var hasHotspotLink = await _context.Hotspots.AnyAsync(h => h.TargetPanoramaId == id);
            if (hasHotspotLink)
            {
                return ApiResponse<bool>.FailureResponse("Không thể xóa cảnh này vì đang có điểm chuyển cảnh (Hotspot) khác trỏ tới.");
            }

            _context.Panoramas.Remove(p);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Xóa cảnh toàn cảnh thành công.");
        }

        private static PanoramaDto MapToDto(Panorama p)
        {
            return new PanoramaDto
            {
                Id = p.Id,
                VirtualTourId = p.VirtualTourId,
                Title = p.Title,
                Description = p.Description,
                PanoramaImageId = p.PanoramaImageId,
                PanoramaImageUrl = p.PanoramaImage?.FileUrl ?? string.Empty,
                ThumbnailId = p.ThumbnailId,
                ThumbnailUrl = p.Thumbnail?.FileUrl,
                DisplayOrder = p.DisplayOrder,
                InitialYaw = p.InitialYaw,
                InitialPitch = p.InitialPitch,
                InitialFov = p.InitialFov,
                Status = p.Status,
                CreatedAt = p.CreatedAt
            };
        }
    }
}
