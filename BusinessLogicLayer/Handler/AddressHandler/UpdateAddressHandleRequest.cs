using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.AddressHandler
{
    public class UpdateAddressHandleRequest : IRequest<UpdateAddressHandleResponse>
    {
        public long Id { get; set; }
        public AddressType AddressType { get; set; }
        public string Text { get; set; }
        public long PersonId { get; set; }
       

    }
}
