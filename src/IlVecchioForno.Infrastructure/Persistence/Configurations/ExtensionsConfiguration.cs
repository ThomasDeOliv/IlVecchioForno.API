using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Persistence.Configurations;

public static class ExtensionsConfiguration
{
    public static void ApplyExtensionsConfiguration(this ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("citext");
    }
}