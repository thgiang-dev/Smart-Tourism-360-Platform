using System;

namespace SmartTourism360.Application.DTOs.Analytics
{
    public class TopTourAnalyticsDto
    {
        public Guid TourId { get; set; }
        public string TourTitle { get; set; } = null!;
        public int OpenCount { get; set; }
        public int PanoramaNavigationCount { get; set; }
    }
}
