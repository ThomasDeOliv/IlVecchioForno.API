using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Common.Responses;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ListArchivedPizzas;

internal sealed class
    ListArchivedPizzasHandler : IRequestHandler<ListArchivedPizzasQuery, IResponse>
{
    private readonly IMapper _mapper;
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IValidator<ListArchivedPizzasQuery> _validator;

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

    public async Task<IResponse> Handle(
        ListArchivedPizzasQuery request,
        CancellationToken cancellationToken = default
    )
    {
        ValidationResult validationResult = await this._validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return new ErrorResponseWithMessages(
                validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )
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

        return new Response<IReadOnlyList<ArchivedPizzaDto>>(
            ResponseType.Query,
            this._mapper.Map<IReadOnlyList<ArchivedPizzaDto>>(items)
        );
    }
}