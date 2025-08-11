using DatabaseAccessLayer.Models;

namespace DatabaseAccessLayer.Entities
{
    public class UserCredential : BaseEntity
    {
        public long UserId { get; set; }//user ile 1-1

        public string Provider { get; set; } = "Local";//kimlik sağlayıcısı:eposta şifre ile giriş
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? SecurityStamp { get; set; }//parola vs kritik bilgi değiştiğinde yenilenen damga.

        public int AccessFailedCount { get; set; }//yanlış şifre denemeleri sonrası kilitleme
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool EmailConfirmed { get; set; }//eposta doğrulaması tamamlandı mı 
        public DateTime? LastLoginAt { get; set; }//son başarılı giriş

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? PasswordUpdatedAt { get; set; }//parola son değişim zamanı


        public User User { get; set; } = null!;
    }

}
