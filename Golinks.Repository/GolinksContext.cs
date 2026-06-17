using Golinks.Domain;
using Golinks.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace Golinks.Repository;

public class GolinksContext(DbContextOptions<GolinksContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Link> Links { get; set; }
    public DbSet<Metric> Metrics { get; set; }

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

    private IDbContextTransaction _currentTransaction;

    public async Task BeginTransactionAsync()
    {
        _currentTransaction = await Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        if (_currentTransaction == null) return;
        await _currentTransaction.CommitAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackAsync()
    {
        if (_currentTransaction == null) return;
        await _currentTransaction.RollbackAsync();
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity && (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;

            if (entry.State == EntityState.Added)
                entity.CreatedAt = DateTime.UtcNow;
            else
                entity.UpdatedAt = DateTime.UtcNow;
        }
    }
}

