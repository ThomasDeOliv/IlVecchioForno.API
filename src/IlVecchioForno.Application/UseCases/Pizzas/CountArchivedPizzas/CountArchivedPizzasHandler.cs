using IlVecchioForno.Application.Common.DTOs;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Presentation;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.CountArchivedPizzas;

internal sealed class CountArchivedPizzasHandler : IRequestHandler<CountArchivedPizzasQuery>
{
    private readonly IPizzaPresenter _presenter;
    private readonly IPizzaRepository _repository;

    public CountArchivedPizzasHandler(
        IPizzaPresenter presenter,
        IPizzaRepository repository
    )
    {
        this._presenter = presenter;
        this._repository = repository;
    }

    public async Task Handle(CountArchivedPizzasQuery request, CancellationToken cancellationToken)
    {
        int total = await this._repository.CountArchivedAsync(cancellationToken);
        this._presenter.EntitiesCount(new EntitiesCountDto(total));
    }
}