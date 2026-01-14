using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.PizzaIngredients;
using IlVecchioForno.Domain.Pizzas;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ChangePizzaDetails;

internal sealed class ChangePizzaDetailsHandler : IRequestHandler<ChangePizzaDetailsCommand, Result<int>>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePizzaDetailsHandler(
        IPizzaRepository pizzaRepository,
        IIngredientRepository ingredientRepository,
        IUnitOfWork unitOfWork
    )
    {
        this._pizzaRepository = pizzaRepository;
        this._ingredientRepository = ingredientRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task<Result<int>> Handle(ChangePizzaDetailsCommand request, CancellationToken cancellationToken)
    {
        Pizza? target = await this._pizzaRepository.FindAsync(request.Id, cancellationToken);

        if (target is null)
            return Result<int>.ValidationError($"Cannot find pizza with id {request.Id}");

        IReadOnlyCollection<Ingredient> targetIngredients =
            await this._ingredientRepository.ResolveAsync(request.IngredientsAndQuantities.Keys, cancellationToken);

        List<PizzaIngredient> pizzaIngredients = targetIngredients
            .Select(t =>
                new PizzaIngredient(
                    new PizzaIngredientQuantity(request.IngredientsAndQuantities[t.Id]),
                    targetIngredients.Single(e => e.Id == t.Id)
                )
            )
            .ToList();

        target.UpdateIngredients(pizzaIngredients);
        target.UpdateDescription(
            !string.IsNullOrEmpty(request.Description)
                ? new PizzaDescription(request.Description)
                : null
        );
        target.UpdatePrice(new PizzaPrice(request.Price));
        int result = await this._unitOfWork.SaveChangesAsync(cancellationToken);
        return Result<int>.Ok(result);
    }
}