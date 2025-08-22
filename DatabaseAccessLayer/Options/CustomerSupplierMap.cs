using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
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

            builder.HasOne(cs => cs.Person)
                   .WithOne()
                   .HasForeignKey<CustomerSupplier>(x => x.PersonId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(cs => cs.User)
                   .WithMany(u => u.CustomerSuppliers)
                   .HasForeignKey(cs => cs.UserId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}