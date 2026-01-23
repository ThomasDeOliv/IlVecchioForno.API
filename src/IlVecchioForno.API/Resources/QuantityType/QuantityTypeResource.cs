namespace IlVecchioForno.API.Resources.QuantityType;

public sealed record QuantityTypeResource(
    int Id,
    string Name,
    string? Unit
) : IResource;