using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Destination;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Domain.Entities;
using SmartTourism360.Infrastructure.Data;

namespace SmartTourism360.Infrastructure.Services
{
    public class DestinationService : IDestinationService
    {
        private readonly SmartTourism360DbContext _context;

        public DestinationService(SmartTourism360DbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<PagedResult<DestinationListDto>>> GetPublicDestinationsAsync(
            string? keyword, Guid? regionId, Guid? categoryId, bool? hasVirtualTour, int page, int pageSize)
        {
            var query = _context.Destinations
                .Include(d => d.Category)
                .Include(d => d.Region)
                .Include(d => d.CoverImage)
                .Where(d => d.Status == "published")
                .AsQueryable();

            query = ApplyFilters(query, keyword, regionId, categoryId, hasVirtualTour, null);

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderBy(d => d.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = items.Select(MapToListDto).ToList();

            var pagedResult = new PagedResult<DestinationListDto>
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return ApiResponse<PagedResult<DestinationListDto>>.SuccessResponse(pagedResult, "Lấy danh sách địa điểm du lịch thành công.");
        }

        public async Task<ApiResponse<List<DestinationMapDto>>> GetPublicMapDestinationsAsync(
            string? keyword, Guid? regionId, Guid? categoryId, bool? hasVirtualTour)
        {
            var query = _context.Destinations
                .Include(d => d.Category)
                .Include(d => d.CoverImage)
                .Where(d => d.Status == "published")
                .AsQueryable();

            query = ApplyFilters(query, keyword, regionId, categoryId, hasVirtualTour, null);

            var items = await query
                .OrderBy(d => d.Name)
                .ToListAsync();

            var dtos = items.Select(MapToMapDto).ToList();
            return ApiResponse<List<DestinationMapDto>>.SuccessResponse(dtos, "Lấy danh sách bản đồ địa điểm du lịch thành công.");
        }

        public async Task<ApiResponse<DestinationDetailDto>> GetPublicDetailAsync(string identifier)
        {
            var isGuid = Guid.TryParse(identifier, out var id);
            var query = _context.Destinations
                .Include(d => d.Category)
                .Include(d => d.Region)
                .Include(d => d.CoverImage)
                .Include(d => d.VirtualTours)
                .AsQueryable();

            var destination = isGuid
                ? await query.FirstOrDefaultAsync(d => d.Id == id && d.Status == "published")
                : await query.FirstOrDefaultAsync(d => d.Slug == identifier && d.Status == "published");

            if (destination == null)
            {
                return ApiResponse<DestinationDetailDto>.FailureResponse("Không tìm thấy địa điểm yêu cầu hoặc địa điểm chưa được xuất bản.");
            }

            var detailDto = await MapToDetailDtoAsync(destination);
            return ApiResponse<DestinationDetailDto>.SuccessResponse(detailDto, "Lấy chi tiết địa điểm thành công.");
        }

        public async Task<ApiResponse<PagedResult<DestinationListDto>>> GetAdminDestinationsAsync(
            string? keyword, Guid? regionId, Guid? categoryId, string? status, int page, int pageSize)
        {
            var query = _context.Destinations
                .Include(d => d.Category)
                .Include(d => d.Region)
                .Include(d => d.CoverImage)
                .AsQueryable();

            query = ApplyFilters(query, keyword, regionId, categoryId, null, status);

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(d => d.UpdatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var dtos = items.Select(MapToListDto).ToList();

            var pagedResult = new PagedResult<DestinationListDto>
            {
                Items = dtos,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return ApiResponse<PagedResult<DestinationListDto>>.SuccessResponse(pagedResult, "Lấy danh sách địa điểm quản trị thành công.");
        }

        public async Task<ApiResponse<DestinationDetailDto>> GetAdminDetailAsync(Guid id)
        {
            var destination = await _context.Destinations
                .Include(d => d.Category)
                .Include(d => d.Region)
                .Include(d => d.CoverImage)
                .Include(d => d.VirtualTours)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (destination == null)
            {
                return ApiResponse<DestinationDetailDto>.FailureResponse("Không tìm thấy địa điểm yêu cầu.");
            }

            var detailDto = await MapToDetailDtoAsync(destination);
            return ApiResponse<DestinationDetailDto>.SuccessResponse(detailDto, "Lấy chi tiết địa điểm quản trị thành công.");
        }

        public async Task<ApiResponse<DestinationDetailDto>> CreateAsync(CreateDestinationRequest request)
        {
            // Kiểm tra khu vực tồn tại
            var regionExists = await _context.Regions.AnyAsync(r => r.Id == request.RegionId);
            if (!regionExists)
            {
                return ApiResponse<DestinationDetailDto>.FailureResponse("Khu vực liên kết không tồn tại.",
                    new List<ValidationError> { new ValidationError { Field = "regionId", Message = "Khu vực không tồn tại." } });
            }

            // Kiểm tra danh mục tồn tại
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId);
            if (!categoryExists)
            {
                return ApiResponse<DestinationDetailDto>.FailureResponse("Danh mục liên kết không tồn tại.",
                    new List<ValidationError> { new ValidationError { Field = "categoryId", Message = "Danh mục không tồn tại." } });
            }

            // Kiểm tra trùng slug trong cùng khu vực
            var slugExists = await _context.Destinations.AnyAsync(d => d.Slug == request.Slug && d.RegionId == request.RegionId);
            if (slugExists)
            {
                return ApiResponse<DestinationDetailDto>.FailureResponse("Slug đã tồn tại trong khu vực này.",
                    new List<ValidationError> { new ValidationError { Field = "slug", Message = "Slug đã tồn tại." } });
            }

            var destination = new Destination
            {
                RegionId = request.RegionId,
                CategoryId = request.CategoryId,
                Name = request.Name,
                Slug = request.Slug,
                ShortDescription = request.ShortDescription,
                FullDescription = request.FullDescription,
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                Location = new Point((double)request.Longitude, (double)request.Latitude) { SRID = 4326 },
                CoverImageId = request.CoverImageId,
                OpeningHoursJson = request.OpeningHours,
                TicketPrice = request.TicketPrice,
                ContactPhone = request.ContactPhone,
                ContactEmail = request.ContactEmail,
                WebsiteUrl = request.WebsiteUrl,
                FacebookUrl = request.FacebookUrl,
                Status = request.Status,
                HasVirtualTour = false,
                HasAudioGuide = false,
                Has3dModel = false
            };

            await _context.Destinations.AddAsync(destination);
            await _context.SaveChangesAsync();

            // Load properties for dto mapping
            await _context.Entry(destination).Reference(d => d.Category).LoadAsync();
            await _context.Entry(destination).Reference(d => d.Region).LoadAsync();
            if (destination.CoverImageId.HasValue)
            {
                await _context.Entry(destination).Reference(d => d.CoverImage).LoadAsync();
            }

            var detailDto = await MapToDetailDtoAsync(destination);
            return ApiResponse<DestinationDetailDto>.SuccessResponse(detailDto, "Tạo địa điểm du lịch thành công.");
        }

        public async Task<ApiResponse<DestinationDetailDto>> UpdateAsync(Guid id, UpdateDestinationRequest request)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return ApiResponse<DestinationDetailDto>.FailureResponse("Không tìm thấy địa điểm du lịch để cập nhật.");
            }

            // Kiểm tra khu vực tồn tại
            var regionExists = await _context.Regions.AnyAsync(r => r.Id == request.RegionId);
            if (!regionExists)
            {
                return ApiResponse<DestinationDetailDto>.FailureResponse("Khu vực liên kết không tồn tại.",
                    new List<ValidationError> { new ValidationError { Field = "regionId", Message = "Khu vực không tồn tại." } });
            }

            // Kiểm tra danh mục tồn tại
            var categoryExists = await _context.Categories.AnyAsync(c => c.Id == request.CategoryId);
            if (!categoryExists)
            {
                return ApiResponse<DestinationDetailDto>.FailureResponse("Danh mục liên kết không tồn tại.",
                    new List<ValidationError> { new ValidationError { Field = "categoryId", Message = "Danh mục không tồn tại." } });
            }

            // Kiểm tra trùng slug trong cùng khu vực
            var slugExists = await _context.Destinations.AnyAsync(d => d.Slug == request.Slug && d.RegionId == request.RegionId && d.Id != id);
            if (slugExists)
            {
                return ApiResponse<DestinationDetailDto>.FailureResponse("Slug đã tồn tại trong khu vực này.",
                    new List<ValidationError> { new ValidationError { Field = "slug", Message = "Slug đã tồn tại." } });
            }

            destination.RegionId = request.RegionId;
            destination.CategoryId = request.CategoryId;
            destination.Name = request.Name;
            destination.Slug = request.Slug;
            destination.ShortDescription = request.ShortDescription;
            destination.FullDescription = request.FullDescription;
            destination.Address = request.Address;
            destination.Latitude = request.Latitude;
            destination.Longitude = request.Longitude;
            destination.Location = new Point((double)request.Longitude, (double)request.Latitude) { SRID = 4326 };
            destination.CoverImageId = request.CoverImageId;
            destination.OpeningHoursJson = request.OpeningHours;
            destination.TicketPrice = request.TicketPrice;
            destination.ContactPhone = request.ContactPhone;
            destination.ContactEmail = request.ContactEmail;
            destination.WebsiteUrl = request.WebsiteUrl;
            destination.FacebookUrl = request.FacebookUrl;
            destination.Status = request.Status;

            _context.Destinations.Update(destination);
            await _context.SaveChangesAsync();

            // Load properties for dto mapping
            await _context.Entry(destination).Reference(d => d.Category).LoadAsync();
            await _context.Entry(destination).Reference(d => d.Region).LoadAsync();
            if (destination.CoverImageId.HasValue)
            {
                await _context.Entry(destination).Reference(d => d.CoverImage).LoadAsync();
            }

            var detailDto = await MapToDetailDtoAsync(destination);
            return ApiResponse<DestinationDetailDto>.SuccessResponse(detailDto, "Cập nhật địa điểm du lịch thành công.");
        }

        public async Task<ApiResponse<DestinationDetailDto>> UpdateLocationAsync(Guid id, UpdateDestinationLocationRequest request)
        {
            var destination = await _context.Destinations
                .Include(d => d.Category)
                .Include(d => d.Region)
                .Include(d => d.CoverImage)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (destination == null)
            {
                return ApiResponse<DestinationDetailDto>.FailureResponse("Không tìm thấy địa điểm du lịch để cập nhật vị trí.");
            }

            destination.Latitude = request.Latitude;
            destination.Longitude = request.Longitude;
            destination.Location = new Point((double)request.Longitude, (double)request.Latitude) { SRID = 4326 };

            _context.Destinations.Update(destination);
            await _context.SaveChangesAsync();

            var detailDto = await MapToDetailDtoAsync(destination);
            return ApiResponse<DestinationDetailDto>.SuccessResponse(detailDto, "Cập nhật tọa độ địa điểm thành công.");
        }

        public async Task<ApiResponse<DestinationDetailDto>> UpdateStatusAsync(Guid id, UpdateDestinationStatusRequest request)
        {
            var destination = await _context.Destinations
                .Include(d => d.Category)
                .Include(d => d.Region)
                .Include(d => d.CoverImage)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (destination == null)
            {
                return ApiResponse<DestinationDetailDto>.FailureResponse("Không tìm thấy địa điểm du lịch để cập nhật trạng thái.");
            }

            destination.Status = request.Status;

            _context.Destinations.Update(destination);
            await _context.SaveChangesAsync();

            var detailDto = await MapToDetailDtoAsync(destination);
            return ApiResponse<DestinationDetailDto>.SuccessResponse(detailDto, "Cập nhật trạng thái địa điểm thành công.");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var destination = await _context.Destinations.FindAsync(id);
            if (destination == null)
            {
                return ApiResponse<bool>.FailureResponse("Không tìm thấy địa điểm du lịch để xóa.");
            }

            // Thay vì xóa cứng, chuyển trạng thái sang archived trong MVP
            destination.Status = "archived";
            _context.Destinations.Update(destination);
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Lưu trữ địa điểm du lịch thành công.");
        }

        private static IQueryable<Destination> ApplyFilters(
            IQueryable<Destination> query, string? keyword, Guid? regionId, Guid? categoryId, bool? hasVirtualTour, string? status)
        {
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var lowerKeyword = keyword.ToLower();
                query = query.Where(d => d.Name.ToLower().Contains(lowerKeyword) 
                    || d.ShortDescription.ToLower().Contains(lowerKeyword) 
                    || (d.FullDescription != null && d.FullDescription.ToLower().Contains(lowerKeyword)));
            }

            if (regionId.HasValue)
            {
                query = query.Where(d => d.RegionId == regionId.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(d => d.CategoryId == categoryId.Value);
            }

            if (hasVirtualTour.HasValue)
            {
                query = query.Where(d => d.HasVirtualTour == hasVirtualTour.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(d => d.Status == status);
            }

            return query;
        }

        private static DestinationListDto MapToListDto(Destination d)
        {
            return new DestinationListDto
            {
                Id = d.Id,
                Name = d.Name,
                Slug = d.Slug,
                ShortDescription = d.ShortDescription,
                Category = new CategoryInfoDto
                {
                    Id = d.Category.Id,
                    Name = d.Category.Name,
                    Slug = d.Category.Slug,
                    Icon = d.Category.Icon,
                    Color = d.Category.Color
                },
                RegionId = d.Region.Id,
                RegionName = d.Region.Name,
                Latitude = d.Latitude,
                Longitude = d.Longitude,
                CoverImageUrl = d.CoverImage?.FileUrl,
                HasVirtualTour = d.HasVirtualTour,
                HasAudioGuide = d.HasAudioGuide,
                Status = d.Status,
                UpdatedAt = d.UpdatedAt
            };
        }

        private static DestinationMapDto MapToMapDto(Destination d)
        {
            return new DestinationMapDto
            {
                Id = d.Id,
                Name = d.Name,
                Slug = d.Slug,
                Latitude = d.Latitude,
                Longitude = d.Longitude,
                Category = new CategoryInfoDto
                {
                    Id = d.Category.Id,
                    Name = d.Category.Name,
                    Slug = d.Category.Slug,
                    Icon = d.Category.Icon,
                    Color = d.Category.Color
                },
                CoverImageUrl = d.CoverImage?.FileUrl,
                HasVirtualTour = d.HasVirtualTour
            };
        }

        private async Task<DestinationDetailDto> MapToDetailDtoAsync(Destination d)
        {
            // Query gallery and videos (polymorphic relationship using MediaFiles)
            var mediaFiles = await _context.MediaFiles
                .Where(m => m.OwnerType == "destination" && m.OwnerId == d.Id)
                .ToListAsync();

            var gallery = mediaFiles
                .Where(m => m.MediaType == "image" || m.MediaType == "gallery")
                .Select(m => new MediaFileDto { Id = m.Id, Url = m.FileUrl, Caption = m.Caption, MediaType = m.MediaType })
                .ToList();

            var videos = mediaFiles
                .Where(m => m.MediaType == "video")
                .Select(m => new MediaFileDto { Id = m.Id, Url = m.FileUrl, Caption = m.Caption, MediaType = m.MediaType })
                .ToList();

            // Query audio guides
            var audioGuidesList = await _context.AudioGuides
                .Include(a => a.Media)
                .Where(a => a.TargetType == "destination" && a.TargetId == d.Id && a.Status == "published")
                .ToListAsync();

            var audioGuides = audioGuidesList.Select(a => new AudioGuideDto
            {
                Id = a.Id,
                Title = a.Title,
                Description = a.Transcript, // Transcript matches description in specification
                AudioUrl = a.Media.FileUrl,
                Duration = a.Duration
            }).ToList();

            // Check published Virtual Tour
            var publishedTour = d.VirtualTours.FirstOrDefault(t => t.Status == "published") 
                ?? d.VirtualTours.FirstOrDefault();

            return new DestinationDetailDto
            {
                Id = d.Id,
                RegionId = d.RegionId,
                RegionName = d.Region?.Name ?? string.Empty,
                Name = d.Name,
                Slug = d.Slug,
                ShortDescription = d.ShortDescription,
                FullDescription = d.FullDescription,
                Address = d.Address,
                Latitude = d.Latitude,
                Longitude = d.Longitude,
                Category = new CategoryInfoDto
                {
                    Id = d.Category.Id,
                    Name = d.Category.Name,
                    Slug = d.Category.Slug,
                    Icon = d.Category.Icon,
                    Color = d.Category.Color
                },
                CoverImageUrl = d.CoverImage?.FileUrl,
                CoverImageId = d.CoverImageId,
                Gallery = gallery,
                Videos = videos,
                AudioGuides = audioGuides,
                HasVirtualTour = d.HasVirtualTour,
                VirtualTourId = publishedTour?.Id,
                OpeningHours = d.OpeningHoursJson,
                TicketPrice = d.TicketPrice,
                ContactPhone = d.ContactPhone,
                ContactEmail = d.ContactEmail,
                WebsiteUrl = d.WebsiteUrl,
                FacebookUrl = d.FacebookUrl,
                Status = d.Status,
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            };
        }
    }
}
