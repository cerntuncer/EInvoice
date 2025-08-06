using MediatR;

namespace BusinessLogicLayer.Handler.CurrentHandler
{
    public class GetCurrentByIdHandleRequest : IRequest<GetCurrentByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
