using MediatR;

namespace BusinessLogicLayer.Handler.CurrentHandler.DTOs
{
    public class GetCurrentsByUserIdHandleRequest : IRequest<GetCurrentsByUserIdHandleResponse>
    {
        public long UserId { get; set; }
    }

}
