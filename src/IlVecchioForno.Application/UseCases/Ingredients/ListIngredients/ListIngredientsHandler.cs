using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Ingredients;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.ListIngredients;

internal sealed class
    ListIngredientsHandler : IRequestHandler<ListIngredientsQuery, Result<IReadOnlyList<IngredientDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IIngredientRepository _repository;
    private readonly IValidator<ListIngredientsQuery> _validator;

    public ListIngredientsHandler(
        IMapper mapper,
        IIngredientRepository repository,
        IValidator<ListIngredientsQuery> validator
    )
    {
        this._mapper = mapper;
        this._repository = repository;
        this._validator = validator;
    }

    public async Task<Result<IReadOnlyList<IngredientDTO>>> Handle(
        ListIngredientsQuery request,
        CancellationToken cancellationToken = default
    )
    {
        ValidationResult? validation = await this._validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
            return Result<IReadOnlyList<IngredientDTO>>.ValidationError(
                string.Join("\n", validation.Errors.Select(e => e.ErrorMessage))
            );

        QuerySpec<IngredientsSorter> query = new QuerySpec<IngredientsSorter>(
            request.Page,
            request.PageSize,
            request.Sorter,
            request.Descending,
            new List<IFilterType>
            {
                new SearchFilterType(request.Search)
            }
        );

        IReadOnlyCollection<Ingredient> items = await this._repository.ListAsync(
            query,
            cancellationToken
        );

        IReadOnlyList<IngredientDTO> result = this._mapper.Map<IReadOnlyList<IngredientDTO>>(items);

        return Result<IReadOnlyList<IngredientDTO>>.Ok(result);
    }
}