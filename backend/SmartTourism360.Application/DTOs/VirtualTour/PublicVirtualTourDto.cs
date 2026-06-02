using System;
using System.Collections.Generic;

namespace SmartTourism360.Application.DTOs.VirtualTour
{
    public class PublicVirtualTourDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid DestinationId { get; set; }
        public Guid? DefaultPanoramaId { get; set; }
        public List<PublicPanoramaDto> Panoramas { get; set; } = new();
    }

    public class PublicPanoramaDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string PanoramaImageUrl { get; set; } = null!;
        public int DisplayOrder { get; set; }
        public decimal InitialYaw { get; set; }
        public decimal InitialPitch { get; set; }
        public decimal InitialFov { get; set; }
        public List<PublicHotspotDto> Hotspots { get; set; } = new();
    }

    public class PublicHotspotDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = null!; // info/navigation/audio/video/image/model3d/link
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Yaw { get; set; }
        public decimal Pitch { get; set; }
        public Guid? TargetPanoramaId { get; set; }
        public string? MediaUrl { get; set; }
        public string? ExternalUrl { get; set; }
        public string? Icon { get; set; }
    }
}
