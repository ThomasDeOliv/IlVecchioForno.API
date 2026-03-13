namespace IlVecchioForno.Application.UseCases.Ingredients;

public sealed record IngredientDto(
    int Id,
    string Name,
    short? QuantityTypeId
);