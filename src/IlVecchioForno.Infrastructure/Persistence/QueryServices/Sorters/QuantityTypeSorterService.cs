using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Domain.QuantityTypes;

namespace IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;

internal sealed class QuantityTypeSorterService : ISorterService<QuantityType, QuantityTypesSorter>
{
    public IOrderedQueryable<QuantityType> OrderBy(IQueryable<QuantityType> q, QuantityTypesSorter sorter,
        bool descending)
    {
        return (sorter, descending) switch
        {
            (QuantityTypesSorter.Name, true) =>
                q.OrderByDescending(p => p.Name)
                    .ThenBy(p => p.Id),

            (QuantityTypesSorter.Name, false) =>
                q.OrderBy(p => p.Name)
                    .ThenBy(p => p.Id),

            (QuantityTypesSorter.Unit, true) =>
                q.OrderByDescending(p => p.Unit)
                    .ThenBy(p => p.Id),

            (QuantityTypesSorter.Unit, false) =>
                q.OrderBy(p => p.Unit)
                    .ThenBy(p => p.Id),

            (QuantityTypesSorter.Id, true) =>
                q.OrderByDescending(p => p.Id),

            _ => q.OrderBy(p => p.Id)
        };
    }
}