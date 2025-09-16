using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAP.Options
{
    public class PersonMap : BaseMap<Person>
    {
        public override void Configure(EntityTypeBuilder<Person> builder)
        {
            base.Configure(builder);

            builder.Property(p => p.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(p => p.TaxOffice)
                   .HasMaxLength(150);

            builder.HasIndex(p => p.IdentityNumber)
                   .IsUnique();
        }
    }
}

