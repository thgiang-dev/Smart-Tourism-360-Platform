using System;

namespace SmartTourism360.Application.DTOs.Destination
{
    public class DestinationListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public CategoryInfoDto Category { get; set; } = null!;
        public string RegionName { get; set; } = null!;
        public Guid RegionId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string? CoverImageUrl { get; set; }
        public bool HasVirtualTour { get; set; }
        public bool HasAudioGuide { get; set; }
        public string Status { get; set; } = "draft";
        public DateTime? UpdatedAt { get; set; }
    }
}
