using System;
using System.Collections.Generic;
using DatabaseAccessLayer.Entities;

namespace BusinessLogicLayer.DesignPatterns.Services.Auth
{
    public interface IJwtTokenService
    {
        string GenerateAccessToken(User user, IEnumerable<string> roles, string email);
        (string token, DateTime expires) GenerateRefreshToken();
    }
}
