using System.Threading;
using System.Threading.Tasks;
using MediatR;
using BusinessLogicLayer.DesignPatterns.Services.Auth;
using BusinessLogicLayer.Handler.AuthHandler.Login;

namespace BusinessLogicLayer.Handler.AuthHandler.Refresh
{
    public class RefreshHandler : IRequestHandler<RefreshRequest, LoginResponse>
    {
        private readonly IAuthService _auth;

        public RefreshHandler(IAuthService auth)
        {
            _auth = auth;
        }

        public Task<LoginResponse> Handle(RefreshRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request?.RefreshToken))
                throw new ArgumentException("Refresh token zorunludur.");

            return _auth.RefreshAsync(request.RefreshToken);
        }
    }
}
