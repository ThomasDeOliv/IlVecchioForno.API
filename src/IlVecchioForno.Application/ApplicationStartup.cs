using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace IlVecchioForno.Application;

public static class ApplicationStartup
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        TypeAdapterConfig typeAdapterConfig = TypeAdapterConfig.GlobalSettings;
        TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
        typeAdapterConfig.Scan(typeof(ApplicationStartup).Assembly);

        services.AddSingleton(typeAdapterConfig)
            .AddScoped<IMapper, ServiceMapper>()
            .AddValidatorsFromAssembly(typeof(ApplicationStartup).Assembly, includeInternalTypes: true)
            .AddMediatR(config => { config.RegisterServicesFromAssembly(typeof(ApplicationStartup).Assembly); });

        return services;
    }
}