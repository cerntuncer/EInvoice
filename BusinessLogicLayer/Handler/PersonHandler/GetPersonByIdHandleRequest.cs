using MediatR;

namespace BusinessLogicLayer.Handler.PersonHandler
{
    public class GetPersonByIdHandleRequest : IRequest<GetPersonByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
