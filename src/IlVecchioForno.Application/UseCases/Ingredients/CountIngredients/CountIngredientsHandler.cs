using IlVecchioForno.Application.Common.DTOs;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Presentation;
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
        int total = await this._repository.CountAsync(cancellationToken);
        this._presenter.EntitiesCount(new EntitiesCountDto(total));
    }
}