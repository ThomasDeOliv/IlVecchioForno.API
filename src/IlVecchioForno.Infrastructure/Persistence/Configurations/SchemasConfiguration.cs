using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Persistence.Configurations;

public static class SchemasConfiguration
{
    public static void ApplySchemaConfiguration(this ModelBuilder modelBuilder, string name)
    {
        modelBuilder.HasDefaultSchema(name);
    }
}