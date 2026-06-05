using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.VirtualTour;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Domain.Entities;
using SmartTourism360.Infrastructure.Data;

namespace SmartTourism360.Infrastructure.Services
{
    public class VirtualTourService : IVirtualTourService
    {
        private readonly SmartTourism360DbContext _context;

        public VirtualTourService(SmartTourism360DbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<VirtualTourDto>>> GetAllAsync()
        {
            var tours = await _context.VirtualTours
                .Include(t => t.Destination)
                .Include(t => t.Thumbnail)
                .Include(t => t.Panoramas)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            var dtos = tours.Select(MapToDto).ToList();
            return ApiResponse<List<VirtualTourDto>>.SuccessResponse(dtos, "Lấy danh sách tất cả tour ảo thành công.");
        }

        public async Task<ApiResponse<List<VirtualTourDto>>> GetByDestinationIdAsync(Guid destinationId)
        {
            var tours = await _context.VirtualTours
                .Include(t => t.Destination)
                .Include(t => t.Thumbnail)
                .Include(t => t.Panoramas)
                .Where(t => t.DestinationId == destinationId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();

            var dtos = tours.Select(MapToDto).ToList();
            return ApiResponse<List<VirtualTourDto>>.SuccessResponse(dtos, "Lấy danh sách tour ảo thành công.");
        }

        public async Task<ApiResponse<VirtualTourDto>> GetByIdAsync(Guid id)
        {
            var tour = await _context.VirtualTours
                .Include(t => t.Destination)
                .Include(t => t.Thumbnail)
                .Include(t => t.Panoramas)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tour == null)
            {
                return ApiResponse<VirtualTourDto>.FailureResponse("Không tìm thấy tour ảo.");
            }

            return ApiResponse<VirtualTourDto>.SuccessResponse(MapToDto(tour), "Lấy thông tin chi tiết tour ảo thành công.");
        }

        public async Task<ApiResponse<VirtualTourDto>> CreateAsync(Guid destinationId, CreateVirtualTourRequest request)
        {
            var destination = await _context.Destinations.FindAsync(destinationId);
            if (destination == null)
            {
                return ApiResponse<VirtualTourDto>.FailureResponse("Địa điểm du lịch liên kết không tồn tại.");
            }

            // Kiểm tra xem ThumbnailId có tồn tại trong media_files
            if (request.ThumbnailId.HasValue)
            {
                var thumbnailExists = await _context.MediaFiles.AnyAsync(m => m.Id == request.ThumbnailId.Value);
                if (!thumbnailExists)
                {
                    return ApiResponse<VirtualTourDto>.FailureResponse("Hình ảnh đại diện không tồn tại trong hệ thống.");
                }
            }

            var tour = new VirtualTour
            {
                DestinationId = destinationId,
                Title = request.Title,
                Description = request.Description,
                ThumbnailId = request.ThumbnailId,
                Status = "draft", // Mặc định tạo ở bản nháp
                CreatedAt = DateTime.UtcNow
            };

            await _context.VirtualTours.AddAsync(tour);
            await _context.SaveChangesAsync();

            // Load navigation properties for response
            await _context.Entry(tour).Reference(t => t.Destination).LoadAsync();
            if (tour.ThumbnailId.HasValue)
            {
                await _context.Entry(tour).Reference(t => t.Thumbnail).LoadAsync();
            }

            return ApiResponse<VirtualTourDto>.SuccessResponse(MapToDto(tour), "Tạo tour ảo thành công.");
        }

        public async Task<ApiResponse<VirtualTourDto>> UpdateAsync(Guid id, UpdateVirtualTourRequest request)
        {
            var tour = await _context.VirtualTours
                .Include(t => t.Destination)
                .Include(t => t.Thumbnail)
                .Include(t => t.Panoramas)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tour == null)
            {
                return ApiResponse<VirtualTourDto>.FailureResponse("Không tìm thấy tour ảo để chỉnh sửa.");
            }

            // Kiểm tra xem ThumbnailId có tồn tại trong media_files
            if (request.ThumbnailId.HasValue)
            {
                var thumbnailExists = await _context.MediaFiles.AnyAsync(m => m.Id == request.ThumbnailId.Value);
                if (!thumbnailExists)
                {
                    return ApiResponse<VirtualTourDto>.FailureResponse("Hình ảnh đại diện không tồn tại trong hệ thống.");
                }
            }

            tour.Title = request.Title;
            tour.Description = request.Description;
            tour.ThumbnailId = request.ThumbnailId;
            tour.Status = request.Status;
            tour.UpdatedAt = DateTime.UtcNow;

            _context.VirtualTours.Update(tour);
            await _context.SaveChangesAsync();

            // Đồng bộ hóa cờ HasVirtualTour của Destination
            await UpdateDestinationVirtualTourFlagAsync(tour.DestinationId);

            return ApiResponse<VirtualTourDto>.SuccessResponse(MapToDto(tour), "Cập nhật thông tin tour ảo thành công.");
        }

