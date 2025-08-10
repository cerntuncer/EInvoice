using System;
using MediatR;

namespace BusinessLogicLayer.Handler.UserCredentialHandler.DTOs
{
    public sealed class CreateUserCredentialHandleRequest : IRequest<CreateUserCredentialHandleResponse>
    {
        public long UserId { get; set; }                 // Hangi User için kimlik oluşturuluyor
        public string Email { get; set; } = null!;       // Login email
        public string Password { get; set; } = null!;    // Düz şifre (hashlenecek)
        public bool LockoutEnabled { get; set; } = false;// Opsiyonel
    }
}
