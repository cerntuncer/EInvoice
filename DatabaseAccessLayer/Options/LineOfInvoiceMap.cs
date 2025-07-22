using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Options
{
    public class LineOfInvoiceMap : BaseMap<LineOfInvoice>
    {
        public override void Configure(EntityTypeBuilder<LineOfInvoice> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.ProductAndService)
                   .WithMany()
                   .HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.Invoice)
                   .WithMany(x => x.LineOfInvoices)
                   .HasForeignKey(x => x.InvoiceID);
        }
    }
}
