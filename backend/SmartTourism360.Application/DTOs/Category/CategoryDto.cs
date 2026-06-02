using System;

namespace SmartTourism360.Application.DTOs.Category
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int DisplayOrder { get; set; }
        public string Status { get; set; } = "active";
    }
}
