using DatabaseAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAP.Options
{
    public abstract class BaseMap<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Identity alanı
            builder.Property(x => x.Id)
                   .ValueGeneratedOnAdd();

            // CreatedDate zorunlu
            builder.Property(x => x.CreatedDate)
                   .IsRequired()
                   .HasDefaultValueSql("GETUTCDATE()");

            // UpdatedDate opsiyonel
            builder.Property(x => x.UpdatedDate)
                   .IsRequired(false);
        }
    }

}
