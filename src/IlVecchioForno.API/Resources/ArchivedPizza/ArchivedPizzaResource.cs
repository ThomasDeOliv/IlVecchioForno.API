using IlVecchioForno.API.Resources.PizzaIngredient;

namespace IlVecchioForno.API.Resources.ArchivedPizza;

public sealed record ArchivedPizzaResource(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    DateTimeOffset ArchivedAt,
    IReadOnlyList<PizzaIngredientResource> Ingredients
) : IResource;