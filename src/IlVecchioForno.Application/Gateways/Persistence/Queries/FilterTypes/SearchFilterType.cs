namespace IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;

public sealed record SearchFilterType(
    string? Search
) : IFilterType;