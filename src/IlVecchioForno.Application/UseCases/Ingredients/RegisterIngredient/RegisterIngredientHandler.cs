using FluentValidation;
using IlVecchioForno.Application.Common.Exceptions;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.QuantityTypes;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.RegisterIngredient;

internal sealed class RegisterIngredientHandler : IRequestHandler<RegisterIngredientCommand, int>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IQuantityTypeRepository _quantityTypeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RegisterIngredientCommand> _validator;

    public RegisterIngredientHandler(
        IIngredientRepository ingredientRepository,
        IQuantityTypeRepository quantityTypeRepository,
        IUnitOfWork unitOfWork,
        IValidator<RegisterIngredientCommand> validator
    )
    {
        this._ingredientRepository = ingredientRepository;
        this._quantityTypeRepository = quantityTypeRepository;
        this._unitOfWork = unitOfWork;
        this._validator = validator;
    }

    public async Task<int> Handle(RegisterIngredientCommand request, CancellationToken cancellationToken)
    {
        await this._validator.ValidateAndThrowAsync(request, cancellationToken);

        QuantityType? targetQuantityType =
            await this._quantityTypeRepository.FindAsync(request.QuantityTypeId, cancellationToken);

        if (targetQuantityType is null)
            throw new InvalidReferenceException("Provided quantity type not found.");

        Ingredient newIngredient = new Ingredient(
            new IngredientName(request.Name),
            targetQuantityType
        );
        this._ingredientRepository.Add(newIngredient);
        await this._unitOfWork.SaveChangesAsync(cancellationToken);

        return newIngredient.Id;
    }
}