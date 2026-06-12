using System;

namespace SmartTourism360.Application.DTOs.Destination
{
    public class DestinationMapDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public CategoryInfoDto Category { get; set; } = null!;
        public string? CoverImageUrl { get; set; }
        public string? RegionName { get; set; }
        public bool HasVirtualTour { get; set; }
    }
}
