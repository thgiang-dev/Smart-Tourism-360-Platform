using System;

namespace SmartTourism360.Application.DTOs.Region
{
    public class RegionDto
    {
        public Guid Id { get; set; }
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
        public string? CoverImageUrl { get; set; }
        public string Status { get; set; } = "draft";
    }
}
