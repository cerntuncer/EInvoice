using DatabaseAccessLayer.Entities;
using MAP.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccessLayer.Options
{
    public class UserCredentialMap : BaseMap<UserCredential>
    {
        public override void Configure(EntityTypeBuilder<UserCredential> builder)
        {

            base.Configure(builder);

            builder.HasOne(x => x.User)
                   .WithOne(u => u.UserCredential)
                   .HasForeignKey<UserCredential>(x => x.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

        }
    }

}
