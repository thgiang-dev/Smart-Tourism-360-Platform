using System;

namespace SmartTourism360.Application.DTOs.Destination
{
    public class CategoryInfoDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Icon { get; set; }
        public string? Color { get; set; }
    }
}
