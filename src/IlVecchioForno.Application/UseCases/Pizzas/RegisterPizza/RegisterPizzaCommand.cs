using IlVecchioForno.Application.Common;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.RegisterPizza;

public sealed record RegisterPizzaCommand(
    string Name,
    string? Description,
    decimal Price,
    IReadOnlyDictionary<int, int> IngredientsAndQuantities
) : IRequest<Result<int>>;