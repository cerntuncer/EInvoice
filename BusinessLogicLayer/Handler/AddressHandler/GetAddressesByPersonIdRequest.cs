using MediatR;


namespace BusinessLogicLayer.Handler.AddressHandler
{
    public class GetAddressesByPersonIdRequest : IRequest<GetAddressesByPersonIdResponse>
    {
        public long PersonId { get; set; }
    }
}
