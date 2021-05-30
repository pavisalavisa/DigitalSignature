using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class CertificateConfiguration : AuditableEntityConfiguration<Certificate>
    {
        public new void Configure(EntityTypeBuilder<Certificate> builder)
        {
            builder.ToTable("Certificates");

            builder.Property(x => x.B64Certificate).IsRequired();
            builder.Property(x => x.B64Password).IsRequired();
            builder.Property(x => x.IsRevoked).IsRequired().HasDefaultValue(false);
            builder.HasOne(x => x.Owner).WithOne().OnDelete(DeleteBehavior.ClientSetNull);

            base.Configure(builder);
        }
    }
}