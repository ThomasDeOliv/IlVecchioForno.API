using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.GetActivePizza;

internal sealed class GetActivePizzaHandler : IRequestHandler<GetActivePizzaQuery, ActivePizzaDto?>
{
    private readonly IMapper _mapper;
    private readonly IPizzaRepository _repository;

    public GetActivePizzaHandler(IMapper mapper, IPizzaRepository repository)
    {
        this._mapper = mapper;
        this._repository = repository;
    }

    public async Task<ActivePizzaDto?> Handle(GetActivePizzaQuery query, CancellationToken cancellationToken)
    {
        Pizza? item = await this._repository.FindAsync(
            query.Id,
            cancellationToken
        );

        if (item is null || item.ArchivedAt is not null)
            return null;

        return this._mapper.Map<ActivePizzaDto>(item);
    }
}