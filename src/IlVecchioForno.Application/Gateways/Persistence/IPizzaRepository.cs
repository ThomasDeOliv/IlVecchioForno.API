using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Domain.Pizzas;

namespace IlVecchioForno.Application.Gateways.Persistence;

public interface IPizzaRepository
{
    Task<IReadOnlyCollection<Pizza>> ListActiveAsync(
        QuerySpec<ActivePizzasSorter> query,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<Pizza>> ListArchivedAsync(
        QuerySpec<ArchivedPizzasSorter> query,
        CancellationToken cancellationToken = default
    );

    Task<Pizza?> FindAsync(
        int id,
        CancellationToken cancellationToken = default
    );

    Task<int> CountActiveAsync(
        CancellationToken cancellationToken = default
    );

    Task<int> CountArchivedAsync(
        CancellationToken cancellationToken = default
    );

    void Add(Pizza pizza);
}