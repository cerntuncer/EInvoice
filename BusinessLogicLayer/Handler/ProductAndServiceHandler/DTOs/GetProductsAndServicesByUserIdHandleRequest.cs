using MediatR;
namespace BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs
{
    public class GetProductsAndServicesByUserIdHandleRequest : IRequest<GetProductsAndServicesByUserIdHandleResponse>
    {
        public long UserId { get; set; }
    }
}
