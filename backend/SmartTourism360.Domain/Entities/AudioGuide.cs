using System;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class AudioGuide : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Transcript { get; set; }
        public Guid MediaId { get; set; }
        public string LanguageCode { get; set; } = "vi"; // vi/en/km...
        public string TargetType { get; set; } = null!; // destination/tour/panorama/hotspot
        public Guid TargetId { get; set; }
        public int? Duration { get; set; }
        public string Status { get; set; } = "draft"; // draft/published/archived

        // Navigation properties
        public MediaFile Media { get; set; } = null!;
    }
}
