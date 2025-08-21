using System.Threading.Tasks;
using DatabaseAccessLayer.Entities;

namespace BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories
{
    public interface IUserCredentialRepository : IRepository<UserCredential>
    {
        Task<UserCredential?> GetByEmailAsync(string email);
        Task<UserCredential?> GetByRefreshTokenAsync(string token);
        Task<UserCredential?> GetByUserIdAsync(long userId);   // eklendi
        Task<bool> ExistsByEmailAsync(string email);           // eklendi (opsiyonel ama faydalı)

        Task AddAsync(UserCredential credential);
        Task UpdateAsync(UserCredential credential);
    }
}
