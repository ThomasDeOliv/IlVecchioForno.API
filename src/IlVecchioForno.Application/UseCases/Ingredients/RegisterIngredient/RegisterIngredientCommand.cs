using IlVecchioForno.Application.Common;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.RegisterIngredient;

public sealed record RegisterIngredientCommand(
    string Name,
    short QuantityTypeId
) : IRequest<Result<int>>;