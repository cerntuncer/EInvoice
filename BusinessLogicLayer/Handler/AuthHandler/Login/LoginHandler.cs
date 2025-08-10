// using MediatR;
using System.Threading;
using System.Threading.Tasks;
using BusinessLogicLayer.DesignPatterns.Services.Auth;

namespace BusinessLogicLayer.Handler.AuthHandler.Login
{
    // public class LoginHandler : IRequestHandler<LoginRequest, LoginResponse>
    public class LoginHandler /* : IRequestHandler<LoginRequest, LoginResponse>*/
    {
        private readonly IAuthService _auth;
        public LoginHandler(IAuthService auth) => _auth = auth;

        public Task<LoginResponse> Handle(LoginRequest request/*, CancellationToken ct*/)
            => _auth.LoginAsync(request.Email, request.Password);
    }
}
