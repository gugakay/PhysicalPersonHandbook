using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Entities.Configuration
{
    public class ConnectedPersonConfiguration : IEntityTypeConfiguration<ConnectedPerson>
    {
        public void Configure(EntityTypeBuilder<ConnectedPerson> builder)
        {
            builder.ToTable("ConnectedPersons");
            builder.Ignore(p => p.Person);
            builder.HasKey(x => x.Id);
        }
    }
}
