using IlVecchioForno.Application.Common.Exceptions;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.GetArchivedPizza;

internal sealed class GetArchivedPizzaHandler : IRequestHandler<GetArchivedPizzaQuery, ArchivedPizzaDto>
{
    private readonly IMapper _mapper;
    private readonly IPizzaRepository _repository;

    public GetArchivedPizzaHandler(IMapper mapper, IPizzaRepository repository)
    {
        this._mapper = mapper;
        this._repository = repository;
    }

    public async Task<ArchivedPizzaDto> Handle(GetArchivedPizzaQuery query, CancellationToken cancellationToken)
    {
        Pizza? item = await this._repository.FindAsync(
            query.Id,
            cancellationToken
        );

        return item?.ArchivedAt is null
            ? throw new EntityNotFoundException($"Archived pizza with id {query.Id} was not found.")
            : this._mapper.Map<ArchivedPizzaDto>(item);
    }
}