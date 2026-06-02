using System;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class Hotspot : BaseEntity
    {
        public Guid PanoramaId { get; set; }
        public string Type { get; set; } = null!; // info/navigation/audio/video/image/model3d/link
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Yaw { get; set; }
        public decimal Pitch { get; set; }
        public Guid? TargetPanoramaId { get; set; }
        public Guid? MediaId { get; set; }
        public string? ExternalUrl { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public string Status { get; set; } = "draft"; // draft/published/archived

        // Navigation properties
        public Panorama Panorama { get; set; } = null!;
        public Panorama? TargetPanorama { get; set; }
        public MediaFile? Media { get; set; }
    }
}
