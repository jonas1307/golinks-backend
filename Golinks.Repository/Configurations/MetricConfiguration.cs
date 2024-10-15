using Golinks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Golinks.Repository.Configurations;

public class MetricConfiguration : IEntityTypeConfiguration<Metric>
{
    public void Configure(EntityTypeBuilder<Metric> builder)
    {
        builder.ToTable("Metrics");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.LinkId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasOne(x => x.Link)
           .WithMany(x => x.Metrics)
           .HasForeignKey(x => x.LinkId);
    }
}
