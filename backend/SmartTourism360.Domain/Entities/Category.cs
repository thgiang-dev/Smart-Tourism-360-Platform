using System.Collections.Generic;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Slug { get; set; } = null!;
        public string? Description { get; set; }
        public string? Icon { get; set; }
        public string? Color { get; set; }
        public int DisplayOrder { get; set; } = 0;
        public string Status { get; set; } = "active"; // active/inactive

        // Navigation properties
        public ICollection<Destination> Destinations { get; set; } = new List<Destination>();
    }
}
