using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler
{
    public class DeleteUserHandleRequest : IRequest<DeleteUserHandleResponse>
    {
        public long Id { get; set; }
    }

  
}
