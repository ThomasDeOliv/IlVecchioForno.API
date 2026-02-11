using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Domain.Pizzas;

namespace IlVecchioForno.Application.Gateways.Persistence;

public interface IPizzaRepository
{
    Task<int> TotalCountActiveAsync(
        TotalCountQuerySpec query,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<Pizza>> ListActiveAsync(
        ListQuerySpec<ActivePizzasSorter> query,
        CancellationToken cancellationToken = default
    );

    Task<int> TotalCountArchivedAsync(
        TotalCountQuerySpec query,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<Pizza>> ListArchivedAsync(
        ListQuerySpec<ArchivedPizzasSorter> query,
        CancellationToken cancellationToken = default
    );

    Task<Pizza?> FindAsync(
        int id,
        CancellationToken cancellationToken = default
    );

    void Add(Pizza pizza);
}