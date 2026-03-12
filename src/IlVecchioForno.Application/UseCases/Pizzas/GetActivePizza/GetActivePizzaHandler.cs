using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Application.UseCases.Pizzas.Presenters;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.GetActivePizza;

internal sealed class GetActivePizzaHandler : IRequestHandler<GetActivePizzaQuery>
{
    private readonly IMapper _mapper;
    private readonly IPizzaPresenter _presenter;
    private readonly IPizzaRepository _repository;
    private readonly IValidator<GetActivePizzaQuery> _validator;

    public GetActivePizzaHandler(
        IMapper mapper,
        IPizzaPresenter presenter,
        IPizzaRepository repository,
        IValidator<GetActivePizzaQuery> validator
    )
    {
        this._mapper = mapper;
        this._presenter = presenter;
        this._repository = repository;
        this._validator = validator;
    }

    public async Task Handle(
        GetActivePizzaQuery query,
        CancellationToken cancellationToken
    )
    {
        ValidationResult validationResult = await this._validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
        {
            this._presenter.ValidationErrors(
                validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )
            );
            return;
        }

        Pizza? item = await this._repository.FindAsync(
            query.Id,
            cancellationToken
        );

        if (item is null || item.ArchivedAt is not null)
        {
            this._presenter.EntityNotFound(
                $"Active pizza with id {query.Id} was not found."
            );
            return;
        }

        this._presenter.EntityFound(
            this._mapper.Map<ActivePizzaDto>(item)
        );
    }
}