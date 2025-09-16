namespace PresentationLayer.Models
{
    public class LoginViewModel
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class ForgotPasswordSubmitModel
    {
        public string Email { get; set; } = null!;
        public string IdentityNumber { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

    public class GenericMessageResponse
    {
        public bool Error { get; set; }
        public string? Message { get; set; }
    }
}