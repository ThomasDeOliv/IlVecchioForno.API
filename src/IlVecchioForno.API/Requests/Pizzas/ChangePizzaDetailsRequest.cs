namespace IlVecchioForno.API.Requests.Pizzas;

public sealed record ChangePizzaDetailsRequest(
    string? Description,
    decimal Price,
    IReadOnlyDictionary<int, int> IngredientsAndQuantities
);