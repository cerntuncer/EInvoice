using MediatR;

namespace BusinessLogicLayer.Handler.CurrentHandler
{
    public class GetCurrentsByUserIdHandleRequest : IRequest<GetCurrentsByUserIdHandleResponse>
    {
        public long UserId { get; set; }
    }

}
