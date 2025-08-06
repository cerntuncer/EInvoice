using MediatR;
namespace BusinessLogicLayer.Handler.ProductAndServiceHandler
{
   
    public class DeleteProductAndServiceHandleRequest : IRequest<DeleteProductAndServiceHandleResponse>
    {
        public long Id { get; set; }
    }

}
