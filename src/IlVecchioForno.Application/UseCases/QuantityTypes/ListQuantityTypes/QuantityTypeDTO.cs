namespace IlVecchioForno.Application.UseCases.QuantityTypes.ListQuantityTypes;

public record QuantityTypeDTO(
    int Id,
    string Name,
    string? Unit
);