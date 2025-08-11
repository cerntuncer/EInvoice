using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Options
{
    public class UserCredentialMap : IEntityTypeConfiguration<UserCredential>
    {
        public void Configure(EntityTypeBuilder<UserCredential> builder)
        {
            builder.ToTable("UserCredentials");

            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.User)
                   .WithOne(u => u.UserCredential)
                   .HasForeignKey<UserCredential>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.UserId).IsUnique();
            builder.HasIndex(x => x.Email).IsUnique();
        }
    }

}
