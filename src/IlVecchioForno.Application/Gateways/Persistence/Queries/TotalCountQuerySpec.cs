using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;

namespace IlVecchioForno.Application.Gateways.Persistence.Queries;

public sealed record TotalCountQuerySpec(
    IEnumerable<IFilterType> Filters
);