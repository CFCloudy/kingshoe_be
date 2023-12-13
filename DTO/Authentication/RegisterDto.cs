using System.ComponentModel.DataAnnotations;


namespace DTO.Authentication
{
    public class RegisterDto
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public string? FullName { get; set; }

        public string? LastName { get; set; }
    }
}
