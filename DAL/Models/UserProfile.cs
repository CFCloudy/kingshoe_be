using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            Orders = new HashSet<Order>();
            UserAddresses = new HashSet<UserAddress>();
            VoucherBanks = new HashSet<VoucherBank>();
        }

        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? FullName { get; set; }

        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Avatar { get; set; }
        public int? RoleId { get; set; }
        public bool Gender { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<UserAddress>? UserAddresses { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
        public virtual ICollection<VoucherBank> VoucherBanks { get; set; }
    }
}
