using System;

namespace SmartTourism360.Application.DTOs.Panorama
{
    public class PanoramaDto
    {
        public Guid Id { get; set; }
        public Guid VirtualTourId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid PanoramaImageId { get; set; }
        public string PanoramaImageUrl { get; set; } = null!;
        public Guid? ThumbnailId { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int DisplayOrder { get; set; }
        public decimal InitialYaw { get; set; }
        public decimal InitialPitch { get; set; }
        public decimal InitialFov { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
