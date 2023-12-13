using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Authentication
{
    public class LoginResponse
    {
        public string Id { get; set; }

        public string? Email { get; set; }

        public string? FullName { get; set; }

        public string? LastName { get; set; }

        public int? Role { get; set; }

        public string? Avartar { get; set; }

        public bool Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public string? RefreshToken { get; set; }

        public string? AccessToken { get; set; }

        public DateTime? RefreshTokenExpiryTime { get; set; }
        public int ProfilesID { get; set; }
    }
}
