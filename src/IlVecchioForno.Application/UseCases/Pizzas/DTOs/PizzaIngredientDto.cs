namespace IlVecchioForno.Application.UseCases.Pizzas.DTOs;

public sealed record PizzaIngredientDto(
    decimal Quantity,
    int IngredientId
);