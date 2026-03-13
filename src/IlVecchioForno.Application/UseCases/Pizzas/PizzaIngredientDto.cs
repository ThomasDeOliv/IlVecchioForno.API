namespace IlVecchioForno.Application.UseCases.Pizzas;

public sealed record PizzaIngredientDto(
    decimal Quantity,
    int IngredientId
);