using DatabaseAccessLayer.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MAP.Options
{
    public class UserMap : BaseMap<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.HasOne(x => x.Person)
                   .WithOne()
                   .HasForeignKey<User>(x => x.PersonId);
        }
    }
}
