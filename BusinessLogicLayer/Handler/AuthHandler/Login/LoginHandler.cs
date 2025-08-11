using System.Threading;
using System.Threading.Tasks;
using MediatR;
using BusinessLogicLayer.DesignPatterns.Services.Auth;

namespace BusinessLogicLayer.Handler.AuthHandler.Login
{
    public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
    {
        private readonly IAuthService _auth;

        public LoginHandler(IAuthService auth)
        {
            _auth = auth;
        }

        public Task<LoginResponse> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request?.Email) || string.IsNullOrWhiteSpace(request?.Password))
                throw new ArgumentException("Email ve şifre zorunludur.");

            return _auth.LoginAsync(request.Email, request.Password);
        }
    }
}
