using IlVecchioForno.Application.Common.Exceptions;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.Ingredients.DTOs;
using IlVecchioForno.Domain.Ingredients;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.GetIngredient;

internal sealed class GetIngredientHandler : IRequestHandler<GetIngredientQuery, IngredientDto>
{
    private readonly IMapper _mapper;
    private readonly IIngredientRepository _repository;

    public GetIngredientHandler(IMapper mapper, IIngredientRepository repository)
    {
        this._mapper = mapper;
        this._repository = repository;
    }

    public async Task<IngredientDto> Handle(GetIngredientQuery query, CancellationToken cancellationToken)
    {
        Ingredient? item = await this._repository.FindAsync(
            query.Id,
            cancellationToken
        );

        return item is null
            ? throw new EntityNotFoundException($"Ingredient with id {query.Id} was not found.")
            : this._mapper.Map<IngredientDto>(item);
    }
}