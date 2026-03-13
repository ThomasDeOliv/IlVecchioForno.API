using FluentValidation;
using IlVecchioForno.Application.Common;

namespace IlVecchioForno.Application.UseCases.Pizzas.ListActivePizzas;

internal sealed class ListActivePizzasQueryValidator : AbstractValidator<ListActivePizzasQuery>
{
    public ListActivePizzasQueryValidator() : base()
    {
        this.RuleFor(q => q.Page)
            .GreaterThanOrEqualTo(QueryDefaultValues.PageNumberMin)
            .WithMessage($"Page must be greater than or equal to {QueryDefaultValues.PageNumberMin}.");

        this.RuleFor(q => q.PageSize)
            .GreaterThanOrEqualTo(QueryDefaultValues.PageSizeMin)
            .LessThanOrEqualTo(QueryDefaultValues.PageSizeMax)
            .WithMessage(
                $"Page size must be within the allowed range ({QueryDefaultValues.PageSizeMin} – {QueryDefaultValues.PageSizeMax})."
            );

        this.RuleFor(q => q.Search)
            .Must(search => search is null || !string.IsNullOrWhiteSpace(search) && search == search.Trim())
            .WithMessage("Search must not be empty, whitespace, or contain leading/trailing whitespace.");

        this.RuleFor(q => q.MinPrice)
            .GreaterThanOrEqualTo(0m)
            .When(q => q.MinPrice.HasValue)
            .WithMessage("Minimal price must be greater than or equal to zero.");

        this.RuleFor(q => q.MaxPrice)
            .GreaterThanOrEqualTo(0m)
            .When(q => q.MaxPrice.HasValue)
            .WithMessage("Maximal price must be greater than or equal to zero.");

        this.RuleFor(q => q)
            .Must(q => !q.MinPrice.HasValue || !q.MaxPrice.HasValue || q.MinPrice <= q.MaxPrice)
            .WithMessage("Minimal price cannot be greater than maximal price.");
    }
}