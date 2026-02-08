using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Common.Responses;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.ListIngredients;

public sealed record ListIngredientsQuery(
    int Page,
    int PageSize,
    IngredientsSorter Sorter,
    bool Descending,
    string? Search
) : IRequest<IResponse>;