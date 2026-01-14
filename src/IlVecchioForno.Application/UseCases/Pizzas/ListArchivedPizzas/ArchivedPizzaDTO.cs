namespace IlVecchioForno.Application.UseCases.Pizzas.ListArchivedPizzas;

public sealed record ArchivedPizzaDTO(
    int Id,
    string Name,
    string? Description,
    decimal Price,
    DateTimeOffset Archived,
    IReadOnlyList<ArchivedPizzaIngredientDTO> Ingredients
);