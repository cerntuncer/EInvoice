using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAP.Options
{
    public class CustomerSupplierMap : BaseMap<CustomerSupplier>
    {
        public override void Configure(EntityTypeBuilder<CustomerSupplier> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Person)
                   .WithOne()
                   .HasForeignKey<CustomerSupplier>(x => x.PersonId);

            builder.HasOne(x => x.User)
                   .WithMany(x => x.CustomerSuppliers)
                   .HasForeignKey(x => x.UserId);
        }
    }
}
