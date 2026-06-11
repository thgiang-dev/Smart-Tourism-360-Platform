using System;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class AnalyticsEvent : BaseEntity
    {
        public string EventName { get; set; } = null!;
        public string? TargetType { get; set; }
        public Guid? TargetId { get; set; }
        public string? SessionId { get; set; }
        public Guid? UserId { get; set; }
        public string? Metadata { get; set; } // stored as jsonb string

        // Navigation properties
        public virtual User? User { get; set; }
    }
}
