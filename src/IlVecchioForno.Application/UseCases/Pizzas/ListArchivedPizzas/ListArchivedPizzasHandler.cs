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

namespace IlVecchioForno.Application.UseCases.Pizzas.ListArchivedPizzas;

internal sealed class
    ListArchivedPizzasHandler : IRequestHandler<ListArchivedPizzasQuery, Result<IReadOnlyList<ArchivedPizzaDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IPizzaRepository _pizzaRepository;
    private IValidator<ListArchivedPizzasQuery> _validator;

    public ListArchivedPizzasHandler(
        IMapper mapper,
        IPizzaRepository pizzaRepository,
        IValidator<ListArchivedPizzasQuery> validator
    )
    {
        this._mapper = mapper;
        this._pizzaRepository = pizzaRepository;
        this._validator = validator;
    }

    public async Task<Result<IReadOnlyList<ArchivedPizzaDTO>>> Handle(
        ListArchivedPizzasQuery request,
        CancellationToken cancellationToken = default
    )
    {
        ValidationResult validation = await this._validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
            return Result<IReadOnlyList<ArchivedPizzaDTO>>.ValidationError(
                string.Join("\n", validation.Errors.Select(e => e.ErrorMessage))
            );

        QuerySpec<ArchivedPizzasSorter> query = new QuerySpec<ArchivedPizzasSorter>(
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

        IReadOnlyCollection<Pizza> items = await this._pizzaRepository.ListArchivedAsync(
            query,
            cancellationToken
        );

        IReadOnlyList<ArchivedPizzaDTO> result = this._mapper.Map<IReadOnlyList<ArchivedPizzaDTO>>(items);
        return Result<IReadOnlyList<ArchivedPizzaDTO>>.Ok(result);
    }
}