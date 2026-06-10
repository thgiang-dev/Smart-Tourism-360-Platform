using System;

namespace SmartTourism360.Application.DTOs.Route
{
    public class RouteDestinationDto
    {
        public Guid Id { get; set; }
        public Guid DestinationId { get; set; }
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string ShortDescription { get; set; } = null!;
        public string? CoverImageUrl { get; set; }
        public string CategoryName { get; set; } = null!;
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int DisplayOrder { get; set; }
        public string? EstimatedStayTime { get; set; }
        public string? Note { get; set; }
        public bool HasVirtualTour { get; set; }
    }
}
