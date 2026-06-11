using System;

namespace SmartTourism360.Application.DTOs.Analytics
{
    public class TopHotspotAnalyticsDto
    {
        public Guid HotspotId { get; set; }
        public string HotspotTitle { get; set; } = null!;
        public string HotspotType { get; set; } = null!;
        public int ClickCount { get; set; }
    }
}
