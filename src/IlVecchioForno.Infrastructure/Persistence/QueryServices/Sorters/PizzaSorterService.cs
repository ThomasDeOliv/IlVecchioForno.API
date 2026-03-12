using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Domain.Pizzas;

// ReSharper disable All

namespace IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;

internal sealed class PizzaSorterService : ISorterService<Pizza, PizzasSorter>
{
    public IOrderedQueryable<Pizza> OrderBy(IQueryable<Pizza> q, PizzasSorter sorter, bool descending)
    {
        return (sorter, descending) switch
        {
            (PizzasSorter.Name, false) =>
                q.OrderBy(p => p.Name.Value)
                    .ThenBy(p => p.Id),

            (PizzasSorter.Name, true) =>
                q.OrderByDescending(p => p.Name.Value)
                    .ThenBy(p => p.Id),

            (PizzasSorter.Price, false) =>
                q.OrderBy(p => p.Price.Value)
                    .ThenBy(p => p.Id),

            (PizzasSorter.Price, true) =>
                q.OrderByDescending(p => p.Price.Value)
                    .ThenBy(p => p.Id),

            (PizzasSorter.Archived, false) =>
                q.OrderBy(p => p.ArchivedAt.HasValue
                            ? p.ArchivedAt.Value
                            : DateTimeOffset.MaxValue // Null last
                    )
                    .ThenBy(p => p.Id),

            (PizzasSorter.Archived, true) =>
                q.OrderByDescending(p => p.ArchivedAt.HasValue
                            ? p.ArchivedAt.Value
                            : DateTimeOffset.MinValue // Null last
                    )
                    .ThenBy(p => p.Id),

            (_, true) =>
                q.OrderByDescending(p => p.Id),

            _ => q.OrderBy(p => p.Id)
        };
    }
}