using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler.DTOs
{
    public class GetUserWithPersonByIdHandleRequest : IRequest<GetUserWithPersonByIdHandleResponse>
    {
        public long Id { get; set; }
    }
}