using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Entities.Configuration
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person>builder)
        {
            builder.HasMany(e => e.PhoneNumbers)
                .WithOne(s => s.Person)
                .HasForeignKey(s => s.PersonId);

            //builder.HasMany(e => e.ConnectedPersons)
            //    .WithOne(s => s.Person)
            //    .HasForeignKey(s => s.PersonId);

            builder.HasOne(e => e.Image)
                .WithOne(s => s.Person)
                .HasForeignKey<Image>(s => s.PersonId);

            builder.Property(c => c.Name).IsRequired().HasMaxLength(50);
            builder.Property(c => c.LastName).IsRequired().HasMaxLength(50);
            builder.Property(c => c.PrivateNumber).IsRequired().HasMaxLength(11);        
        }
    }
}
