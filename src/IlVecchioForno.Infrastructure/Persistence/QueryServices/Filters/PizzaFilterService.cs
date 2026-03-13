using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;

internal sealed class PizzaFilterService : IFilterService<Pizza>
{
    public IQueryable<Pizza> Filter(IQueryable<Pizza> q, IEnumerable<IFilterType> filters)
    {
        return filters.Aggregate(q, (current, filter) => filter switch
            {
                RangeFilterType<decimal> rangeFilter =>
                    current.Where(p =>
                        (
                            !rangeFilter.Min.HasValue
                            || p.Price >= rangeFilter.Min.Value
                        )
                        &&
                        (
                            !rangeFilter.Max.HasValue
                            || p.Price <= rangeFilter.Max.Value
                        )
                    ),

                SearchFilterType searchFilter when !string.IsNullOrEmpty(searchFilter.Search) =>
                    current.Where(p =>
                        EF.Functions.ILike(p.Name, $"%{searchFilter.Search}%")
                        || p.Description != null
                        && EF.Functions.ILike(p.Description, $"%{searchFilter.Search}%")
                    ),

                SearchFilterType searchFilter when string.IsNullOrEmpty(searchFilter.Search) => current,

                _ => throw new NotSupportedFilterException(
                    nameof(Pizza),
                    new NotSupportedException($"Filter type {filter.GetType().Name} is not supported.")
                )
            }
        );
    }
}