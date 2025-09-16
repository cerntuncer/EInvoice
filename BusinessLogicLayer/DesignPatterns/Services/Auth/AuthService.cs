using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using BusinessLogicLayer.Handler.AuthHandler.Login;
using DatabaseAccessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;


namespace BusinessLogicLayer.DesignPatterns.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserCredentialRepository _credRepo;//kimlik bilgileri tablosu
        private readonly IPasswordHasher<UserCredential> _hasher;//düz şifreyi veritabanındaki hash ile karşılaştırır
        private readonly IJwtTokenService _jwt;//token üretir/yeniler
        private readonly IConfiguration _cfg;//konfigürasyonlardan değer okur 

        public AuthService(
            IUserCredentialRepository credRepo,
            IPasswordHasher<UserCredential> hasher,
            IJwtTokenService jwt,
            IConfiguration cfg)
        {
            _credRepo = credRepo;
            _hasher = hasher;
            _jwt = jwt;
            _cfg = cfg;
        }

        public async Task<LoginResponse> LoginAsync(string email, string password)//giriş isteği
        {
            var normalizedEmail = NormalizeEmail(email);
            var cred = await _credRepo.GetByEmailAsync(normalizedEmail);
            if (cred is null) throw new UnauthorizedAccessException("Kullanıcı bulunamadı.");//email yoksa 401 

            if (cred.LockoutEnabled && cred.LockoutEnd.HasValue && cred.LockoutEnd > DateTime.UtcNow)
                throw new UnauthorizedAccessException("Hesap kilitli.");

            var verify = _hasher.VerifyHashedPassword(cred, cred.PasswordHash, password);//şifre doğrulama
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

            cred.RefreshToken = refresh;//refresh token ı kaydeder.tokenı dbde sakladım çünkü daha sonra doğrulama olacak
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

        private static string NormalizeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return string.Empty;
            var trimmed = email.Trim();
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

        public async Task<LoginResponse> RefreshAsync(string refreshToken)//müşteri sadece refresh token gönderir,sunucu yeni access ve yeni refresh üretir
        {
            var cred = await _credRepo.GetByRefreshTokenAsync(refreshToken);//refresh token doğrulama
            if (cred is null || cred.RefreshTokenExpiresAt <= DateTime.UtcNow)
                throw new UnauthorizedAccessException("Geçersiz/expired refresh token.");

            var roles = Enumerable.Empty<string>();//yeni access ve refresh üretimi 
            var access = _jwt.GenerateAccessToken(cred.User, roles, cred.Email);
            var (newRef, newExp) = _jwt.GenerateRefreshToken();

            cred.RefreshToken = newRef;//refresh güncellenir
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