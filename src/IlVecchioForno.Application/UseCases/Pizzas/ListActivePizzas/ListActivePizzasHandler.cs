using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ListActivePizzas;

internal sealed class
    ListActivePizzasHandler : IRequestHandler<ListActivePizzasQuery, Result<IReadOnlyList<ActivePizzaDTO>>>
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

    public async Task<Result<IReadOnlyList<ActivePizzaDTO>>> Handle(
        ListActivePizzasQuery request,
        CancellationToken cancellationToken = default
    )
    {
        ValidationResult validation = await this._validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
            return Result<IReadOnlyList<ActivePizzaDTO>>.ValidationError(
                string.Join("\n", validation.Errors.Select(e => e.ErrorMessage))
            );

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

        IReadOnlyList<ActivePizzaDTO> result = this._mapper.Map<IReadOnlyList<ActivePizzaDTO>>(items);
        return Result<IReadOnlyList<ActivePizzaDTO>>.Ok(result);
    }
}