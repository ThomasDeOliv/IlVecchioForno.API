using IlVecchioForno.API.Cors;
using IlVecchioForno.API.Filters;

namespace IlVecchioForno.API;

public static class ApiStartup
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services
            .AddDefaultCorsConfiguration()
            .AddScoped<GlobalExceptionFilter>()
            .AddOpenApi()
            .AddControllers(options => { options.Filters.Add<GlobalExceptionFilter>(); });

        services.AddAuthentication();
        services.AddAuthorization();

        return services;
    }
}