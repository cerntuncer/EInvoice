using MediatR;

namespace BusinessLogicLayer.Handler.AddressHandler.DTOs
{
    public class DeleteAddressHandleRequest : IRequest<DeleteAddressHandleResponse>
    {
        public long Id { get; set; }
        public long PersonId { get; set; }
        
    }
}
