using System.ComponentModel.DataAnnotations;

namespace PresentationLayer.Models
{
    public class ProfileViewModel
    {
        public long UserId { get; set; }
        public int UserType { get; set; }
        public int UserStatus { get; set; }

        public long PersonId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public long IdentityNumber { get; set; }

        public string TaxOffice { get; set; }

        public int PersonType { get; set; }
        public int PersonStatus { get; set; }

        public string Email { get; set; }
    }
}