        public async Task<ApiResponse<VirtualTourDto>> SetDefaultPanoramaAsync(Guid id, Guid panoramaId)
        {
            var tour = await _context.VirtualTours
                .Include(t => t.Destination)
                .Include(t => t.Thumbnail)
                .Include(t => t.Panoramas)
                .FirstOrDefaultAsync(t => t.Id == id);

            if (tour == null)
            {
                return ApiResponse<VirtualTourDto>.FailureResponse("Không tìm thấy tour ảo.");
            }

            // Kiểm tra xem panorama có thuộc về tour này không
            var panoramaExists = tour.Panoramas.Any(p => p.Id == panoramaId);
            if (!panoramaExists)
            {
                return ApiResponse<VirtualTourDto>.FailureResponse("Cảnh toàn cảnh không thuộc về tour ảo này.");
            }

            tour.DefaultPanoramaId = panoramaId;
            tour.UpdatedAt = DateTime.UtcNow;

            _context.VirtualTours.Update(tour);
            await _context.SaveChangesAsync();

            return ApiResponse<VirtualTourDto>.SuccessResponse(MapToDto(tour), "Đặt cảnh mặc định cho tour ảo thành công.");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var tour = await _context.VirtualTours.FindAsync(id);
            if (tour == null)
            {
                return ApiResponse<bool>.FailureResponse("Không tìm thấy tour ảo để xóa.");
            }

            var destId = tour.DestinationId;

            // Xóa tour ảo
            _context.VirtualTours.Remove(tour);
            await _context.SaveChangesAsync();

            // Đồng bộ hóa cờ HasVirtualTour của Destination
            await UpdateDestinationVirtualTourFlagAsync(destId);

            return ApiResponse<bool>.SuccessResponse(true, "Xóa tour ảo thành công.");
        }

        private async Task UpdateDestinationVirtualTourFlagAsync(Guid destinationId)
        {
            var destination = await _context.Destinations.FindAsync(destinationId);
            if (destination != null)
            {
                // Kiểm tra xem có bất kỳ tour nào đã được published cho địa điểm này hay chưa
                var hasPublishedTour = await _context.VirtualTours.AnyAsync(t => t.DestinationId == destinationId && t.Status == "published");
                destination.HasVirtualTour = hasPublishedTour;
                _context.Destinations.Update(destination);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<ApiResponse<PublicVirtualTourDto>> GetPublicVirtualTourAsync(Guid destinationId)
        {
            var tour = await _context.VirtualTours
                .Include(t => t.Panoramas.Where(p => p.Status == "published").OrderBy(p => p.DisplayOrder))
                    .ThenInclude(p => p.PanoramaImage)
                .Include(t => t.Panoramas.Where(p => p.Status == "published").OrderBy(p => p.DisplayOrder))
                    .ThenInclude(p => p.Hotspots.Where(h => h.Status == "published").OrderBy(h => h.DisplayOrder))
                        .ThenInclude(h => h.Media)
                .FirstOrDefaultAsync(t => t.DestinationId == destinationId && t.Status == "published");

            if (tour == null)
            {
                return ApiResponse<PublicVirtualTourDto>.FailureResponse("Không tìm thấy tour ảo đã xuất bản cho địa điểm này.");
            }

            var publicTourDto = new PublicVirtualTourDto
            {
                Id = tour.Id,
                Title = tour.Title,
                Description = tour.Description,
                DestinationId = tour.DestinationId,
                DefaultPanoramaId = tour.DefaultPanoramaId,
                Panoramas = tour.Panoramas.Select(p => new PublicPanoramaDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = p.Description,
                    PanoramaImageUrl = p.PanoramaImage?.FileUrl ?? string.Empty,
                    DisplayOrder = p.DisplayOrder,
                    InitialYaw = p.InitialYaw,
                    InitialPitch = p.InitialPitch,
                    InitialFov = p.InitialFov,
                    Hotspots = p.Hotspots.Select(h => new PublicHotspotDto
                    {
                        Id = h.Id,
                        Type = h.Type,
                        Title = h.Title,
                        Description = h.Description,
                        Yaw = h.Yaw,
                        Pitch = h.Pitch,
                        TargetPanoramaId = h.TargetPanoramaId,
                        MediaUrl = h.Media?.FileUrl,
                        ExternalUrl = h.ExternalUrl,
                        Icon = h.Icon
                    }).ToList()
                }).ToList()
            };

            return ApiResponse<PublicVirtualTourDto>.SuccessResponse(publicTourDto, "Lấy thông tin tour ảo thành công.");
        }

        private static VirtualTourDto MapToDto(VirtualTour t)
        {
            return new VirtualTourDto
            {
                Id = t.Id,
                DestinationId = t.DestinationId,
                DestinationName = t.Destination?.Name ?? string.Empty,
                Title = t.Title,
                Description = t.Description,
                ThumbnailId = t.ThumbnailId,
                ThumbnailUrl = t.Thumbnail?.FileUrl,
                DefaultPanoramaId = t.DefaultPanoramaId,
                Status = t.Status,
                PanoramasCount = t.Panoramas?.Count ?? 0,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            };
        }
    }
}
