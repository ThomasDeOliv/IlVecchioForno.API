using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.QuantityTypes;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.ListQuantityTypes;

internal sealed class ListQuantityTypesHandler : IRequestHandler<ListQuantityTypesQuery, Result<IReadOnlyList<QuantityTypeDTO>>>
{
    private readonly IMapper _mapper;
    private readonly IQuantityTypeRepository _repository;
    private readonly IValidator<ListQuantityTypesQuery> _validator;

    public ListQuantityTypesHandler(
        IMapper mapper,
        IQuantityTypeRepository repository,
        IValidator<ListQuantityTypesQuery> validator
    )
    {
        this._mapper = mapper;
        this._repository = repository;
        this._validator = validator;
    }

    public async Task<Result<IReadOnlyList<QuantityTypeDTO>>> Handle(
        ListQuantityTypesQuery request,
        CancellationToken cancellationToken = default
    )
    {
        ValidationResult validation = await this._validator.ValidateAsync(request, cancellationToken);

        if (!validation.IsValid)
            return Result<IReadOnlyList<QuantityTypeDTO>>.ValidationError(
                string.Join("\n", validation.Errors.Select(e => e.ErrorMessage))
            );

        QuerySpec<QuantityTypesSorter> querySpec =
            new QuerySpec<QuantityTypesSorter>(
                request.Page,
                request.PageSize,
                request.Sorter,
                request.Descending,
                new List<IFilterType>
                {
                    new SearchFilterType(request.Search)
                }
            );

        IReadOnlyCollection<QuantityType> items = await this._repository.ListAsync(
            querySpec,
            cancellationToken
        );

        IReadOnlyList<QuantityTypeDTO> result = this._mapper.Map<IReadOnlyList<QuantityTypeDTO>>(items);

        return Result<IReadOnlyList<QuantityTypeDTO>>.Ok(result);
    }
}