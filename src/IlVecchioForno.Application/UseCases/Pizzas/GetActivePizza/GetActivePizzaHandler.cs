using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Common.Responses;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.GetActivePizza;

internal sealed class GetActivePizzaHandler : IRequestHandler<GetActivePizzaQuery, IResponse>
{
    private readonly IMapper _mapper;
    private readonly IPizzaRepository _repository;
    private readonly IValidator<GetActivePizzaQuery> _validator;

    public GetActivePizzaHandler(
        IMapper mapper,
        IPizzaRepository repository,
        IValidator<GetActivePizzaQuery> validator
    )
    {
        this._mapper = mapper;
        this._repository = repository;
        this._validator = validator;
    }

    public async Task<IResponse> Handle(GetActivePizzaQuery query, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await this._validator.ValidateAsync(query, cancellationToken);

        if (!validationResult.IsValid)
            return new ErrorResponseWithMessages(
                validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )
            );

        Pizza? item = await this._repository.FindAsync(
            query.Id,
            cancellationToken
        );

        if (item is null || item.ArchivedAt is not null)
            return new ErrorResponseWithMessage(
                ErrorResponseType.EntityNotFoundError,
                $"Active pizza with id {query.Id} was not found."
            );

        return new Response<ActivePizzaDto>(
            ResponseType.Query,
            this._mapper.Map<ActivePizzaDto>(item)
        );
    }
}