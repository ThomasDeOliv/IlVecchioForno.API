using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
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
) : IRequest<IReadOnlyList<ActivePizzaDto>>;