namespace IlVecchioForno.Application.UseCases.QuantityTypes;

public sealed record QuantityTypeDto(
    int Id,
    string Name,
    string? Unit
);