using IlVecchioForno.Application.Common.DTOs;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Application.UseCases.Ingredients.Presenters;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.CountIngredients;

internal sealed class CountIngredientsHandler : IRequestHandler<CountIngredientsQuery>
{
    private readonly IIngredientPresenter _presenter;
    private readonly IIngredientRepository _repository;

    public CountIngredientsHandler(
        IIngredientPresenter presenter,
        IIngredientRepository repository
    )
    {
        this._presenter = presenter;
        this._repository = repository;
    }

    public async Task Handle(CountIngredientsQuery request, CancellationToken cancellationToken)
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