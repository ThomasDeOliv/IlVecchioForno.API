using IlVecchioForno.Application.Common.Responses;
using MediatR;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.GetQuantityType;

public sealed record GetQuantityTypeQuery(
    short Id
) : IRequest<IResponse>;