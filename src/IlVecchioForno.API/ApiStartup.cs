using IlVecchioForno.API.Filters;

namespace IlVecchioForno.API;

public static class ApiStartup
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        services.AddScoped<GlobalExceptionFilter>()
            .AddOpenApi()
            .AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());

        services.AddAuthentication();
        services.AddAuthorization();

        return services;
    }
}