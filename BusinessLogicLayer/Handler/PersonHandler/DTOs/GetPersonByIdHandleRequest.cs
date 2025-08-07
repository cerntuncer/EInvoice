using MediatR;

namespace BusinessLogicLayer.Handler.PersonHandler.DTOs
{
    public class GetPersonByIdHandleRequest : IRequest<GetPersonByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
