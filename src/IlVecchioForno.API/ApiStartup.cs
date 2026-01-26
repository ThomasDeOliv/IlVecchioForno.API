using IlVecchioForno.API.Filters;
using Microsoft.AspNetCore.Routing;

namespace IlVecchioForno.API;

public static class ApiStartup
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
        
        services.AddScoped<GlobalExceptionFilter>()
            .AddOpenApi()
            .AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());

        services.AddAuthentication();
        services.AddAuthorization();

        return services;
    }
}