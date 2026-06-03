using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartTourism360.Application.DTOs.Category;
using SmartTourism360.Application.Interfaces;

namespace SmartTourism360.Api.Controllers
{
    [ApiController]
    public class CategoriesController : BaseApiController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // --- Public Endpoints ---

        [HttpGet("/api/categories")]
        public async Task<IActionResult> GetPublic()
        {
            var result = await _categoryService.GetAllPublicCategoriesAsync();
            return Success(result.Data, result.Message);
        }

        // --- Admin Endpoints ---

        [Authorize]
        [HttpGet("/api/admin/categories")]
        public async Task<IActionResult> GetAllAdmin()
        {
            var result = await _categoryService.GetAllAdminCategoriesAsync();
            return Success(result.Data, result.Message);
        }

        [Authorize]
        [HttpGet("/api/admin/categories/{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 404);
            }
            return Success(result.Data, result.Message);
        }

        [Authorize]
        [HttpPost("/api/admin/categories")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            var result = await _categoryService.CreateAsync(request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message, 201);
        }

        [Authorize]
        [HttpPut("/api/admin/categories/{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryRequest request)
        {
            var result = await _categoryService.UpdateAsync(id, request);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }

        [Authorize]
        [HttpDelete("/api/admin/categories/{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryService.DeleteAsync(id);
            if (!result.Success)
            {
                return Failure(result.Message, result.Errors, 400);
            }
            return Success(result.Data, result.Message);
        }
    }
}
