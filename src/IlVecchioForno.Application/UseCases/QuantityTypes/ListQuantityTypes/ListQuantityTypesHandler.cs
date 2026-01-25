using FluentValidation;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;
using IlVecchioForno.Domain.QuantityTypes;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.ListQuantityTypes;

internal sealed class ListQuantityTypesHandler : IRequestHandler<ListQuantityTypesQuery, IReadOnlyList<QuantityTypeDto>>
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

    public async Task<IReadOnlyList<QuantityTypeDto>> Handle(
        ListQuantityTypesQuery request,
        CancellationToken cancellationToken = default
    )
    {
        await this._validator.ValidateAndThrowAsync(request, cancellationToken);

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

        return this._mapper.Map<IReadOnlyList<QuantityTypeDto>>(items);
    }
}