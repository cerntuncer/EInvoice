using BusinessLogicLayer.DesignPatterns.Services.Auth;

namespace BusinessLogicLayer.Handler.AuthHandler.Refresh
{
    public class RefreshHandler /* : IRequestHandler<RefreshRequest, LoginResponse>*/
    {
        private readonly IAuthService _auth;
        public RefreshHandler(IAuthService auth) => _auth = auth;

        public Task<BusinessLogicLayer.Handler.AuthHandler.Login.LoginResponse>
            Handle(RefreshRequest request/*, CancellationToken ct*/)
            => _auth.RefreshAsync(request.RefreshToken);
    }
}
