using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.Ingredients.DTOs;
using IlVecchioForno.Application.UseCases.Ingredients.Presenters;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.QuantityTypes;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.RegisterIngredient;

internal sealed class RegisterIngredientHandler : IRequestHandler<RegisterIngredientCommand>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IMapper _mapper;
    private readonly IIngredientPresenter _presenter;
    private readonly IQuantityTypeRepository _quantityTypeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RegisterIngredientCommand> _validator;

    public RegisterIngredientHandler(
        IIngredientPresenter presenter,
        IIngredientRepository ingredientRepository,
        IMapper mapper,
        IQuantityTypeRepository quantityTypeRepository,
        IUnitOfWork unitOfWork,
        IValidator<RegisterIngredientCommand> validator
    )
    {
        this._presenter = presenter;
        this._ingredientRepository = ingredientRepository;
        this._mapper = mapper;
        this._quantityTypeRepository = quantityTypeRepository;
        this._unitOfWork = unitOfWork;
        this._validator = validator;
    }

    public async Task Handle(
        RegisterIngredientCommand command,
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

        QuantityType? targetQuantityType = null;

        if (command.QuantityTypeId.HasValue)
        {
            targetQuantityType =
                await this._quantityTypeRepository.FindAsync(command.QuantityTypeId.Value, cancellationToken);

            if (targetQuantityType is null)
            {
                this._presenter.InvalidReferenceError(
                    "Provided quantity type not found."
                );

                return;
            }
        }

        Ingredient newIngredient = new Ingredient(
            new IngredientName(command.Name),
            targetQuantityType
        );

        this._ingredientRepository.Add(newIngredient);

        if (await this._unitOfWork.SaveChangesAsync(cancellationToken) == 0)
        {
            this._presenter.RegistrationError("Cannot register ingredient.");
            return;
        }

        this._presenter.EntityRegistered(
            this._mapper.Map<IngredientDto>(newIngredient)
        );
    }
}