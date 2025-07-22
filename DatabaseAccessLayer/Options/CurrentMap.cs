using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Options
{
    public class CurrentMap : BaseMap<Current>
    {
        public override void Configure(EntityTypeBuilder<Current> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.Current)
                   .HasForeignKey(x => x.UserId);
        }
    }
}
