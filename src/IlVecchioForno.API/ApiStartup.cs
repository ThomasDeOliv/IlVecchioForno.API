using IlVecchioForno.API.Filters;
using IlVecchioForno.API.Presenters;
using IlVecchioForno.API.Resources.ActivePizza;
using IlVecchioForno.API.Resources.ArchivedPizza;
using IlVecchioForno.API.Resources.Ingredient;
using IlVecchioForno.API.Resources.QuantityType;
using IlVecchioForno.Application.UseCases.Ingredients.DTOs;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;

namespace IlVecchioForno.API;

public static class ApiStartup
{
    public static IServiceCollection AddApiDependencies(this IServiceCollection services)
    {
        services.AddScoped<GlobalExceptionFilter>();
        services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
        services.AddAuthentication();
        services.AddAuthorization();
        services.AddOpenApi();

        services.AddScoped<IPresenter<ActivePizzaDto, ActivePizzaResource>, ActivePizzaPresenter>();
        services.AddScoped<IPresenter<ArchivedPizzaDto, ArchivedPizzaResource>, ArchivedPizzaPresenter>();
        services.AddScoped<IPresenter<IngredientDto, IngredientResource>, IngredientPresenter>();
        services.AddScoped<IPresenter<QuantityTypeDto, QuantityTypeResource>, QuantityTypePresenter>();

        return services;
    }
}