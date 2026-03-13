using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Ingredients;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.ListIngredients;

internal sealed class ListIngredientsHandler : IRequestHandler<ListIngredientsQuery>
{
    private readonly IMapper _mapper;
    private readonly IIngredientPresenter _presenter;
    private readonly IIngredientRepository _repository;
    private readonly IValidator<ListIngredientsQuery> _validator;

    public ListIngredientsHandler(
        IIngredientPresenter presenter,
        IIngredientRepository repository,
        IValidator<ListIngredientsQuery> validator,
        IMapper mapper
    )
    {
        this._presenter = presenter;
        this._repository = repository;
        this._validator = validator;
        this._mapper = mapper;
    }

    public async Task Handle(
        ListIngredientsQuery query,
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

        ListQuerySpec<IngredientsSorter> querySpec = new ListQuerySpec<IngredientsSorter>(
            query.Page,
            query.PageSize,
            query.Sorter,
            query.Descending,
            new List<IFilterType>
            {
                new SearchFilterType(query.Search)
            }
        );

        IReadOnlyCollection<Ingredient> items = await this._repository.ListAsync(
            querySpec,
            cancellationToken
        );

        this._presenter.EntitiesListed(
            this._mapper.Map<IReadOnlyList<IngredientDto>>(items)
        );
    }
}