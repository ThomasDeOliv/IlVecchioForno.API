using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Application.Gateways.Presentation;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ListActivePizzas;

internal sealed class
    ListActivePizzasHandler : IRequestHandler<ListActivePizzasQuery>
{
    private readonly IMapper _mapper;
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IPizzaPresenter _presenter;
    private readonly IValidator<ListActivePizzasQuery> _validator;

    public ListActivePizzasHandler(
        IMapper mapper,
        IPizzaPresenter presenter,
        IPizzaRepository pizzaRepository,
        IValidator<ListActivePizzasQuery> validator
    )
    {
        this._mapper = mapper;
        this._presenter = presenter;
        this._pizzaRepository = pizzaRepository;
        this._validator = validator;
    }

    public async Task Handle(
        ListActivePizzasQuery query,
        CancellationToken cancellationToken = default
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

        ListQuerySpec<ActivePizzasSorter> querySpec = new ListQuerySpec<ActivePizzasSorter>(
            query.Page,
            query.PageSize,
            query.Sorter,
            query.Descending,
            new List<IFilterType>
            {
                new RangeFilterType<decimal>(query.MinPrice, query.MaxPrice),
                new SearchFilterType(query.Search)
            }
        );

        IReadOnlyCollection<Pizza> items = await this._pizzaRepository.ListActiveAsync(
            querySpec,
            cancellationToken
        );

        this._presenter.EntitiesListed(
            this._mapper.Map<IReadOnlyList<ActivePizzaDto>>(items)
        );
    }
}