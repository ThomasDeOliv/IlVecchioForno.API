using IlVecchioForno.Application.Common.DTOs;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Presentation;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.CountActivePizzas;

internal sealed class CountActivePizzasHandler : IRequestHandler<CountActivePizzasQuery>
{
    private readonly IPizzaPresenter _presenter;
    private readonly IPizzaRepository _repository;

    public CountActivePizzasHandler(
        IPizzaPresenter presenter,
        IPizzaRepository repository
    )
    {
        this._presenter = presenter;
        this._repository = repository;
    }

    public async Task Handle(CountActivePizzasQuery request, CancellationToken cancellationToken)
    {
        int total = await this._repository.CountActiveAsync(cancellationToken);
        this._presenter.EntitiesCount(new EntitiesCountDto(total));
    }
}