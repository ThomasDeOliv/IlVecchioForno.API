namespace IlVecchioForno.Application.UseCases.Pizzas.ListActivePizzas;

public sealed record ActivePizzaDTO(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    IReadOnlyList<ActivePizzaIngredientDTO> Ingredients
);