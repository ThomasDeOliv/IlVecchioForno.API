namespace IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;

public sealed record QuantityTypeDto(
    int Id,
    string Name,
    string? Unit
);