namespace IlVecchioForno.Application.UseCases.QuantityTypes.ListQuantityTypes;

public sealed record QuantityTypeDTO(
    int Id,
    string Name,
    string? Unit
);