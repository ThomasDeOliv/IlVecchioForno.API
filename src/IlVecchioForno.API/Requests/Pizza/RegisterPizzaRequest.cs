namespace IlVecchioForno.API.Requests.Pizza;

public sealed record RegisterPizzaRequest(
    string Name,
    string? Description,
    decimal Price,
    IReadOnlyDictionary<int, int> IngredientsAndQuantities    
);