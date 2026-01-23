namespace IlVecchioForno.API.Resources.PizzaIngredient;

public sealed record PizzaIngredientResource(
    decimal Quantity,
    int IngredientId
) : IResource;