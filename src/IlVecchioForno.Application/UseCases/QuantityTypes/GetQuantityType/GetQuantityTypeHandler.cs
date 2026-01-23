using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;
using IlVecchioForno.Domain.QuantityTypes;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.GetQuantityType;

internal sealed class GetQuantityTypeHandler : IRequestHandler<GetQuantityTypeQuery, QuantityTypeDto?>
{
    private readonly IMapper _mapper;
    private readonly IQuantityTypeRepository _repository;

    public GetQuantityTypeHandler(IMapper mapper, IQuantityTypeRepository repository)
    {
        this._mapper = mapper;
        this._repository = repository;
    }

    public async Task<QuantityTypeDto?> Handle(GetQuantityTypeQuery query, CancellationToken cancellationToken)
    {
        QuantityType? item = await this._repository.FindAsync(
            query.Id,
            cancellationToken
        );

        return item is not null
            ? this._mapper.Map<QuantityTypeDto>(item)
            : null;
    }
}