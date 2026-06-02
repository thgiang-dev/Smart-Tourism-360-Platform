using System;
using SmartTourism360.Domain.Common;

namespace SmartTourism360.Domain.Entities
{
    public class User : BaseEntity
    {
        public Guid RoleId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public Guid? AvatarId { get; set; }
        public string Status { get; set; } = "active"; // active/inactive/locked
        public DateTime? LastLoginAt { get; set; }

        // Navigation properties
        public Role Role { get; set; } = null!;
        public MediaFile? Avatar { get; set; }
    }
}
