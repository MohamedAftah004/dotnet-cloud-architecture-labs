using CloudJobEngine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudJobEngine.Infrastructure.Persistence.Configurations
{
    public class JobConfiguration : IEntityTypeConfiguration<Job>
    {
        public void Configure(EntityTypeBuilder<Job> builder)
        {
            builder.HasKey(j => j.Id);

            builder.Property(j => j.UserId)
                   .IsRequired();

            builder.Property(j => j.FileKey)
                   .HasMaxLength(500);

            builder.Property(j => j.Status)
                   .IsRequired();

            builder.Property(j => j.CreatedAt)
                   .IsRequired();

            builder.Property(j => j.ProcessedAt);
        }
    }
}
