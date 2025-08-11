using System.Threading.Tasks;
using BusinessLogicLayer.Handler.AuthHandler.Login;

namespace BusinessLogicLayer.DesignPatterns.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(string email, string password);
        Task<LoginResponse> RefreshAsync(string refreshToken);
        /*kullanıcı oturumu devam ederken access token süresi
         * bitince tekrar email şifre almadan yeni token 
         * almasını sağlar
         */

    }
}
