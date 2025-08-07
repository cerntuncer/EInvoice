using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ApiLayer.JWTHandling
{
    public static class JWTGenerator
    {
        public static string GenerateToken()
        {
            // Anahtar (güçlü bir key gir, bu örnek kısa ve test amaçlı)
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("jfhdfdfdfgjdfdsdsdffddsfsdfsdfsdfsdfsfsdfsf"));

            // İmzalama algoritması
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Token oluşturuluyor
            var token = new JwtSecurityToken(
                issuer: "http://localhost",
                audience: "http://localhost",
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials
            );

            // Token yazdırılıyor
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
    }
}
