using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.Pizzas.Presenters;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.PizzaIngredients;
using IlVecchioForno.Domain.Pizzas;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.ChangePizzaDetails;

internal sealed class ChangePizzaDetailsHandler : IRequestHandler<ChangePizzaDetailsCommand>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IPizzaPresenter _presenter;
    private readonly IUnitOfWork _unitOfWork;

    public ChangePizzaDetailsHandler(
        IIngredientRepository ingredientRepository,
        IPizzaPresenter presenter,
        IPizzaRepository pizzaRepository,
        IUnitOfWork unitOfWork
    )
    {
        this._ingredientRepository = ingredientRepository;
        this._presenter = presenter;
        this._pizzaRepository = pizzaRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task Handle(
        ChangePizzaDetailsCommand command,
        CancellationToken cancellationToken
    )
    {
        Pizza? target = await this._pizzaRepository.FindAsync(command.Id, cancellationToken);

        if (target is null)
        {
            this._presenter.InvalidReferenceError("Pizza not found.");
            return;
        }

        IReadOnlyCollection<Ingredient> targetIngredients =
            await this._ingredientRepository.ResolveAsync(command.IngredientsAndQuantities.Keys, cancellationToken);

        if (targetIngredients.Count != command.IngredientsAndQuantities.Count)
        {
            this._presenter.InvalidReferenceError("Some ingredients were not found.");
            return;
        }

        List<PizzaIngredient> pizzaIngredients = targetIngredients
            .Select(t =>
                new PizzaIngredient(
                    new PizzaIngredientQuantity(command.IngredientsAndQuantities[t.Id]),
                    t
                )
            )
            .ToList();

        target.UpdateIngredients(pizzaIngredients);
        target.UpdateDescription(
            !string.IsNullOrEmpty(command.Description)
                ? new PizzaDescription(command.Description)
                : null
        );
        target.UpdatePrice(new PizzaPrice(command.Price));
        await this._unitOfWork.SaveChangesAsync(cancellationToken);

        this._presenter.EntityUpdated();
    }
}