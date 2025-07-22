using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Options
{
    public class InvoiceMap : BaseMap<Invoice>
    {
        public override void Configure(EntityTypeBuilder<Invoice> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Current)
                   .WithMany()
                   .HasForeignKey(x => x.CurrentId);

            builder.HasOne(x => x.CustomerSupplier)
                   .WithMany()
                   .HasForeignKey(x => x.CustomerSupplierId);
        }
    }

}
