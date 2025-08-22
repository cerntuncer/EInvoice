using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Options
{
    public class BankMap : BaseMap<Bank>
    {
        public override void Configure(EntityTypeBuilder<Bank> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Current)
                   .WithMany()
                   .HasForeignKey(x => x.CurrentId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
