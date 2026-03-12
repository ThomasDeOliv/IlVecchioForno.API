namespace IlVecchioForno.API.Requests.Pizzas;

public sealed record RegisterPizzaRequest(
    string Name,
    string? Description,
    decimal Price,
    IReadOnlyDictionary<int, int> IngredientsAndQuantities
);