using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Common.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;

internal sealed class QuantityTypeFilterService : IFilterService<QuantityType>
{
    public IQueryable<QuantityType> Filter(IQueryable<QuantityType> q, IEnumerable<IFilterType> filters)
    {
        return filters.Aggregate(q, (current, filter) => filter switch
            {
                SearchFilterType searchFilter when !string.IsNullOrWhiteSpace(searchFilter.Search) =>
                    current.Where(qT =>
                        EF.Functions.ILike(qT.Name, $"%{searchFilter.Search}%")
                        || EF.Functions.ILike(qT.Unit, $"%{searchFilter.Search}%")
                    ),

                SearchFilterType searchFilter when string.IsNullOrEmpty(searchFilter.Search) => current,

                _ => throw new NotSupportedFilterException(
                    nameof(QuantityType),
                    new NotSupportedException($"Filter type {filter.GetType().Name} is not supported.")
                )
            }
        );
    }
}