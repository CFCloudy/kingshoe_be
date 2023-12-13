using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ApplicationUser: IdentityUser
    {
        public UserProfile? UserProfiles { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public ICollection<UserOtp>? Otps { get; set; }

        public int Status { get; set; }
    }
}
