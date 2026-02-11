using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.CountIngredients;

public sealed record CountIngredientsQuery(
    string? Search
) : IRequest;