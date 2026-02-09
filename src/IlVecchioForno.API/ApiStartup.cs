using IlVecchioForno.API.Cors;
using IlVecchioForno.API.Filters;
using IlVecchioForno.API.Presenters.Ingredients;
using IlVecchioForno.API.Presenters.Pizzas;
using IlVecchioForno.API.Presenters.QuantityTypes;
using IlVecchioForno.Application.Gateways.Presentation;

namespace IlVecchioForno.API;

public static class ApiStartup
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        services.Configure<RouteOptions>(options => options.LowercaseUrls = true);

        services
            .AddDefaultCorsConfiguration()
            .AddScoped<IApiPizzaPresenter, PizzaPresenter>()
            .AddScoped<IPizzaPresenter>(sp => sp.GetRequiredService<IApiPizzaPresenter>())
            .AddScoped<IApiIngredientPresenter, IngredientPresenter>()
            .AddScoped<IIngredientPresenter>(sp => sp.GetRequiredService<IApiIngredientPresenter>())
            .AddScoped<IApiQuantityTypePresenter, QuantityTypePresenter>()
            .AddScoped<IQuantityTypePresenter>(sp => sp.GetRequiredService<IApiQuantityTypePresenter>())
            .AddScoped<GlobalExceptionFilter>()
            .AddOpenApi()
            .AddControllers(options => { options.Filters.Add<GlobalExceptionFilter>(); }
            );

        services.AddAuthentication();
        services.AddAuthorization();

        return services;
    }
}