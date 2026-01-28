using FluentValidation;
using IlVecchioForno.Application.Common.Exceptions;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.GetActivePizza;

internal sealed class GetActivePizzaHandler : IRequestHandler<GetActivePizzaQuery, ActivePizzaDto>
{
    private readonly IMapper _mapper;
    private readonly IPizzaRepository _repository;
    private readonly IValidator<GetActivePizzaQuery> _validator;

    public GetActivePizzaHandler(
        IMapper mapper,
        IPizzaRepository repository,
        IValidator<GetActivePizzaQuery> validator
    )
    {
        this._mapper = mapper;
        this._repository = repository;
        this._validator = validator;
    }

    public async Task<ActivePizzaDto> Handle(GetActivePizzaQuery query, CancellationToken cancellationToken)
    {
        await this._validator.ValidateAndThrowAsync(query, cancellationToken);

        Pizza? item = await this._repository.FindAsync(
            query.Id,
            cancellationToken
        );

        if (item is null || item.ArchivedAt is not null)
            throw new EntityNotFoundException($"Active pizza with id {query.Id} was not found.");

        return this._mapper.Map<ActivePizzaDto>(item);
    }
}