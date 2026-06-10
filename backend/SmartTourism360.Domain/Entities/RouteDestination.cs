using System;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class RouteDestination : BaseEntity
    {
        public Guid RouteId { get; set; }
        public Guid DestinationId { get; set; }
        public int DisplayOrder { get; set; }
        public string? EstimatedStayTime { get; set; }
        public string? Note { get; set; }

        // Navigation properties
        public Route Route { get; set; } = null!;
        public Destination Destination { get; set; } = null!;
    }
}
