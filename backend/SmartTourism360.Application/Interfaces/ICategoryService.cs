using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartTourism360.Application.Common.Models;
using SmartTourism360.Application.DTOs.Category;

namespace SmartTourism360.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<List<CategoryDto>>> GetAllPublicCategoriesAsync();
        Task<ApiResponse<List<CategoryDto>>> GetAllAdminCategoriesAsync();
        Task<ApiResponse<CategoryDto>> GetByIdAsync(Guid id);
        Task<ApiResponse<CategoryDto>> CreateAsync(CreateCategoryRequest request);
        Task<ApiResponse<CategoryDto>> UpdateAsync(Guid id, UpdateCategoryRequest request);
        Task<ApiResponse<bool>> DeleteAsync(Guid id);
    }
}
