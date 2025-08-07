using MediatR;


namespace BusinessLogicLayer.Handler.AddressHandler.DTOs
{
    public class GetAddressesByPersonIdRequest : IRequest<GetAddressesByPersonIdResponse>
    {
        public long PersonId { get; set; }
    }
}
