using System;
using System.Collections.Generic;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class Region : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? Province { get; set; }
        public string? District { get; set; }
        public string? Ward { get; set; }
        public decimal? CenterLatitude { get; set; }
        public decimal? CenterLongitude { get; set; }
        public int? DefaultZoom { get; set; }
        public Guid? CoverImageId { get; set; }
        public string Status { get; set; } = "draft"; // draft/published/archived

        // Navigation properties
        public MediaFile? CoverImage { get; set; }
        public ICollection<Destination> Destinations { get; set; } = new List<Destination>();
    }
}
