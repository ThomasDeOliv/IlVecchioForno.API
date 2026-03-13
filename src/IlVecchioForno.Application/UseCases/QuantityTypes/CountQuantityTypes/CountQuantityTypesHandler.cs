using IlVecchioForno.Application.Common.DTOs;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
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
        TotalCountQuerySpec querySpec = new TotalCountQuerySpec(
            new List<IFilterType>
            {
                new SearchFilterType(request.Search)
            }
        );
        int total = await this._repository.TotalCountAsync(querySpec, cancellationToken);
        this._presenter.EntitiesCount(new EntitiesCountDto(total));
    }
}