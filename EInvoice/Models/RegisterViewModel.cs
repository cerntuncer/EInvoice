namespace PresentationLayer.Models
{
    public class RegisterViewModel
    {
        public string Name { get; set; } = null!;
        public int UserType { get; set; }           // 1: Gerçek, 2: Tüzel
        public string IdentityNumber { get; set; } = null!;
        public string? TaxOffice { get; set; }
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
