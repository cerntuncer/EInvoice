using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.PersonHandler.DTOs
{
    public class GetPersonByIdHandleResponse
    {
        public string Message { get; set; }
        public bool Error { get; set; }
        public long Id { get; set; }
        public string Name { get; set; }
        public long IdentityNumber { get; set; }
        public string? TaxOffice { get; set; }
        public PersonType Type { get; set; }
        public Status Status { get; set; }
    }
}
