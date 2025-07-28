using DatabaseAccessLayer.Enumerations;
using MediatR;

namespace BusinessLogicLayer.Handler.UserHandler
{
    public class CreateUserHandleRequest : IRequest<CreateUserHandleResponse>
    {
        public UserType Type { get; set; }
        public Status Status { get; set; }
        public long PersonId { get; set; }
    }
}
