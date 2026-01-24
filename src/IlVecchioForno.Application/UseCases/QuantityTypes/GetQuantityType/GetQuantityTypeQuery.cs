using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;
using MediatR;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.GetQuantityType;

public sealed record GetQuantityTypeQuery(
    short Id
) : IRequest<QuantityTypeDto>;