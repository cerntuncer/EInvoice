using MediatR;

namespace BusinessLogicLayer.Handler.ProductAndServiceHandler
{
    public class GetProductAndServiceByIdHandleRequest : IRequest<GetProductAndServiceByIdHandleResponse>
    {
        public long Id { get; set; }
    }

}
