using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Presentation;
using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;
using IlVecchioForno.Domain.QuantityTypes;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.GetQuantityType;

internal sealed class
    GetQuantityTypeHandler : IRequestHandler<GetQuantityTypeQuery>
{
    private readonly IMapper _mapper;
    private readonly IQuantityTypePresenter _presenter;
    private readonly IQuantityTypeRepository _repository;

    public GetQuantityTypeHandler(
        IMapper mapper,
        IQuantityTypePresenter presenter,
        IQuantityTypeRepository repository
    )
    {
        this._mapper = mapper;
        this._presenter = presenter;
        this._repository = repository;
    }

    public async Task Handle(
        GetQuantityTypeQuery query,
        CancellationToken cancellationToken
    )
    {
        QuantityType? item = await this._repository.FindAsync(
            query.Id,
            cancellationToken
        );

        if (item is not null)
        {
            this._presenter.EntityFound(
                this._mapper.Map<QuantityTypeDto>(item)
            );

            return;
        }

        this._presenter.EntityNotFound($"Quantity type with id {query.Id} was not found.");
    }
}