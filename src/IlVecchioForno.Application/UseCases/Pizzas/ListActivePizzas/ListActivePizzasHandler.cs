using FluentValidation;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ListActivePizzas;

internal sealed class
    ListActivePizzasHandler : IRequestHandler<ListActivePizzasQuery, IReadOnlyList<ActivePizzaDto>>
{
    private readonly IMapper _mapper;
    private readonly IPizzaRepository _pizzaRepository;
    private IValidator<ListActivePizzasQuery> _validator;

    public ListActivePizzasHandler(
        IMapper mapper,
        IPizzaRepository pizzaRepository,
        IValidator<ListActivePizzasQuery> validator
    )
    {
        this._mapper = mapper;
        this._pizzaRepository = pizzaRepository;
        this._validator = validator;
    }

    public async Task<IReadOnlyList<ActivePizzaDto>> Handle(
        ListActivePizzasQuery request,
        CancellationToken cancellationToken = default
    )
    {
        await this._validator.ValidateAndThrowAsync(request, cancellationToken);

        QuerySpec<ActivePizzasSorter> query = new QuerySpec<ActivePizzasSorter>(
            request.Page,
            request.PageSize,
            request.Sorter,
            request.Descending,
            new List<IFilterType>
            {
                new RangeFilterType<decimal>(request.MinPrice, request.MaxPrice),
                new SearchFilterType(request.Search)
            }
        );

        IReadOnlyCollection<Pizza> items = await this._pizzaRepository.ListActiveAsync(
            query,
            cancellationToken
        );

        IReadOnlyList<ActivePizzaDto> result = this._mapper.Map<IReadOnlyList<ActivePizzaDto>>(items);

        return this._mapper.Map<IReadOnlyList<ActivePizzaDto>>(result);
    }
}