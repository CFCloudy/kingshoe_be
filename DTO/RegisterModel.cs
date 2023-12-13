using System.ComponentModel.DataAnnotations;


namespace DTO
{
    public class RegisterModel
    {

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        public string? FullName { get; set; }

        public string? LastName { get; set; }
    }
    public class SendOTPDTO
    {
        public string Id { get; set; }
    }

    public class ConfirmOTPModel
    {
        public string? Code { get; set; }

        public string? UserId { get; set; }
    }

    public class ChangeStatus
    {
        public string Id { get; set; }

        public bool Status { get; set; }
    }

    public class TokenModel
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
