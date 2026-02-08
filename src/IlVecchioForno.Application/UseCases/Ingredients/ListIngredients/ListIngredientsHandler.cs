using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Common.Responses;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Application.UseCases.Ingredients.DTOs;
using IlVecchioForno.Domain.Ingredients;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.ListIngredients;

internal sealed class ListIngredientsHandler
    : IRequestHandler<ListIngredientsQuery, IResponse>
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

    public async Task<IResponse> Handle(
        ListIngredientsQuery request,
        CancellationToken cancellationToken = default
    )
    {
        ValidationResult validationResult = await this._validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return new ResponseWithErrorMessages(
                validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )
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

        return new ResponseForQuery<IReadOnlyList<IngredientDto>>(
            this._mapper.Map<IReadOnlyList<IngredientDto>>(items)
        );
    }
}