namespace IlVecchioForno.API.Resources.Ingredient;

public sealed record IngredientResource(
    int Id,
    string Name,
    int QuantityTypeId
) : IResource;