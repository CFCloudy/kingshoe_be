using System;
using System.Collections.Generic;

namespace DAL.Models
{
    public partial class UserOtp
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public DateTime CreatedTime { get; set; }

        public DateTime ExpireTime { get; set; }
        public bool IsUsed { get; set; }

        public string Code { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
    }
}
