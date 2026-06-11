using System;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class Model3D : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid MediaId { get; set; }
        public Guid? ThumbnailId { get; set; }
        public string TargetType { get; set; } = null!; // destination, panorama, hotspot
        public Guid TargetId { get; set; }
        public string? Format { get; set; } // glb, gltf
        public int? PolygonCount { get; set; }
        public string Status { get; set; } = "draft"; // draft/published/archived

        // Navigation properties
        public MediaFile Media { get; set; } = null!;
        public MediaFile? Thumbnail { get; set; }
    }
}
