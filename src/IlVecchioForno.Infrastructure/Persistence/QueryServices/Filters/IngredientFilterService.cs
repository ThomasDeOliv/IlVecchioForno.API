using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Infrastructure.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;

internal sealed class IngredientFilterService : IFilterService<Ingredient>
{
    public IQueryable<Ingredient> Filter(IQueryable<Ingredient> q, IEnumerable<IFilterType> filters)
    {
        return filters.Aggregate(q, (current, filter) => filter switch
            {
                SearchFilterType searchFilter when !string.IsNullOrEmpty(searchFilter.Search) =>
                    current.Where(i =>
                        EF.Functions.ILike(i.Name, $"%{searchFilter.Search}%")
                    ),

                SearchFilterType searchFilter when string.IsNullOrEmpty(searchFilter.Search) => current,

                _ => throw new InvalidFilterException(
                    nameof(Ingredient),
                    new NotSupportedException($"Filter type {filter.GetType().Name} is not supported.")
                )
            }
        );
    }
}