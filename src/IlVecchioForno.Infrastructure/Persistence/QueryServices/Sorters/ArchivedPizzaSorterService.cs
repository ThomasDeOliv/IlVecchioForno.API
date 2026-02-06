using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Domain.Pizzas;

namespace IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;

internal sealed class ArchivedPizzaSorterService : ISorterService<Pizza, ArchivedPizzasSorter>
{
    public IOrderedQueryable<Pizza> OrderBy(IQueryable<Pizza> q, ArchivedPizzasSorter sorter, bool descending)
    {
        return (sorter, descending) switch
        {
            (ArchivedPizzasSorter.Name, false) =>
                q.OrderBy(p => p.Name.Value)
                    .ThenBy(p => p.Id),

            (ArchivedPizzasSorter.Name, true) =>
                q.OrderByDescending(p => p.Name.Value)
                    .ThenBy(p => p.Id),

            (ArchivedPizzasSorter.Price, false) =>
                q.OrderBy(p => p.Price.Value)
                    .ThenBy(p => p.Id),

            (ArchivedPizzasSorter.Price, true) =>
                q.OrderByDescending(p => p.Price.Value)
                    .ThenBy(p => p.Id),

            (ArchivedPizzasSorter.Archived, false) =>
                q.OrderBy(p => p.ArchivedAt)
                    .ThenBy(p => p.Id),

            (ArchivedPizzasSorter.Archived, true) =>
                q.OrderByDescending(p => p.ArchivedAt)
                    .ThenBy(p => p.Id),

            (_, true) =>
                q.OrderByDescending(p => p.Id),

            _ => q.OrderBy(p => p.Id)
        };
    }
}