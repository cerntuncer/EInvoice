using MediatR;

namespace BusinessLogicLayer.Handler.AuthHandler.Login
{
    public sealed class LoginRequest : IRequest<LoginResponse>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
