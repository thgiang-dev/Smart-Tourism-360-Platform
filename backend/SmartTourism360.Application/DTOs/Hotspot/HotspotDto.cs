using System;

namespace SmartTourism360.Application.DTOs.Hotspot
{
    public class HotspotDto
    {
        public Guid Id { get; set; }
        public Guid PanoramaId { get; set; }
        public string Type { get; set; } = null!; // info/navigation/audio/video/image/model3d/link
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Yaw { get; set; }
        public decimal Pitch { get; set; }
        public Guid? TargetPanoramaId { get; set; }
        public string? TargetPanoramaTitle { get; set; }
        public Guid? MediaId { get; set; }
        public string? MediaUrl { get; set; }
        public string? ExternalUrl { get; set; }
        public string? Icon { get; set; }
        public int DisplayOrder { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
