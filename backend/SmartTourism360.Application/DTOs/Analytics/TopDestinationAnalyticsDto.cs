using System;

namespace SmartTourism360.Application.DTOs.Analytics
{
    public class TopDestinationAnalyticsDto
    {
        public Guid DestinationId { get; set; }
        public string DestinationName { get; set; } = null!;
        public int ViewCount { get; set; }
        public int TourOpenCount { get; set; }
    }
}
