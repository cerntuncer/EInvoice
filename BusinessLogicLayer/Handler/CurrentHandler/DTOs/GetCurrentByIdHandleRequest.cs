using MediatR;

namespace BusinessLogicLayer.Handler.CurrentHandler.DTOs
{
    public class GetCurrentByIdHandleRequest : IRequest<GetCurrentByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
