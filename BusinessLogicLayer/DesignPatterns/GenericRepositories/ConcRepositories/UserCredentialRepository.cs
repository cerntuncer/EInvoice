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

        public async Task<UserCredential?> GetByEmailAsync(string email)
        {
            var normLower = NormalizeEmail(email);

            // İlk deneme: server-side kıyas (daha performanslı)
            var cred = await _db.UserCredentials
                      .Include(c => c.User)
                      .ThenInclude(u => u.Person)
                      .FirstOrDefaultAsync(c => (c.Provider == "Local" || c.Provider == null || c.Provider == "") &&
                                                c.Email != null &&
                                                c.Email.Trim().ToLower() == normLower);

            if (cred != null) return cred;

            // İkinci deneme: accent-insensitive normalization (client-side)
            cred = _db.UserCredentials
                      .Include(c => c.User)
                      .ThenInclude(u => u.Person)
                      .AsNoTracking()
                      .AsEnumerable()
                      .FirstOrDefault(c => (c.Provider == "Local" || c.Provider == null || c.Provider == "") &&
                                           c.Email != null &&
                                           NormalizeEmail(c.Email) == normLower);
            return cred;
        }

        private static string NormalizeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return string.Empty;
            var trimmed = email.Trim();
            // Unicode normalization + diacritic removal + invariant lower
            var formD = trimmed.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder(formD.Length);
            foreach (var ch in formD)
            {
                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(ch);
                }
            }
            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC).ToLowerInvariant();
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
