using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Domain.QuantityTypes;

namespace IlVecchioForno.Application.Gateways.Persistence;

public interface IQuantityTypeRepository
{
    Task<int> TotalCountAsync(
        TotalCountQuerySpec query,
        CancellationToken cancellationToken = default
    );

    Task<IReadOnlyCollection<QuantityType>> ListAsync(
        ListQuerySpec<QuantityTypesSorter> query,
        CancellationToken cancellationToken = default
    );

    Task<QuantityType?> FindAsync(
        short id,
        CancellationToken cancellationToken = default
    );
}