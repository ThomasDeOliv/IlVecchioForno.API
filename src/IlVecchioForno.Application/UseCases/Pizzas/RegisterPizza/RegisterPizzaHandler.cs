using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.PizzaIngredients;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.RegisterPizza;

internal sealed class RegisterPizzaHandler : IRequestHandler<RegisterPizzaCommand>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IMapper _mapper;
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IPizzaPresenter _presenter;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RegisterPizzaCommand> _validator;

    public RegisterPizzaHandler(
        IIngredientRepository ingredientRepository,
        IMapper mapper,
        IPizzaPresenter presenter,
        IPizzaRepository pizzaRepository,
        IUnitOfWork unitOfWork,
        IValidator<RegisterPizzaCommand> validator
    )
    {
        this._ingredientRepository = ingredientRepository;
        this._mapper = mapper;
        this._presenter = presenter;
        this._pizzaRepository = pizzaRepository;
        this._unitOfWork = unitOfWork;
        this._validator = validator;
    }

    public async Task Handle(
        RegisterPizzaCommand command,
        CancellationToken cancellationToken
    )
    {
        ValidationResult validationResult = await this._validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            this._presenter.ValidationErrors(
                validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )
            );
            return;
        }

        IReadOnlyCollection<Ingredient> targetIngredients = await this._ingredientRepository
            .ResolveAsync(command.IngredientsAndQuantities.Keys, cancellationToken);

        if (targetIngredients.Count != command.IngredientsAndQuantities.Count)
        {
            this._presenter.InvalidReferenceError("Some ingredients were not found.");
            return;
        }

        List<PizzaIngredient> pizzaIngredients = targetIngredients
            .Select(t => new PizzaIngredient(
                    command.IngredientsAndQuantities[t.Id],
                    t
                )
            ).ToList();

        Pizza pizza = new Pizza(
            command.Name,
            !string.IsNullOrEmpty(command.Description)
                ? new PizzaDescription(command.Description)
                : null,
            command.Price
        );

        pizza.UpdateIngredients(pizzaIngredients);
        this._pizzaRepository.Add(pizza);

        if (await this._unitOfWork.SaveChangesAsync(cancellationToken) == 0)
        {
            this._presenter.RegistrationError("Cannot register pizza.");
            return;
        }

        this._presenter.EntityRegistered(
            this._mapper.Map<ActivePizzaDto>(pizza)
        );
    }
}