using IlVecchioForno.API.Cors;
using IlVecchioForno.API.Filters;
using IlVecchioForno.API.Presenters;

namespace IlVecchioForno.API;

public static class ApiStartup
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services
            .AddDefaultCorsConfiguration()
            .AddScoped<IPresenter, Presenter>()
            .AddScoped<GlobalExceptionFilter>()
            .AddScoped<CreatedAtLocationFilter>()
            .AddOpenApi()
            .AddControllers(options =>
                {
                    options.Filters.Add<GlobalExceptionFilter>(); // TODO reevaluate and remove in future
                    options.Filters.Add<CreatedAtLocationFilter>();
                }
            );

        services.AddAuthentication();
        services.AddAuthorization();

        return services;
    }
}