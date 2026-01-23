using FluentValidation;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Application.UseCases.Ingredients.DTOs;
using IlVecchioForno.Domain.Ingredients;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.ListIngredients;

internal sealed class ListIngredientsHandler
    : IRequestHandler<ListIngredientsQuery, IReadOnlyList<IngredientDto>>
{
    private readonly IMapper _mapper;
    private readonly IIngredientRepository _repository;
    private readonly IValidator<ListIngredientsQuery> _validator;

    public ListIngredientsHandler(
        IIngredientRepository repository,
        IValidator<ListIngredientsQuery> validator,
        IMapper mapper
    )
    {
        this._repository = repository;
        this._validator = validator;
        this._mapper = mapper;
    }

    public async Task<IReadOnlyList<IngredientDto>> Handle(
        ListIngredientsQuery request,
        CancellationToken cancellationToken = default
    )
    {
        await this._validator.ValidateAndThrowAsync(request, cancellationToken);

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

        return this._mapper.Map<IReadOnlyList<IngredientDto>>(items);
    }
}