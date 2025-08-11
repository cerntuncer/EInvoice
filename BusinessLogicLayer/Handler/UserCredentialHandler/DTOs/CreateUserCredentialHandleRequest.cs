using MediatR;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.DTOs
{
    public class CreateUserCredentialHandleRequest : IRequest<CreateUserCredentialHandleResponse>
    {
        public long UserId { get; set; }   // Kimlik hangi User'a açılıyor
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool LockoutEnabled { get; set; } = false;
    }
}
