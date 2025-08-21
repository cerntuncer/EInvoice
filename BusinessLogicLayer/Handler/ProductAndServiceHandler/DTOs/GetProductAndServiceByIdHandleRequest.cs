using MediatR;

namespace BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs
{
    public class GetProductAndServiceByIdHandleRequest : IRequest<GetProductAndServiceByIdHandleResponse>
    {
        public long Id { get; set; }
        public long UserId { get; set; }
    }

}
