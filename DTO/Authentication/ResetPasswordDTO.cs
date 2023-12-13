using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Authentication
{
    public class ResetPasswordDTO
    {
        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "OldPassword is required")]
        public string? OldPassword { get; set; }

        public string Id { get; set; }
    }
}
