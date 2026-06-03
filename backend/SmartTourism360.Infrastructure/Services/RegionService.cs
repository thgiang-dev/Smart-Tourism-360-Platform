using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Region;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Domain.Entities;
using SmartTourism360.Infrastructure.Data;

namespace SmartTourism360.Infrastructure.Services
{
    public class RegionService : IRegionService
    {
        private readonly SmartTourism360DbContext _context;

        public RegionService(SmartTourism360DbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<RegionDto>> GetCurrentRegionAsync()
        {
            var region = await _context.Regions
                .Include(r => r.CoverImage)
                .FirstOrDefaultAsync(r => r.Status == "published")
                ?? await _context.Regions
                    .Include(r => r.CoverImage)
                    .FirstOrDefaultAsync();

            if (region == null)
            {
                return ApiResponse<RegionDto>.FailureResponse("Không tìm thấy khu vực nào trong hệ thống.");
            }

            return ApiResponse<RegionDto>.SuccessResponse(MapToDto(region), "Lấy khu vực hiện tại thành công.");
        }

        public async Task<ApiResponse<List<RegionDto>>> GetAllAdminRegionsAsync()
        {
            var regions = await _context.Regions
                .Include(r => r.CoverImage)
                .OrderBy(r => r.Name)
                .ToListAsync();

            var dtos = regions.Select(MapToDto).ToList();
            return ApiResponse<List<RegionDto>>.SuccessResponse(dtos, "Lấy danh sách khu vực thành công.");
        }

        public async Task<ApiResponse<RegionDto>> GetByIdAsync(Guid id)
        {
            var region = await _context.Regions
                .Include(r => r.CoverImage)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (region == null)
            {
                return ApiResponse<RegionDto>.FailureResponse("Không tìm thấy khu vực yêu cầu.");
            }

            return ApiResponse<RegionDto>.SuccessResponse(MapToDto(region), "Lấy thông tin khu vực thành công.");
        }

        public async Task<ApiResponse<RegionDto>> CreateAsync(CreateRegionRequest request)
        {
            // Kiểm tra trùng slug
            var slugExists = await _context.Regions.AnyAsync(r => r.Slug == request.Slug);
            if (slugExists)
            {
                return ApiResponse<RegionDto>.FailureResponse("Slug này đã được sử dụng bởi khu vực khác.",
                    new List<ValidationError> { new ValidationError { Field = "slug", Message = "Slug đã tồn tại." } });
            }

            var region = new Region
            {
                Name = request.Name,
                Slug = request.Slug,
                Description = request.Description,
                Province = request.Province,
                District = request.District,
                Ward = request.Ward,
                CenterLatitude = request.CenterLatitude,
                CenterLongitude = request.CenterLongitude,
                DefaultZoom = request.DefaultZoom,
                CoverImageId = request.CoverImageId,
                Status = request.Status
            };

            await _context.Regions.AddAsync(region);
            await _context.SaveChangesAsync();

            // Load cover image if exist
            if (region.CoverImageId.HasValue)
            {
                await _context.Entry(region).Reference(r => r.CoverImage).LoadAsync();
            }

            return ApiResponse<RegionDto>.SuccessResponse(MapToDto(region), "Tạo khu vực thành công.");
        }

        public async Task<ApiResponse<RegionDto>> UpdateAsync(Guid id, UpdateRegionRequest request)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return ApiResponse<RegionDto>.FailureResponse("Không tìm thấy khu vực để cập nhật.");
            }

            // Kiểm tra trùng slug với khu vực khác
            var slugExists = await _context.Regions.AnyAsync(r => r.Slug == request.Slug && r.Id != id);
            if (slugExists)
            {
                return ApiResponse<RegionDto>.FailureResponse("Slug này đã được sử dụng bởi khu vực khác.",
                    new List<ValidationError> { new ValidationError { Field = "slug", Message = "Slug đã tồn tại." } });
            }

            region.Name = request.Name;
            region.Slug = request.Slug;
            region.Description = request.Description;
            region.Province = request.Province;
            region.District = request.District;
            region.Ward = request.Ward;
            region.CenterLatitude = request.CenterLatitude;
            region.CenterLongitude = request.CenterLongitude;
            region.DefaultZoom = request.DefaultZoom;
            region.CoverImageId = request.CoverImageId;
            region.Status = request.Status;

            _context.Regions.Update(region);
            await _context.SaveChangesAsync();

            // Load cover image if exist
            if (region.CoverImageId.HasValue)
            {
                await _context.Entry(region).Reference(r => r.CoverImage).LoadAsync();
            }

            return ApiResponse<RegionDto>.SuccessResponse(MapToDto(region), "Cập nhật khu vực thành công.");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                return ApiResponse<bool>.FailureResponse("Không tìm thấy khu vực để xóa.");
            }

            // Kiểm tra ràng buộc địa điểm du lịch
            var hasDestinations = await _context.Destinations.AnyAsync(d => d.RegionId == id);
            if (hasDestinations)
            {
                // Thay vì xóa cứng, chuyển trạng thái sang archived
                region.Status = "archived";
                _context.Regions.Update(region);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Khu vực có chứa địa điểm liên kết nên đã được chuyển sang trạng thái Lưu trữ (Archived).");
            }

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Xóa khu vực thành công.");
        }

        private static RegionDto MapToDto(Region region)
        {
            return new RegionDto
            {
                Id = region.Id,
                Name = region.Name,
                Slug = region.Slug,
                Description = region.Description,
                Province = region.Province,
                District = region.District,
                Ward = region.Ward,
                CenterLatitude = region.CenterLatitude,
                CenterLongitude = region.CenterLongitude,
                DefaultZoom = region.DefaultZoom,
                CoverImageId = region.CoverImageId,
                CoverImageUrl = region.CoverImage?.FileUrl,
                Status = region.Status
            };
        }
    }
}
