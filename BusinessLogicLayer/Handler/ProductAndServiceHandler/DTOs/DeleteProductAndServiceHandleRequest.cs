using MediatR;
namespace BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs
{
   
    public class DeleteProductAndServiceHandleRequest : IRequest<DeleteProductAndServiceHandleResponse>
    {
        public long Id { get; set; }
        public long UserId { get; set; }
    }

}
