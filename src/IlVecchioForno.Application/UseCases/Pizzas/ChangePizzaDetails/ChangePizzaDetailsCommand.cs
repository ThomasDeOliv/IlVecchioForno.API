using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ChangePizzaDetails;

public sealed record ChangePizzaDetailsCommand(
    int Id,
    string? Description,
    decimal Price,
    IReadOnlyDictionary<int, int> IngredientsAndQuantities
) : IRequest;