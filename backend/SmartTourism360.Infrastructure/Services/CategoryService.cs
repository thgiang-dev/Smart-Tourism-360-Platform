using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Category;
using SmartTourism360.Application.Interfaces;
using SmartTourism360.Domain.Entities;
using SmartTourism360.Infrastructure.Data;

namespace SmartTourism360.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly SmartTourism360DbContext _context;

        public CategoryService(SmartTourism360DbContext context)
        {
            _context = context;
        }

        public async Task<ApiResponse<List<CategoryDto>>> GetAllPublicCategoriesAsync()
        {
            var categories = await _context.Categories
                .Where(c => c.Status == "active")
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            var dtos = categories.Select(MapToDto).ToList();
            return ApiResponse<List<CategoryDto>>.SuccessResponse(dtos, "Lấy danh sách danh mục thành công.");
        }

        public async Task<ApiResponse<List<CategoryDto>>> GetAllAdminCategoriesAsync()
        {
            var categories = await _context.Categories
                .OrderBy(c => c.DisplayOrder)
                .ToListAsync();

            var dtos = categories.Select(MapToDto).ToList();
            return ApiResponse<List<CategoryDto>>.SuccessResponse(dtos, "Lấy danh sách danh mục quản trị thành công.");
        }

        public async Task<ApiResponse<CategoryDto>> GetByIdAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return ApiResponse<CategoryDto>.FailureResponse("Không tìm thấy danh mục yêu cầu.");
            }

            return ApiResponse<CategoryDto>.SuccessResponse(MapToDto(category), "Lấy thông tin danh mục thành công.");
        }

        public async Task<ApiResponse<CategoryDto>> CreateAsync(CreateCategoryRequest request)
        {
            // Kiểm tra trùng slug
            var slugExists = await _context.Categories.AnyAsync(c => c.Slug == request.Slug);
            if (slugExists)
            {
                return ApiResponse<CategoryDto>.FailureResponse("Slug này đã được sử dụng bởi danh mục khác.",
                    new List<ValidationError> { new ValidationError { Field = "slug", Message = "Slug đã tồn tại." } });
            }

            var category = new Category
            {
                Name = request.Name,
                Slug = request.Slug,
                Description = request.Description,
                Icon = request.Icon,
                Color = request.Color,
                DisplayOrder = request.DisplayOrder,
                Status = request.Status
            };

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return ApiResponse<CategoryDto>.SuccessResponse(MapToDto(category), "Tạo danh mục thành công.");
        }

        public async Task<ApiResponse<CategoryDto>> UpdateAsync(Guid id, UpdateCategoryRequest request)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return ApiResponse<CategoryDto>.FailureResponse("Không tìm thấy danh mục để cập nhật.");
            }

            // Kiểm tra trùng slug với danh mục khác
            var slugExists = await _context.Categories.AnyAsync(c => c.Slug == request.Slug && c.Id != id);
            if (slugExists)
            {
                return ApiResponse<CategoryDto>.FailureResponse("Slug này đã được sử dụng bởi danh mục khác.",
                    new List<ValidationError> { new ValidationError { Field = "slug", Message = "Slug đã tồn tại." } });
            }

            category.Name = request.Name;
            category.Slug = request.Slug;
            category.Description = request.Description;
            category.Icon = request.Icon;
            category.Color = request.Color;
            category.DisplayOrder = request.DisplayOrder;
            category.Status = request.Status;

            _context.Categories.Update(category);
            await _context.SaveChangesAsync();

            return ApiResponse<CategoryDto>.SuccessResponse(MapToDto(category), "Cập nhật danh mục thành công.");
        }

        public async Task<ApiResponse<bool>> DeleteAsync(Guid id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return ApiResponse<bool>.FailureResponse("Không tìm thấy danh mục để xóa.");
            }

            // Kiểm tra ràng buộc địa điểm
            var hasDestinations = await _context.Destinations.AnyAsync(d => d.CategoryId == id);
            if (hasDestinations)
            {
                // Thay vì xóa cứng, chuyển trạng thái sang inactive
                category.Status = "inactive";
                _context.Categories.Update(category);
                await _context.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Danh mục có chứa địa điểm liên kết nên đã được chuyển sang trạng thái Ngưng hoạt động (Inactive).");
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return ApiResponse<bool>.SuccessResponse(true, "Xóa danh mục thành công.");
        }

        private static CategoryDto MapToDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Slug = category.Slug,
                Description = category.Description,
                Icon = category.Icon,
                Color = category.Color,
                DisplayOrder = category.DisplayOrder,
                Status = category.Status
            };
        }
    }
}
