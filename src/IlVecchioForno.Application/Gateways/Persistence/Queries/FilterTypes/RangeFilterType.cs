using System.Numerics;

namespace IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;

public sealed record RangeFilterType<T>(
    T? Min,
    T? Max
) : IFilterType
    where T : struct, INumber<T>;