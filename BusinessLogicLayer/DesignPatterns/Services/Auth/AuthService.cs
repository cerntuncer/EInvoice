using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.AuthHandler.Login;
using DatabaseAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace BusinessLogicLayer.DesignPatterns.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserCredentialRepository _credRepo;
        private readonly IUserRepository _userRepo; // sende zaten var
        private readonly IPasswordHasher<UserCredential> _hasher;
        private readonly IJwtTokenService _jwt;
        private readonly IConfiguration _cfg;

        public AuthService(
            IUserCredentialRepository credRepo,
            IUserRepository userRepo,
            IPasswordHasher<UserCredential> hasher,
            IJwtTokenService jwt,
            IConfiguration cfg)
        {
            _credRepo = credRepo;
            _userRepo = userRepo;
            _hasher = hasher;
            _jwt = jwt;
            _cfg = cfg;
        }

        public async Task<LoginResponse> LoginAsync(string email, string password)
        {
            var cred = await _credRepo.GetByEmailAsync(email);
            if (cred is null) throw new UnauthorizedAccessException("Kullanıcı bulunamadı.");

            if (cred.LockoutEnabled && cred.LockoutEnd.HasValue && cred.LockoutEnd > DateTime.UtcNow)
                throw new UnauthorizedAccessException("Hesap kilitli.");

            var verify = _hasher.VerifyHashedPassword(cred, cred.PasswordHash, password);
            if (verify == PasswordVerificationResult.Failed)
            {
                cred.AccessFailedCount += 1;
                if (cred.LockoutEnabled && cred.AccessFailedCount >= 5)
                {
                    cred.LockoutEnd = DateTime.UtcNow.AddMinutes(15);
                    cred.AccessFailedCount = 0;
                }
                await _credRepo.UpdateAsync(cred);
                throw new UnauthorizedAccessException("E‑posta veya şifre hatalı.");
            }

            cred.AccessFailedCount = 0;
            cred.LastLoginAt = DateTime.UtcNow;

            var user = cred.User;
            var roles = Enumerable.Empty<string>();

            var access = _jwt.GenerateAccessToken(user, roles, cred.Email);
            var (refresh, refreshExp) = _jwt.GenerateRefreshToken();

            cred.RefreshToken = refresh;
            cred.RefreshTokenExpiresAt = refreshExp;
            await _credRepo.UpdateAsync(cred);

            return new LoginResponse
            {
                AccessToken = access,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_cfg.GetValue<int>("Jwt:AccessTokenMinutes")),
                RefreshToken = refresh,
                RefreshTokenExpiresAt = refreshExp
            };
        }

        public async Task<LoginResponse> RefreshAsync(string refreshToken)
        {
            var cred = await _credRepo.GetByRefreshTokenAsync(refreshToken);
            if (cred is null || cred.RefreshTokenExpiresAt <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Geçersiz/expired refresh token.");

            var roles = Enumerable.Empty<string>();
            var access = _jwt.GenerateAccessToken(cred.User, roles, cred.Email);
            var (newRef, newExp) = _jwt.GenerateRefreshToken();

            cred.RefreshToken = newRef;
            cred.RefreshTokenExpiresAt = newExp;
            await _credRepo.UpdateAsync(cred);

            return new LoginResponse
            {
                AccessToken = access,
                AccessTokenExpiresAt = DateTime.UtcNow.AddMinutes(_cfg.GetValue<int>("Jwt:AccessTokenMinutes")),
                RefreshToken = newRef,
                RefreshTokenExpiresAt = newExp
            };
        }
    }
}
