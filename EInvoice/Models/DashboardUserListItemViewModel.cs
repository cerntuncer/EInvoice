using DatabaseAccessLayer.Enumerations;

namespace PresentationLayer.Models
{
    public class DashboardUserListItemViewModel
    {
        public long UserId { get; set; }
        public string Name { get; set; } = null!;
        public string UserType { get; set; } = null!;
        public long IdentityNumber { get; set; }
        public string TaxOffice { get; set; }
    }
}
