using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;

namespace IlVecchioForno.Application.Gateways.Persistence.Queries;

public sealed record ListQuerySpec<TSorter>(
    int Page,
    int PageSize,
    TSorter Sorter,
    bool Descending,
    IEnumerable<IFilterType> Filters
) where TSorter : struct, Enum;