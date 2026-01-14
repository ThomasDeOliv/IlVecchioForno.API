using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ListActivePizzas;

public sealed record ListActivePizzasQuery(
    int Page,
    int PageSize,
    ActivePizzasSorter Sorter,
    bool Descending,
    string? Search,
    decimal? MinPrice,
    decimal? MaxPrice
) : IRequest<Result<IReadOnlyList<ActivePizzaDTO>>>;