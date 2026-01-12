using IlVecchioForno.Application.Gateways.Persistence.Queries.Sorters;
using IlVecchioForno.Domain.Ingredients;

namespace IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;

internal sealed class IngredientSorterService : ISorterService<Ingredient, IngredientsSorter>
{
    public IOrderedQueryable<Ingredient> OrderBy(IQueryable<Ingredient> q, IngredientsSorter sorter, bool descending)
    {
        return (sorter, descending) switch
        {
            (IngredientsSorter.Name, true) =>
                q.OrderByDescending(p => p.Name)
                    .ThenBy(p => p.Id),

            (IngredientsSorter.Name, false) =>
                q.OrderBy(p => p.Name)
                    .ThenBy(p => p.Id),

            (IngredientsSorter.Id, true) =>
                q.OrderByDescending(p => p.Id),

            _ => q.OrderBy(p => p.Id)
        };
    }
}