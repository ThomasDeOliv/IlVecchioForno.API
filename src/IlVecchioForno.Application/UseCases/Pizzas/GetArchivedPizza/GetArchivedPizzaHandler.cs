using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.GetArchivedPizza;

internal sealed class GetArchivedPizzaHandler : IRequestHandler<GetArchivedPizzaQuery>
{
    private readonly IMapper _mapper;
    private readonly IPizzaPresenter _presenter;
    private readonly IPizzaRepository _repository;

    public GetArchivedPizzaHandler(
        IMapper mapper,
        IPizzaPresenter presenter,
        IPizzaRepository repository
    )
    {
        this._mapper = mapper;
        this._presenter = presenter;
        this._repository = repository;
    }

    public async Task Handle(
        GetArchivedPizzaQuery query,
        CancellationToken cancellationToken
    )
    {
        Pizza? item = await this._repository.FindAsync(
            query.Id,
            cancellationToken
        );

        if (item?.ArchivedAt is null)
        {
            this._presenter.EntityNotFound(
                $"Archived pizza with id {query.Id} was not found."
            );
            return;
        }

        this._presenter.EntityFound(
            this._mapper.Map<ArchivedPizzaDto>(item)
        );
    }
}