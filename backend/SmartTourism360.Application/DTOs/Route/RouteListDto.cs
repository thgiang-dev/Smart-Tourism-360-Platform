using System;

namespace SmartTourism360.Application.DTOs.Route
{
    public class RouteListDto
    {
        public Guid Id { get; set; }
        public Guid RegionId { get; set; }
        public string RegionName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? EstimatedDuration { get; set; }
        public decimal? DistanceKm { get; set; }
        public string? Theme { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int DestinationCount { get; set; }
        public string Status { get; set; } = "draft";
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
