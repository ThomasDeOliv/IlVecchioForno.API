using FluentValidation;

namespace IlVecchioForno.Application.UseCases.Pizzas.GetActivePizza;

internal sealed class GetActivePizzaQueryValidator : AbstractValidator<GetActivePizzaQuery>
{
    public GetActivePizzaQueryValidator()
    {
        this.RuleFor(q => q.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0.");
    }
}