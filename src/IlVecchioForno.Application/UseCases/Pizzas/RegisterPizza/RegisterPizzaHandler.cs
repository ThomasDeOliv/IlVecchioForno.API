using IlVecchioForno.Application.Common.Responses;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.PizzaIngredients;
using IlVecchioForno.Domain.Pizzas;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Pizzas.RegisterPizza;

internal sealed class RegisterPizzaHandler : IRequestHandler<RegisterPizzaCommand, IResponse>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IMapper _mapper;
    private readonly IPizzaRepository _pizzaRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RegisterPizzaHandler(
        IIngredientRepository ingredientRepository,
        IMapper mapper,
        IPizzaRepository pizzaRepository,
        IUnitOfWork unitOfWork
    )
    {
        this._ingredientRepository = ingredientRepository;
        this._mapper = mapper;
        this._pizzaRepository = pizzaRepository;
        this._unitOfWork = unitOfWork;
    }

    public async Task<IResponse> Handle(RegisterPizzaCommand request, CancellationToken cancellationToken)
    {
        IReadOnlyCollection<Ingredient> targetIngredients = await this._ingredientRepository
            .ResolveAsync(request.IngredientsAndQuantities.Keys, cancellationToken);

        if (targetIngredients.Count != request.IngredientsAndQuantities.Count)
            return new ResponseWithErrorMessage(
                ErrorMessageType.InvalidReferenceError,
                "Some ingredients were not found."
            );

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

        if (result == 0)
            return new ResponseWithErrorMessage(
                ErrorMessageType.EntityRegistrationError,
                "Cannot register pizza."
            );

        return new ResponseForCommand<ActivePizzaDto>(
            this._mapper.Map<ActivePizzaDto>(pizza)
        );
    }
}