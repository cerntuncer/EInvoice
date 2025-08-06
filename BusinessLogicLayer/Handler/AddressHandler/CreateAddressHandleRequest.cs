using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.AddressHandler
{
    public class CreateAddressHandleRequest :IRequest<CreateAddressHandleResponse>
    {
        public AddressType AddressType { get; set; }   // Ev, iş, fatura vs.
        public string Text { get; set; }             
        public long PersonId { get; set; } 
      

    }
}
