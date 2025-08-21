using DatabaseAccessLayer.Enumerations;

namespace PresentationLayer.Models.ApiResponses
{
    public class GetUserWithPersonByIdResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long UserId { get; set; }
        public int UserType { get; set; }
        public int UserStatus { get; set; }
        public long PersonId { get; set; }
        public string PersonName { get; set; }
        public long IdentityNumber { get; set; }
        public string? TaxOffice { get; set; }
        public int PersonType { get; set; }
        public int PersonStatus { get; set; }
    }
}
