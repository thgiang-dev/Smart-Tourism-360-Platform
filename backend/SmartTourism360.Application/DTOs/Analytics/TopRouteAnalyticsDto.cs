using System;

namespace SmartTourism360.Application.DTOs.Analytics
{
    public class TopRouteAnalyticsDto
    {
        public Guid RouteId { get; set; }
        public string RouteTitle { get; set; } = null!;
        public int ViewCount { get; set; }
        public int DestinationClickCount { get; set; }
    }
}
