using System.Threading.Tasks;
using BusinessLogicLayer.Handler.AuthHandler.Login;

namespace BusinessLogicLayer.DesignPatterns.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(string email, string password);
        Task<LoginResponse> RefreshAsync(string refreshToken);
    }
}
