using System;

namespace SmartTourism360.Application.DTOs.VirtualTour
{
    public class VirtualTourDto
    {
        public Guid Id { get; set; }
        public Guid DestinationId { get; set; }
        public string DestinationName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Guid? ThumbnailId { get; set; }
        public string? ThumbnailUrl { get; set; }
        public Guid? DefaultPanoramaId { get; set; }
        public string Status { get; set; } = null!;
        public int PanoramasCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
