using FluentValidation;
using Mapster;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace IlVecchioForno.Application;

public static class ApplicationStartup
{
    public static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.RequireExplicitMapping = true;
        TypeAdapterConfig.GlobalSettings.Scan(typeof(ApplicationStartup).Assembly);

        services.AddValidatorsFromAssembly(typeof(ApplicationStartup).Assembly, includeInternalTypes: true)
            .AddMediatR(config => { config.RegisterServicesFromAssembly(typeof(ApplicationStartup).Assembly); })
            .AddSingleton(TypeAdapterConfig.GlobalSettings)
            .AddScoped<IMapper, ServiceMapper>();

        return services;
    }
}