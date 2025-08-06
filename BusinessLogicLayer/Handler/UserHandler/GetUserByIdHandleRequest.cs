using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler
{
    public class GetUserByIdHandleRequest : IRequest<GetUserByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
