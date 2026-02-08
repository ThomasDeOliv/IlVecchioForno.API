using FluentValidation;
using FluentValidation.Results;
using IlVecchioForno.Application.Common.Responses;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.UseCases.Ingredients.DTOs;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.QuantityTypes;
using MapsterMapper;
using MediatR;

namespace IlVecchioForno.Application.UseCases.Ingredients.RegisterIngredient;

internal sealed class RegisterIngredientHandler : IRequestHandler<RegisterIngredientCommand, IResponse>
{
    private readonly IIngredientRepository _ingredientRepository;
    private readonly IMapper _mapper;
    private readonly IQuantityTypeRepository _quantityTypeRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<RegisterIngredientCommand> _validator;

    public RegisterIngredientHandler(
        IIngredientRepository ingredientRepository,
        IMapper mapper,
        IQuantityTypeRepository quantityTypeRepository,
        IUnitOfWork unitOfWork,
        IValidator<RegisterIngredientCommand> validator
    )
    {
        this._ingredientRepository = ingredientRepository;
        this._mapper = mapper;
        this._quantityTypeRepository = quantityTypeRepository;
        this._unitOfWork = unitOfWork;
        this._validator = validator;
    }

    public async Task<IResponse> Handle(RegisterIngredientCommand request, CancellationToken cancellationToken)
    {
        ValidationResult validationResult = await this._validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
            return new ResponseWithErrorMessages(
                validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    )
            );

        QuantityType? targetQuantityType = null;

        if (request.QuantityTypeId.HasValue)
        {
            targetQuantityType =
                await this._quantityTypeRepository.FindAsync(request.QuantityTypeId.Value, cancellationToken);

            if (targetQuantityType is null)
                return new ResponseWithErrorMessage(
                    ErrorMessageType.InvalidReferenceError,
                    "Provided quantity type not found."
                );
        }

        Ingredient newIngredient = new Ingredient(
            new IngredientName(request.Name),
            targetQuantityType
        );
        this._ingredientRepository.Add(newIngredient);
        await this._unitOfWork.SaveChangesAsync(cancellationToken);

        return new ResponseForCommand<IngredientDto>(
            this._mapper.Map<IngredientDto>(newIngredient)
        );
    }
}