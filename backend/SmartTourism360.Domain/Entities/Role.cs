using System.Collections.Generic;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }

        // Navigation property
        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
