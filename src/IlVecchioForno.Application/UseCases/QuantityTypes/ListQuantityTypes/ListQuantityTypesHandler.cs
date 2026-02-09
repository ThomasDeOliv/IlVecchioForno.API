using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Common.Responses;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;
using IlVecchioForno.Domain.QuantityTypes;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.ListQuantityTypes;

internal sealed class
    ListQuantityTypesHandler : IRequestHandler<ListQuantityTypesQuery, IResponse>
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

    public async Task<IResponse> Handle(
        ListQuantityTypesQuery request,
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

        return new Response<IReadOnlyList<QuantityTypeDto>>(
            ResponseType.Query,
            this._mapper.Map<IReadOnlyList<QuantityTypeDto>>(items)
        );
    }
}