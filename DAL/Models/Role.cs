using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class Role
    {
        public Role()
        {
            UserProfiles = new HashSet<UserProfile>();
        }

        public int Id { get; set; }
        public string? RoleName { get; set; }
        public int? Status { get; set; }
        public string? RoleUrl { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }
}
