using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.GetIngredient;

public sealed record GetIngredientQuery(
    int Id
) : IRequest;