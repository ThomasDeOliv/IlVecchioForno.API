using IlVecchioForno.API.Resources.PizzaIngredient;

namespace IlVecchioForno.API.Resources.ActivePizza;

public sealed record ActivePizzaResource(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    IReadOnlyList<PizzaIngredientResource> Ingredients
) : IResource;