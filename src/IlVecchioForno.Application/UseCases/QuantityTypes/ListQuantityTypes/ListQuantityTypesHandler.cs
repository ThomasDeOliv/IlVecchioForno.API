using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.QuantityTypes;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.ListQuantityTypes;

internal sealed class
    ListQuantityTypesHandler : IRequestHandler<ListQuantityTypesQuery>
{
    private readonly IMapper _mapper;
    private readonly IQuantityTypePresenter _presenter;
    private readonly IQuantityTypeRepository _repository;
    private readonly IValidator<ListQuantityTypesQuery> _validator;

    public ListQuantityTypesHandler(
        IMapper mapper,
        IQuantityTypePresenter presenter,
        IQuantityTypeRepository repository,
        IValidator<ListQuantityTypesQuery> validator
    )
    {
        this._mapper = mapper;
        this._presenter = presenter;
        this._repository = repository;
        this._validator = validator;
    }

    public async Task Handle(
        ListQuantityTypesQuery query,
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

        ListQuerySpec<QuantityTypesSorter> querySpec =
            new ListQuerySpec<QuantityTypesSorter>(
                query.Page,
                query.PageSize,
                query.Sorter,
                query.Descending,
                new List<IFilterType>
                {
                    new SearchFilterType(query.Search)
                }
            );

        IReadOnlyCollection<QuantityType> items = await this._repository.ListAsync(
            querySpec,
            cancellationToken
        );

        this._presenter.EntitiesListed(
            this._mapper.Map<IReadOnlyList<QuantityTypeDto>>(items)
        );
    }
}