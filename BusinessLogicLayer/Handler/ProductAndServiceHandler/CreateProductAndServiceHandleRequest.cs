using MediatR;

namespace BusinessLogicLayer.Handler.ProductAndServiceHandler
{
    public class CreateProductAndServiceHandleRequest : IRequest<CreateProductAndServiceHandleResponse>
    {
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
