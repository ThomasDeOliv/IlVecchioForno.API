using FluentValidation;
using IlVecchioForno.Application.Common;

namespace IlVecchioForno.Application.UseCases.QuantityTypes.ListQuantityTypes;

internal sealed class ListQuantityTypesQueryValidator : AbstractValidator<ListQuantityTypesQuery>
{
    public ListQuantityTypesQueryValidator()
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
            .Must(search => search is null || (!string.IsNullOrWhiteSpace(search) && search == search.Trim()))
            .WithMessage("Search must not be empty, whitespace, or contain leading/trailing whitespace.");
    }
}