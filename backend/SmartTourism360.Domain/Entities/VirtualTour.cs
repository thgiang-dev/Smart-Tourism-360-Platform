using System;
using System.Collections.Generic;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class VirtualTour : BaseEntity
    {
        public Guid DestinationId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? ThumbnailId { get; set; }
        public Guid? DefaultPanoramaId { get; set; }
        public string Status { get; set; } = "draft"; // draft/published/archived

        // Navigation properties
        public Destination Destination { get; set; } = null!;
        public MediaFile? Thumbnail { get; set; }
        public Panorama? DefaultPanorama { get; set; }
        public ICollection<Panorama> Panoramas { get; set; } = new List<Panorama>();
    }
}
