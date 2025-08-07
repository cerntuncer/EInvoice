using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler.DTOs
{
    public class GetUserByIdHandleRequest : IRequest<GetUserByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
