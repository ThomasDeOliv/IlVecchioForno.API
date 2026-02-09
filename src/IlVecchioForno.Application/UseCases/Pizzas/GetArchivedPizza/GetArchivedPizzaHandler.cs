using IlVecchioForno.Application.Common.Responses;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.GetArchivedPizza;

internal sealed class GetArchivedPizzaHandler : IRequestHandler<GetArchivedPizzaQuery, IResponse>
{
    private readonly IMapper _mapper;
    private readonly IPizzaRepository _repository;

    public GetArchivedPizzaHandler(IMapper mapper, IPizzaRepository repository)
    {
        this._mapper = mapper;
        this._repository = repository;
    }

    public async Task<IResponse> Handle(GetArchivedPizzaQuery query, CancellationToken cancellationToken)
    {
        Pizza? item = await this._repository.FindAsync(
            query.Id,
            cancellationToken
        );

        if (item?.ArchivedAt is null)
            return new ErrorResponseWithMessage(
                ErrorResponseType.EntityNotFoundError,
                $"Archived pizza with id {query.Id} was not found."
            );

        return new Response<ArchivedPizzaDto>(
            ResponseType.Query,
            this._mapper.Map<ArchivedPizzaDto>(item)
        );
    }
}