using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.RegisterIngredient;

public sealed record RegisterIngredientCommand(
    string Name,
    short QuantityTypeId
) : IRequest<int>;