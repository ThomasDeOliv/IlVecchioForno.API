using FluentValidation;
using IlVecchioForno.Domain.Ingredients;

namespace IlVecchioForno.Application.UseCases.Ingredients.RegisterIngredient;

internal sealed class RegisterIngredientValidator : AbstractValidator<RegisterIngredientCommand>
{
    public RegisterIngredientValidator()
    {
        this.RuleFor(c => c.Name)
            .Cascade(CascadeMode.Stop)
            .NotNull()
            .WithMessage(
                "Name cannot be null."
            )
            .NotEmpty()
            .WithMessage(
                "Name cannot be empty."
            )
            .Length(IngredientInvariant.NameMinLength, IngredientInvariant.NameMaxLength)
            .WithMessage(
                $"Name size must be within the allowed range ({IngredientInvariant.NameMinLength} – {IngredientInvariant.NameMaxLength})."
            );
    }
}