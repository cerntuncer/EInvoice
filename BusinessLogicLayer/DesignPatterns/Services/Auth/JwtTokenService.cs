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

        public string GenerateAccessToken(User user, IEnumerable<string> roles, string email)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
    {
        new("sub", user.Id.ToString()),
        new(ClaimTypes.Email, email ?? string.Empty)
    };

            // Roller boşsa burası zaten pas geçilir
            foreach (var r in roles)
                claims.Add(new Claim(ClaimTypes.Role, r));

            var token = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],
                audience: _cfg["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:AccessTokenMinutes"]!)),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string token, DateTime expires) GenerateRefreshToken()
        {
            var bytes = RandomNumberGenerator.GetBytes(64);
            var token = Convert.ToBase64String(bytes);
            var exp = DateTime.UtcNow.AddDays(int.Parse(_cfg["Jwt:RefreshTokenDays"]!));
            return (token, exp);
        }
    }
}
