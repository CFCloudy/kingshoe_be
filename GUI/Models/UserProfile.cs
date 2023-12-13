using System;
using System.Collections.Generic;

namespace GUI.Models
{
    public partial class UserProfile
    {
        public UserProfile()
        {
            Orders = new HashSet<Order>();
            //UserAddresses = new HashSet<UserAddress>();
            //UserOtps = new HashSet<UserOtp>();
            VoucherBanks = new HashSet<VoucherBank>();
        }

        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? Email { get; set; }
        public string? FullName { get; set; }
        public string? Phone { get; set; }
        public int? Avatar { get; set; }
        public int? RoleId { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }

        //public virtual Role? Role { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        //public virtual ICollection<UserAddress> UserAddresses { get; set; }
        //public virtual ICollection<UserOtp> UserOtps { get; set; }
        public virtual ICollection<VoucherBank> VoucherBanks { get; set; }
    }
}
