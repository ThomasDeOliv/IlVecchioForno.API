using MediatR;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.CountQuantityTypes;

public sealed record CountQuantityTypesQuery(
    string? Search
) : IRequest;