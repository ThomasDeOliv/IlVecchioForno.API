using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using MediatR;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.ListQuantityTypes;

public sealed record ListQuantityTypesQuery(
    int Page,
    int PageSize,
    QuantityTypesSorter Sorter,
    bool Descending,
    string? Search
) : IRequest<Result<IReadOnlyList<QuantityTypeDTO>>>;