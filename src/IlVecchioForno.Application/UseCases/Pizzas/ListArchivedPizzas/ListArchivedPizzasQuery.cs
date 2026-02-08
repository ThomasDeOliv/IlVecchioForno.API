using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Common.Responses;
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
) : IRequest<IResponse>;