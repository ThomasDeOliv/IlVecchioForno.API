using IlVecchioForno.Application.Common.Queries.Sorters;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.ListIngredients;

public sealed record ListIngredientsQuery(
    int Page,
    int PageSize,
    IngredientsSorter Sorter,
    bool Descending,
    string? Search
) : IRequest;