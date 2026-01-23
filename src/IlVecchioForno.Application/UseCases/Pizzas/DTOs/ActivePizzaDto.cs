namespace IlVecchioForno.Application.UseCases.Pizzas.DTOs;

public sealed record ActivePizzaDto(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    IReadOnlyList<PizzaIngredientDto> Ingredients
) : PizzaDtoBase(
    Id,
    Name,
    Description,
    Price,
    Ingredients
);