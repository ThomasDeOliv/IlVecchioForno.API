using FluentValidation;
using IlVecchioForno.Domain.Pizzas;

namespace IlVecchioForno.Application.UseCases.Pizzas.RegisterPizza;

public class RegisterPizzaCommandValidator : AbstractValidator<RegisterPizzaCommand>
{
    public RegisterPizzaCommandValidator()
    {
        this.RuleFor(c => c.Name)
            .Cascade(CascadeMode.Stop)
            .Must(v => !string.IsNullOrWhiteSpace(v) && v == v.Trim())
            .WithMessage(
                "Name must not be null, empty, whitespace, or contain leading/trailing whitespace."
            )
            .Length(PizzaInvariant.NameMinLength, PizzaInvariant.NameMaxLength)
            .WithMessage(
                $"Name size must be within the allowed range ({PizzaInvariant.NameMinLength} – {PizzaInvariant.NameMaxLength})."
            );

        this.RuleFor(c => c.Description)
            .Cascade(CascadeMode.Stop)
            .Must(v => v is null || (!string.IsNullOrWhiteSpace(v) && v == v.Trim()))
            .WithMessage("Description must not be empty, whitespace, or contain leading/trailing whitespace.")
            .Length(PizzaInvariant.DescriptionMinLength, PizzaInvariant.DescriptionMaxLength)
            .WithMessage(
                $"Description size must be within the allowed range ({PizzaInvariant.DescriptionMinLength} – {PizzaInvariant.DescriptionMaxLength})."
            );
    }
}