namespace IlVecchioForno.API.Requests.Pizza;

public sealed record ChangePizzaDetailsRequest(
    string? Description,
    decimal Price,
    IReadOnlyDictionary<int, int> IngredientsAndQuantities
);