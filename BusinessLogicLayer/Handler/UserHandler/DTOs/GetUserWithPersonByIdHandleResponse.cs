using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.UserHandler.DTOs
{
    public class GetUserWithPersonByIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long UserId { get; set; }
        public UserType UserType { get; set; }
        public Status UserStatus { get; set; }
        public long PersonId { get; set; }
        public string PersonName { get; set; }
        public long IdentityNumber { get; set; }
        public string? TaxOffice { get; set; }
        public PersonType PersonType { get; set; }
        public Status PersonStatus { get; set; }
    }
}