using Golinks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Golinks.Repository.Configurations;

public class LinkConfiguration : IEntityTypeConfiguration<Link>
{
    public void Configure(EntityTypeBuilder<Link> builder)
    {
        builder.ToTable("Links");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Url)
            .IsRequired()
            .HasMaxLength(1024);

        builder.Property(x => x.Slug)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.Property(x => x.TotalUsage)
            .HasDefaultValue(0)
            .IsRequired();

        builder.HasIndex(x => x.Slug)
            .IsUnique();
    }
}

