namespace IlVecchioForno.Application.UseCases.Ingredients.ListIngredients;

public sealed record IngredientDTO(
    int Id,
    string Name,
    int QuantityTypeId
);