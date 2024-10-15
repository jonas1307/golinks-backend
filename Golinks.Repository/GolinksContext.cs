using Golinks.Domain.Entities;
using Golinks.Repository.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Golinks.Repository;

public class GolinksContext(DbContextOptions<GolinksContext> options) : DbContext(options)
{
    public DbSet<Link> Links { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();

        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && e.State == EntityState.Added);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                ((BaseEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
            }
        }
    }
}

