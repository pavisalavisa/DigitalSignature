using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
     public class EventConfiguration : AuditableEntityConfiguration<Event>
    {
        public new void Configure(EntityTypeBuilder<Event> builder)
        {
            builder.ToTable("Events");
            
            builder.HasOne(x => x.TriggeredBy).WithMany().OnDelete(DeleteBehavior.ClientSetNull);

            base.Configure(builder);
        }
    }
}