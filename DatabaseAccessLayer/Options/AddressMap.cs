
using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAP.Options
{

    public class AddressMap : BaseMap<Address>
    {
        public override void Configure(EntityTypeBuilder<Address> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Person)
                   .WithMany(x => x.Addresses)
                   .HasForeignKey(x => x.PersonId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}