using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Ingredients;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.GetIngredient;

internal sealed class GetIngredientHandler : IRequestHandler<GetIngredientQuery>
{
    private readonly IMapper _mapper;
    private readonly IIngredientPresenter _presenter;
    private readonly IIngredientRepository _repository;

    public GetIngredientHandler(
        IMapper mapper,
        IIngredientPresenter presenter,
        IIngredientRepository repository
    )
    {
        this._mapper = mapper;
        this._presenter = presenter;
        this._repository = repository;
    }

    public async Task Handle(
        GetIngredientQuery query,
        CancellationToken cancellationToken
    )
    {
        Ingredient? item = await this._repository.FindAsync(
            query.Id,
            cancellationToken
        );

        if (item is null)
        {
            this._presenter.EntityNotFound(
                $"Ingredient with id {query.Id} was not found."
            );
            return;
        }

        this._presenter.EntityFound(
            this._mapper.Map<IngredientDto>(item)
        );
    }
}