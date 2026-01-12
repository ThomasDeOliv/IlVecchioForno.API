using IlVecchioForno.Domain.Common;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace IlVecchioForno.Infrastructure.Persistence;

public class IlVecchioFornoDbContext : DbContext
{
    public IlVecchioFornoDbContext(DbContextOptions<IlVecchioFornoDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Pizza> Pizzas { get; set; }
    public virtual DbSet<Ingredient> Ingredients { get; set; }
    public virtual DbSet<QuantityType> QuantityTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplySchemaConfiguration("pizzas_schema");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IlVecchioFornoDbContext).Assembly);
    }

    private void ApplyEntityAudit()
    {
        DateTime now = DateTime.UtcNow;

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
}