using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Category
{
    public class UpdateCategoryRequest
    {
        [Required(ErrorMessage = "Tên danh mục không được để trống.")]
        [MaxLength(100, ErrorMessage = "Tên danh mục không được vượt quá 100 ký tự.")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Slug không được để trống.")]
        [MaxLength(100, ErrorMessage = "Slug không được vượt quá 100 ký tự.")]
        [RegularExpression(@"^[a-z0-9-]+$", ErrorMessage = "Slug chỉ được chứa chữ thường, số và dấu gạch ngang.")]
        public string Slug { get; set; } = null!;

        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }

        public int DisplayOrder { get; set; } = 0;

        [RegularExpression(@"^(active|inactive)$", ErrorMessage = "Trạng thái không hợp lệ.")]
        public string Status { get; set; } = "active";
    }
}
