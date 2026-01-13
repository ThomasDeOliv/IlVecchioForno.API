using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using IlVecchioForno.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IlVecchioForno.Infrastructure;

public static class InfrastructureStartup
{
    public const string ConnectionStringName = "IlVecchioFornoContext";

    public static IServiceCollection AddInfrastructureDependencies(this IServiceCollection services,
        string connectionString)
    {
        return services.AddDbContext<IlVecchioFornoDbContext>(options => options.UseNpgsql(connectionString))
            .AddSingleton(TimeProvider.System)
            .AddScoped<IUnitOfWork, EfUnitOfWork>()
            .AddScoped<IPizzaRepository, EfPizzaRepository>()
            .AddScoped<IIngredientRepository, EfIngredientRepository>()
            .AddScoped<IQuantityTypeRepository, EfQuantityTypeRepository>()
            .AddScoped<IFilterService<Ingredient>, IngredientFilterService>()
            .AddScoped<IFilterService<Pizza>, PizzaFilterService>()
            .AddScoped<IFilterService<QuantityType>, QuantityTypeFilterService>()
            .AddScoped<ISorterService<Ingredient, IngredientsSorter>, IngredientSorterService>()
            .AddScoped<ISorterService<Pizza, ActivePizzasSorter>, ActivePizzaSorterService>()
            .AddScoped<ISorterService<Pizza, ArchivedPizzasSorter>, ArchivedPizzaSorterService>()
            .AddScoped<ISorterService<QuantityType, QuantityTypesSorter>, QuantityTypeSorterService>()
            .AddScoped(typeof(IPaginationService<>), typeof(PaginationService<>));
    }
}