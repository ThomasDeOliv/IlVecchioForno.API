using IlVecchioForno.Application.Common.Responses;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.PizzaIngredients;
using IlVecchioForno.Domain.Pizzas;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ChangePizzaDetails;

internal sealed class ChangePizzaDetailsHandler : IRequestHandler<ChangePizzaDetailsCommand, IResponse>
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

    public async Task<IResponse> Handle(ChangePizzaDetailsCommand request, CancellationToken cancellationToken)
    {
        Pizza? target = await this._pizzaRepository.FindAsync(request.Id, cancellationToken);

        if (target is null)
            return new ResponseWithErrorMessage(
                ErrorMessageType.InvalidReferenceError,
                "Pizza not found."
            );

        IReadOnlyCollection<Ingredient> targetIngredients =
            await this._ingredientRepository.ResolveAsync(request.IngredientsAndQuantities.Keys, cancellationToken);

        if (targetIngredients.Count != request.IngredientsAndQuantities.Count)
            return new ResponseWithErrorMessage(
                ErrorMessageType.InvalidReferenceError,
                "Some ingredients were not found."
            );

        List<PizzaIngredient> pizzaIngredients = targetIngredients
            .Select(t =>
                new PizzaIngredient(
                    new PizzaIngredientQuantity(request.IngredientsAndQuantities[t.Id]),
                    t
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
        await this._unitOfWork.SaveChangesAsync(cancellationToken);
        return new ResponseForCommand<Unit>(Unit.Value);
    }
}