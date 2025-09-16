using BusinessLogicLayer.DesignPatterns.GenericRepositories.BaseRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAccessLayer.Repositories
{
    //usercredential tablosuyla konuşuyor.EFcore repom
    public class UserCredentialRepository : BaseRepositoriesository<UserCredential>, IUserCredentialRepository
    {
        public UserCredentialRepository(MyContext db) : base(db){}

        public Task<UserCredential?> GetByEmailAsync(string email)
        {
            var norm = email.Trim().ToUpperInvariant();
            // Hem veritabanındaki e-postayı hem de girişi trim + upper ile karşılaştır
            return _db.UserCredentials
                      .Include(c => c.User)
                      .ThenInclude(u => u.Person)
                      .FirstOrDefaultAsync(c => (c.Provider == "Local" || c.Provider == null || c.Provider == "") &&
                                                c.Email != null &&
                                                c.Email.Trim().ToUpper() == norm);
        }

        // Refresh token ile kimlik getir
        public Task<UserCredential?> GetByRefreshTokenAsync(string token)
        {
            return _db.UserCredentials
                      .Include(c => c.User)
                      .FirstOrDefaultAsync(c => c.RefreshToken == token);
        }

        public async Task AddAsync(UserCredential credential)
        {
            if (credential.CreatedDate == default)
                credential.CreatedDate = DateTime.UtcNow;

            await _db.UserCredentials.AddAsync(credential);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateAsync(UserCredential credential)
        {
            credential.UpdatedDate = DateTime.UtcNow;
            _db.UserCredentials.Update(credential);
            await _db.SaveChangesAsync();
        }


        public Task<UserCredential?> GetByUserIdAsync(long userId)
        {
            return _db.UserCredentials
                      .Include(c => c.User)
                      .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public Task<bool> ExistsByEmailAsync(string email)
        {
            var norm = email.Trim().ToUpperInvariant();
            return _db.UserCredentials
                      .AsNoTracking()
                      .AnyAsync(c => c.Provider == "Local" &&
                                     c.Email.ToUpper() == norm);
        }
    }
}
