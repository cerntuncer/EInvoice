using DatabaseAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Entities
{
    public class UserCredential : BaseEntity
    {
        public long UserId { get; set; }

        public string Provider { get; set; } = "Local";
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string? SecurityStamp { get; set; }

        public int AccessFailedCount { get; set; }
        public bool LockoutEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public bool EmailConfirmed { get; set; }
        public DateTime? LastLoginAt { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public DateTime? PasswordUpdatedAt { get; set; }


        public User User { get; set; } = null!;
    }

}
