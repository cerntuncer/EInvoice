using MediatR;


namespace BusinessLogicLayer.Handler.AddressHandler.DTOs
{
    public class GetAddressByIdHandleRequest : IRequest<GetAddressByIdHandleResponse>
    {
        public long Id { get; set; } // Address ID
    }
}
