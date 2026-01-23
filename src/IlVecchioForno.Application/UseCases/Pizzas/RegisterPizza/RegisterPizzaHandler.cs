using IlVecchioForno.Application.Common.Exceptions;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.PizzaIngredients;
using IlVecchioForno.Domain.Pizzas;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.RegisterPizza;

internal sealed class RegisterPizzaHandler : IRequestHandler<RegisterPizzaCommand, int>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterPizzaHandler(
        IIngredientRepository ingredientRepository,
        IPizzaRepository pizzaRepository,
        IUnitOfWork unitOfWork
    )
    {
        this._ingredientRepository = ingredientRepository;
        this._pizzaRepository = pizzaRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task<int> Handle(RegisterPizzaCommand request, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Ingredient> targetIngredients = await this._ingredientRepository
            .ResolveAsync(request.IngredientsAndQuantities.Keys, cancellationToken);

        if (targetIngredients.Count != request.IngredientsAndQuantities.Count)
            throw new InvalidReferenceException("Some ingredients were not found.");

        List<PizzaIngredient> pizzaIngredients = targetIngredients
            .Select(t => new PizzaIngredient(
                    new PizzaIngredientQuantity(request.IngredientsAndQuantities[t.Id]),
                    t
                )
            ).ToList();

        Pizza pizza = new Pizza(
            new PizzaName(request.Name),
            !string.IsNullOrEmpty(request.Description) ? new PizzaDescription(request.Description) : null,
            new PizzaPrice(request.Price)
        );

        pizza.UpdateIngredients(pizzaIngredients);
        this._pizzaRepository.Add(pizza);
        int result = await this._unitOfWork.SaveChangesAsync(cancellationToken);

        return result != 0
            ? pizza.Id
            : throw new EntityRegistrationException("Cannot register pizza.");
    }
}