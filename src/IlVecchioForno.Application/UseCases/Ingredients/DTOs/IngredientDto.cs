namespace IlVecchioForno.Application.UseCases.Ingredients.DTOs;

public sealed record IngredientDto(
    int Id,
    string Name,
    int QuantityTypeId
);