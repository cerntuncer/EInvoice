using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DatabaseAccessLayer.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogicLayer.DesignPatterns.Services.Auth
{
    public class JwtTokenService : IJwtTokenService
    {
        private readonly IConfiguration _cfg;
        public JwtTokenService(IConfiguration cfg) => _cfg = cfg;

        public string GenerateAccessToken(User user, IEnumerable<string> roles, string email)//access jwtüretir
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);//imza algoritması hmac256

            var claims = new List<Claim>//kullanıcının id,mail
    {
        new("sub", user.Id.ToString()),
        new(ClaimTypes.Email, email ?? string.Empty)
    };

            // Roller boşsa burası zaten pas geçilir
            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));

            var token = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],//tokenı yayınlayan(uygulama)
                audience: _cfg["Jwt:Audience"],//tokenı kimin kullanacağı
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:AccessTokenMinutes"]!)),
                signingCredentials: creds//hmac imza için gerekli bilgiler
            );

            return new JwtSecurityTokenHandler().WriteToken(token);//string jwt
        }

        public (string token, DateTime expires) GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(bytes);//base64 saklaması kolay bir string
            var exp = DateTime.UtcNow.AddDays(int.Parse(_cfg["Jwt:RefreshTokenDays"]!));
            return (token, exp);
        }
    }
}
