using System;
using System.Collections.Generic;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class Route : BaseEntity
    {
        public Guid RegionId { get; set; }
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? EstimatedDuration { get; set; }
        public decimal? DistanceKm { get; set; }
        public string? Theme { get; set; }
        public Guid? ThumbnailId { get; set; }
        public string Status { get; set; } = "draft"; // draft/published/archived

        // Navigation properties
        public Region Region { get; set; } = null!;
        public MediaFile? Thumbnail { get; set; }
        public ICollection<RouteDestination> RouteDestinations { get; set; } = new List<RouteDestination>();
    }
}
