namespace IlVecchioForno.Application.UseCases.Pizzas.ListArchivedPizzas;

public sealed record ArchivedPizzaIngredientDTO(
    decimal quantity,
    int IngredientId
);