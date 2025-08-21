using DatabaseAccessLayer.Entities;
using MAP.Options;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DatabaseAccessLayer.Options
{
    public class ProductAndServiceMap : BaseMap<ProductAndService>
    {
        public override void Configure(EntityTypeBuilder<ProductAndService> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.User)
                   .WithMany()
                   .HasForeignKey(x => x.UserId);
        }
    }
}