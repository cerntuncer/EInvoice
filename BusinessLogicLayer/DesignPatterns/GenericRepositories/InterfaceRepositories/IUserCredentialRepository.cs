
using System;
using System.Threading.Tasks;
using DatabaseAccessLayer.Entities; // UserCredential entity’nin namespace’i

namespace BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories
{
    public interface IUserCredentialRepository
    {
        Task<UserCredential?> GetByEmailAsync(string email);
        Task<UserCredential?> GetByRefreshTokenAsync(string token);
        Task AddAsync(UserCredential credential);
        Task UpdateAsync(UserCredential credential);
    }
}