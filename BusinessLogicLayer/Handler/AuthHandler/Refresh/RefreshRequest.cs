using MediatR;
using BusinessLogicLayer.Handler.AuthHandler.Login;

namespace BusinessLogicLayer.Handler.AuthHandler.Refresh
{
    public class RefreshRequest : IRequest<LoginResponse>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
