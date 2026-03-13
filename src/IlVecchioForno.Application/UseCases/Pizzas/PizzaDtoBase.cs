namespace IlVecchioForno.Application.UseCases.Pizzas;

public abstract record PizzaDtoBase(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    IReadOnlyList<PizzaIngredientDto> Ingredients
);