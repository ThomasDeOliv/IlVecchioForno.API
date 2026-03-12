using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Persistence.Configurations;

internal static class SchemasConfiguration
{
    public static void ApplySchemasConfiguration(this ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("pizzas_schema");
    }
}