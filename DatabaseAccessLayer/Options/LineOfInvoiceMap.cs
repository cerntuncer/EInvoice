using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace MAP.Options
{
    public class LineOfInvoiceMap : BaseMap<LineOfInvoice>
    {
        public override void Configure(EntityTypeBuilder<LineOfInvoice> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.ProductAndService)
                   .WithMany()
                   .HasForeignKey(x => x.ProductAndServiceId)
                   .OnDelete(DeleteBehavior.NoAction);


            builder.HasOne(x => x.Invoice)
                   .WithMany(x => x.LineOfInvoices)
                   .HasForeignKey(x => x.InvoiceId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
