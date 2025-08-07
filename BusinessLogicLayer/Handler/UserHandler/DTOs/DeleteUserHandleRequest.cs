using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler.DTOs
{
    public class DeleteUserHandleRequest : IRequest<DeleteUserHandleResponse>
    {
        public long Id { get; set; }
    }

  
}
