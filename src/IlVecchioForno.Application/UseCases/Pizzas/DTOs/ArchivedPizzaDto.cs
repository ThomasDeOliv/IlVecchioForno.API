namespace IlVecchioForno.Application.UseCases.Pizzas.DTOs;

public sealed record ArchivedPizzaDto(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    DateTimeOffset ArchivedAt,
    IReadOnlyList<PizzaIngredientDto> Ingredients
) : PizzaDtoBase(
    Id,
    Name,
    Description,
    Price,
    Ingredients
);