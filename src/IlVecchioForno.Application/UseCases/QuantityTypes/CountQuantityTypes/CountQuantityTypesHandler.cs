using IlVecchioForno.Application.Common.DTOs;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Presentation;
using MediatR;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.CountQuantityTypes;

internal sealed class CountQuantityTypesHandler : IRequestHandler<CountQuantityTypesQuery>
{
    private readonly IQuantityTypePresenter _presenter;
    private readonly IQuantityTypeRepository _repository;

    public CountQuantityTypesHandler(
        IQuantityTypePresenter presenter,
        IQuantityTypeRepository repository
    )
    {
        this._presenter = presenter;
        this._repository = repository;
    }

    public async Task Handle(CountQuantityTypesQuery request, CancellationToken cancellationToken)
    {
        int total = await this._repository.CountAsync(cancellationToken);
        this._presenter.EntitiesCount(new EntitiesCountDto(total));
    }
}