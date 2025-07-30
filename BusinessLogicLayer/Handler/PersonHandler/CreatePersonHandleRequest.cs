using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.PersonHandler
{
    public class CreatePersonHandleRequest : IRequest<CreatePersonHandleResponse>
    {
        public string Name { get; set; }
        public long IdentityNumber { get; set; }
        public string TaxOffice { get; set; }
        public PersonType Type { get; set; }
        public Status Status { get; set; }
    }
}

