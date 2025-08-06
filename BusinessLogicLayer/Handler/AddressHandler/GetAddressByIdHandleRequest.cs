using MediatR;


namespace BusinessLogicLayer.Handler.AddressHandler
{
    public class GetAddressByIdHandleRequest : IRequest<GetAddressByIdHandleResponse>
    {
        public long Id { get; set; } // Address ID
    }
}
