using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Common.Exceptions;

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
                            || (decimal)p.Price >= rangeFilter.Min.Value
                        )
                        &&
                        (
                            !rangeFilter.Max.HasValue
                            || (decimal)p.Price <= rangeFilter.Max.Value
                        )
                    ),

                SearchFilterType searchFilter when !string.IsNullOrEmpty(searchFilter.Search) =>
                    current.Where(p =>
                        ((string)p.Name).Contains(searchFilter.Search)
                        || p.Description != null
                        && ((string)p.Description).Contains(searchFilter.Search)
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