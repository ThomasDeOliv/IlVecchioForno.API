using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ListArchivedPizzas;

public sealed record ListArchivedPizzasQuery(
    int Page,
    int PageSize,
    ArchivedPizzasSorter Sorter,
    bool Descending,
    string? Search,
    decimal? MinPrice,
    decimal? MaxPrice
) : IRequest<Result<IReadOnlyList<ArchivedPizzaDTO>>>;