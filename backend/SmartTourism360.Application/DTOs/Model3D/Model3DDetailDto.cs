using System;

namespace SmartTourism360.Application.DTOs.Model3D
{
    public class Model3DDetailDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid MediaId { get; set; }
        public string ModelUrl { get; set; } = null!;
        public Guid? ThumbnailId { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string TargetType { get; set; } = null!;
        public Guid TargetId { get; set; }
        public string? TargetName { get; set; }
        public string? Format { get; set; }
        public int? PolygonCount { get; set; }
        public string Status { get; set; } = "draft";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
