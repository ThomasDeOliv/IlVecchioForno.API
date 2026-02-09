using IlVecchioForno.Application.Common.Responses;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;
using IlVecchioForno.Domain.QuantityTypes;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.GetQuantityType;

internal sealed class
    GetQuantityTypeHandler : IRequestHandler<GetQuantityTypeQuery, IResponse>
{
    private readonly IMapper _mapper;
    private readonly IQuantityTypeRepository _repository;

    public GetQuantityTypeHandler(IMapper mapper, IQuantityTypeRepository repository)
    {
        this._mapper = mapper;
        this._repository = repository;
    }

    public async Task<IResponse> Handle(GetQuantityTypeQuery query,
        CancellationToken cancellationToken)
    {
        QuantityType? item = await this._repository.FindAsync(
            query.Id,
            cancellationToken
        );

        if (item is null)
            return new ErrorResponseWithMessage(
                ErrorResponseType.EntityNotFoundError,
                $"Quantity type with id {query.Id} was not found."
            );

        return new Response<QuantityTypeDto>(
            ResponseType.Query,
            this._mapper.Map<QuantityTypeDto>(item)
        );
    }
}