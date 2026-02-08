using IlVecchioForno.Application.Common.Responses;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.RegisterPizza;

public sealed record RegisterPizzaCommand(
    string Name,
    string? Description,
    decimal Price,
    IReadOnlyDictionary<int, int> IngredientsAndQuantities
) : IRequest<IResponse>;