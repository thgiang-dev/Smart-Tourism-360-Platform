using System;
using System.ComponentModel.DataAnnotations;

namespace SmartTourism360.Application.DTOs.Analytics
{
    public class TrackEventRequest
    {
        [Required]
        [MaxLength(100)]
        public string EventName { get; set; } = null!;

        [MaxLength(50)]
        public string? TargetType { get; set; }

        public Guid? TargetId { get; set; }

        [MaxLength(150)]
        public string? SessionId { get; set; }

        public string? Metadata { get; set; } // stored as jsonb string, checked to be <= 5KB
    }
}
