namespace IlVecchioForno.Application.UseCases.Pizzas.ListActivePizzas;

public sealed record ActivePizzaIngredientDTO(
    decimal quantity,
    int IngredientId
);