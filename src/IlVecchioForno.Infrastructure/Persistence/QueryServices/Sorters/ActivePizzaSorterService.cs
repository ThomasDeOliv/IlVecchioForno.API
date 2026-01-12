using IlVecchioForno.Application.Gateways.Persistence.Queries.Sorters;
using IlVecchioForno.Domain.Pizzas;

namespace IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;

internal sealed class ActivePizzaSorterService : ISorterService<Pizza, ActivePizzasSorter>
{
    public IOrderedQueryable<Pizza> OrderBy(IQueryable<Pizza> q, ActivePizzasSorter sorter, bool descending)
    {
        return (sorter, descending) switch
        {
            (ActivePizzasSorter.Name, false) =>
                q.OrderBy(p => p.Name)
                    .ThenBy(p => p.Id),

            (ActivePizzasSorter.Name, true) =>
                q.OrderByDescending(p => p.Name)
                    .ThenBy(p => p.Id),

            (ActivePizzasSorter.Price, false) =>
                q.OrderBy(p => p.Price)
                    .ThenBy(p => p.Id),

            (ActivePizzasSorter.Price, true) =>
                q.OrderByDescending(p => p.Price)
                    .ThenBy(p => p.Id),

            (_, true) =>
                q.OrderByDescending(p => p.Id),

            _ => q.OrderBy(p => p.Id)
        };
    }
}