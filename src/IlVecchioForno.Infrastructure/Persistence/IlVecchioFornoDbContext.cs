using IlVecchioForno.Domain.Common;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;

namespace IlVecchioForno.Infrastructure.Persistence;

public class IlVecchioFornoDbContext : DbContext
{
    private readonly TimeProvider _timeProvider;

    public IlVecchioFornoDbContext(
        DbContextOptions<IlVecchioFornoDbContext> options,
        TimeProvider timeProvider
    ) : base(options)
    {
        this._timeProvider = timeProvider;
    }

    public virtual DbSet<Pizza> Pizzas { get; set; }
    public virtual DbSet<Ingredient> Ingredients { get; set; }
    public virtual DbSet<QuantityType> QuantityTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyExtensionsConfiguration();
        modelBuilder.ApplySchemasConfiguration();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IlVecchioFornoDbContext).Assembly);
    }

    private void ApplyEntityAudit()
    {
        DateTime now = this._timeProvider.GetUtcNow().UtcDateTime;

        foreach (EntityEntry<IAuditable> entry in this.ChangeTracker.Entries<IAuditable>())
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Property(e => e.CreatedAt).CurrentValue = now;
                    entry.Property(e => e.UpdatedAt).CurrentValue = now;
                    break;

                case EntityState.Modified:
                    entry.Property(e => e.UpdatedAt).CurrentValue = now;
                    break;
            }
    }

    public override int SaveChanges()
    {
        this.ApplyEntityAudit();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        this.ApplyEntityAudit();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        this.ApplyEntityAudit();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        this.ApplyEntityAudit();
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }
}