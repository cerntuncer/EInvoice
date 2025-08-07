using MediatR;
using DatabaseAccessLayer.Enumerations;

namespace BusinessLogicLayer.Handler.UserHandler.DTOs
{
    public class UpdateUserHandleRequest : IRequest<UpdateUserHandleResponse>
    {
        public long Id { get; set; }
        public UserType Type { get; set; }
        public Status Status { get; set; }
    }

   
}
