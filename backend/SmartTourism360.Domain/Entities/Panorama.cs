using System;
using System.Collections.Generic;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class Panorama : BaseEntity
    {
        public Guid VirtualTourId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid PanoramaImageId { get; set; }
        public Guid? ThumbnailId { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public decimal InitialYaw { get; set; } = 0;
        public decimal InitialPitch { get; set; } = 0;
        public decimal InitialFov { get; set; } = 90;
        public string Status { get; set; } = "draft"; // draft/published/archived

        // Navigation properties
        public VirtualTour VirtualTour { get; set; } = null!;
        public MediaFile PanoramaImage { get; set; } = null!;
        public MediaFile? Thumbnail { get; set; }
        public ICollection<Hotspot> Hotspots { get; set; } = new List<Hotspot>();
    }
}
