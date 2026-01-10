using Microsoft.Extensions.DependencyInjection;

namespace IlVecchioForno.Infrastructure;

public static class InfrastructureStartup
{
    public const string ConnectionStringName = "IlVecchioFornoContext";
    
    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services, string connectionString)
    {
        return null!;
    }
}